# Payment Summary Implementation Guide

## Overview
This document describes the implementation of the Payment Summary endpoint that retrieves successful payments grouped by month for the current authenticated user.

## Files Created/Modified

### 1. DTOs (Data Transfer Objects)

#### `src/AhlanFeekum.Application.Contracts/UserProfiles/PaymentSummaryRequestDto.cs`
**Purpose**: Request DTO for specifying the date range.

```csharp
public class PaymentSummaryRequestDto
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}
```

#### `src/AhlanFeekum.Application.Contracts/UserProfiles/PaymentSummaryResponseDto.cs`
**Purpose**: Response DTO containing monthly payment breakdown and totals.

```csharp
public class PaymentSummaryResponseDto
{
    public Dictionary<string, decimal> MonthlyPayments { get; set; }
    public decimal TotalPayment { get; set; }
    public string Currency { get; set; }
    public int PaymentCount { get; set; }
}
```

### 2. Service Interface

#### `src/AhlanFeekum.Application.Contracts/UserProfiles/IUserProfilesAppService.Extended.cs`
**Added Method**:
```csharp
Task<PaymentSummaryResponseDto> GetPaymentSummaryAsync(PaymentSummaryRequestDto input);
```

### 3. Service Implementation

#### `src/AhlanFeekum.Application/UserProfiles/UserProfilesAppService.Extended.cs`

**Added Dependencies**:
- `using System.Globalization;` - For date formatting
- `private IUserPaymentRepository _userPaymentRepository` - Repository for querying payments

**Constructor Updated**:
Added `IUserPaymentRepository userPaymentRepository` parameter and initialized the field.

**Method Implementation**:
```csharp
[Authorize]
public virtual async Task<PaymentSummaryResponseDto> GetPaymentSummaryAsync(PaymentSummaryRequestDto input)
{
    // 1. Validate user authentication
    // 2. Validate date range
    // 3. Query successful payments for current user within date range
    // 4. Group by month
    // 5. Calculate totals
    // 6. Return summary
}
```

### 4. API Controller

#### `src/AhlanFeekum.HttpApi/Controllers/Payments/PaymentController.cs`
**Added Endpoint**:
```csharp
[HttpPost("summary")]
public virtual async Task<PaymentSummaryResponseDto> GetPaymentSummaryAsync([FromBody] PaymentSummaryRequestDto input)
{
    return await _userProfilesAppService.GetPaymentSummaryAsync(input);
}
```

**Endpoint URL**: `POST /api/mobile/payments/summary`

## Implementation Details

### Authentication & Authorization
- **Authentication Required**: Yes
- **Authorization**: `[Authorize]` attribute on service method
- **User Context**: Uses `ICurrentUser` to get the authenticated user's ID

### Data Filtering

The implementation filters payments based on:
1. **User**: Only payments belonging to the current authenticated user (`UserProfileId == _currentUser.Id`)
2. **Status**: Only successful payments (`Status == UserPaymentStatus.succeeded`)
3. **Date Range**: Only payments created between `StartDate` and `EndDate` (inclusive)

### Query Implementation

```csharp
var queryable = await _userPaymentRepository.GetQueryableAsync();

var payments = await AsyncExecuter.ToListAsync(
    queryable
        .Where(p => p.UserProfileId == _currentUser.Id.Value)
        .Where(p => p.Status == UserPaymentStatus.succeeded)
        .Where(p => p.CreationTime >= input.StartDate && p.CreationTime <= input.EndDate)
        .OrderBy(p => p.CreationTime)
);
```

### Grouping Logic

Payments are grouped by year-month using LINQ:

```csharp
var monthlyPayments = payments
    .GroupBy(p => new DateTime(p.CreationTime.Year, p.CreationTime.Month, 1))
    .OrderBy(g => g.Key)
    .ToDictionary(
        g => g.Key.ToString("yyyy-MM"),
        g => g.Sum(p => p.Amount / 100m)
    );
```

**Key Points**:
- Groups by the first day of each month
- Formats keys as "yyyy-MM" (e.g., "2024-01")
- Converts amounts from cents to dollars (divides by 100)
- Orders months chronologically

### Amount Conversion

Stripe stores amounts in cents (smallest currency unit). The implementation converts to the main currency unit:

```csharp
p.Amount / 100m  // Converts cents to dollars
```

**Examples**:
- Stripe: `15000` cents → Response: `150.00` dollars
- Stripe: `9999` cents → Response: `99.99` dollars

### Currency Handling

If a user has payments in multiple currencies, the most common one is selected:

```csharp
var currency = payments
    .GroupBy(p => p.Currency)
    .OrderByDescending(g => g.Count())
    .First()
    .Key;
```

### Error Handling

The implementation handles several error scenarios:

1. **User Not Authenticated**:
   ```csharp
   if (_currentUser == null || !_currentUser.Id.HasValue)
       throw new UserFriendlyException("User not logged in");
   ```

2. **Invalid Date Range**:
   ```csharp
   if (input.EndDate < input.StartDate)
       throw new UserFriendlyException("End date must be greater than or equal to start date");
   ```

3. **No Payments Found**:
   Returns an empty response with zero values:
   ```csharp
   return new PaymentSummaryResponseDto
   {
       MonthlyPayments = new Dictionary<string, decimal>(),
       TotalPayment = 0,
       Currency = "usd",
       PaymentCount = 0
   };
   ```

4. **General Exceptions**:
   Logs the error and throws a user-friendly exception:
   ```csharp
   catch (Exception ex)
   {
       _logger.LogError(ex, "Error retrieving payment summary for user {UserId}", _currentUser?.Id);
       throw new UserFriendlyException($"Failed to retrieve payment summary: {ex.Message}");
   }
   ```

## Database Schema

### UserPayment Entity Fields Used
- `Id` (Guid) - Primary key
- `UserProfileId` (Guid) - Foreign key to user
- `Status` (UserPaymentStatus enum) - Payment status
- `Amount` (long) - Amount in cents
- `Currency` (string) - Currency code
- `CreationTime` (DateTime) - When payment was created
- `AmountReceived` (long) - Amount received in cents
- `AmountCapturable` (long) - Amount that can be captured

### UserPaymentStatus Enum
```csharp
public enum UserPaymentStatus
{
    Pending = 1,
    succeeded = 2
}
```

## Performance Considerations

### Current Implementation
- Loads all matching payments into memory
- Performs grouping and aggregation in-memory using LINQ

### Optimization Opportunities
For large datasets, consider:
1. **Database-level aggregation**: Use raw SQL or stored procedures
2. **Pagination**: Limit the date range or add pagination
3. **Caching**: Cache results for frequently requested date ranges
4. **Indexing**: Ensure proper indexes on `UserProfileId`, `Status`, and `CreationTime`

### Recommended Indexes
```sql
CREATE INDEX IX_UserPayments_UserProfileId_Status_CreationTime 
ON AppUserPayments (UserProfileId, Status, CreationTime);
```

## Testing

### Unit Test Scenarios
1. Valid date range with payments
2. Valid date range with no payments
3. Invalid date range (end before start)
4. Unauthenticated user
5. Multiple currencies
6. Single month vs. multiple months
7. Payments at date boundaries

### Integration Test Scenarios
1. End-to-end API call with JWT token
2. Response format validation
3. Amount conversion accuracy
4. Monthly grouping accuracy
5. Error response formats

### Manual Testing
Use the provided Postman collection (`PAYMENT_SUMMARY_POSTMAN.json`) to test various scenarios.

## Security Considerations

### Authentication
- JWT token required in Authorization header
- Token must be valid and not expired

### Authorization
- Users can only see their own payments
- No admin override (can be added if needed)

### Data Validation
- Date range validation prevents invalid queries
- Input sanitization handled by ASP.NET Core model binding

### Sensitive Data
- Payment amounts are visible to the user (owner)
- No credit card or payment method details exposed
- Only aggregated data is returned

## Future Enhancements

### Potential Features
1. **Filter by Currency**: Allow users to filter by specific currency
2. **Filter by Reservation**: Show payments for specific reservations
3. **Export to CSV/Excel**: Download payment summary as a file
4. **Date Range Presets**: Add shortcuts like "Last Month", "Last Quarter", "Last Year"
5. **Charts/Graphs**: Return data formatted for visualization
6. **Admin View**: Allow admins to view any user's payment summary
7. **Refund Tracking**: Include refunded amounts separately
8. **Payment Method Breakdown**: Group by payment method type
9. **Comparison**: Compare current period with previous period

### API Versioning
Consider versioning the API if breaking changes are needed:
- `/api/v1/mobile/payments/summary`
- `/api/v2/mobile/payments/summary`

## Troubleshooting

### Common Issues

#### Issue: Empty Response Despite Having Payments
**Possible Causes**:
- Payments are not in "succeeded" status
- Date range doesn't match payment creation dates
- Wrong user authenticated
- Payments belong to a different user

**Solution**: Check payment status and creation dates in the database.

#### Issue: Incorrect Amounts
**Possible Causes**:
- Currency conversion not applied
- Amounts in different currencies mixed together

**Solution**: Verify the currency and ensure amounts are in cents in the database.

#### Issue: Missing Months
**Expected Behavior**: Months with no payments are not included in the response.

**Solution**: This is by design. If you need all months, implement client-side logic to fill gaps.

#### Issue: 401 Unauthorized
**Possible Causes**:
- JWT token missing or invalid
- Token expired
- User not logged in

**Solution**: Ensure valid JWT token is provided in the Authorization header.

## Related Documentation

- `PAYMENT_SUMMARY_API.md` - Complete API documentation
- `PAYMENT_SUMMARY_POSTMAN.json` - Postman collection for testing
- `STRIPE_PAYMENT_INTEGRATION.md` - Stripe payment integration guide
- `WEBHOOK_SETUP_GUIDE.md` - Webhook setup guide

## Support

For questions or issues:
1. Check the API documentation (`PAYMENT_SUMMARY_API.md`)
2. Review the Postman collection for examples
3. Check application logs for error details
4. Contact the development team

## Version History

- **v1.0** (2024-11-27): Initial implementation
  - Basic payment summary by month
  - Current user filtering
  - Successful payments only
  - Date range filtering

