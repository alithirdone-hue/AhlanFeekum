# Payment Summary API - Quick Start Guide

## What Was Added

A new endpoint to retrieve successful payments for the current user, grouped by month within a specified date range.

## Endpoint

```
POST /api/mobile/payments/summary
```

## Quick Example

### Request
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/summary" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-12-31T23:59:59Z"
  }'
```

### Response
```json
{
  "monthlyPayments": {
    "2024-01": 150.00,
    "2024-02": 300.50,
    "2024-03": 225.75
  },
  "totalPayment": 676.25,
  "currency": "usd",
  "paymentCount": 8
}
```

## Files Created

### DTOs (Application.Contracts)
1. `PaymentSummaryRequestDto.cs` - Request with start/end dates
2. `PaymentSummaryResponseDto.cs` - Response with monthly breakdown

### Service Layer (Application)
3. `IUserProfilesAppService.Extended.cs` - Added method signature
4. `UserProfilesAppService.Extended.cs` - Implemented business logic

### API Layer (HttpApi)
5. `PaymentController.cs` - Added endpoint

### Documentation
6. `PAYMENT_SUMMARY_API.md` - Complete API documentation
7. `PAYMENT_SUMMARY_POSTMAN.json` - Postman collection
8. `PAYMENT_SUMMARY_IMPLEMENTATION.md` - Implementation details
9. `PAYMENT_SUMMARY_QUICK_START.md` - This file

## Key Features

✅ **User-Specific**: Only shows payments for the authenticated user
✅ **Successful Only**: Filters for succeeded payments only
✅ **Date Range**: Flexible date range filtering
✅ **Monthly Grouping**: Automatically groups by year-month (YYYY-MM)
✅ **Amount Conversion**: Converts from cents to dollars automatically
✅ **Total Calculation**: Provides total payment amount
✅ **Payment Count**: Shows number of successful payments
✅ **Error Handling**: Comprehensive error handling and validation

## How It Works

1. **Authentication**: User must be logged in with valid JWT token
2. **Query**: Fetches all successful payments for the user in date range
3. **Group**: Groups payments by month (format: "2024-01", "2024-02", etc.)
4. **Calculate**: Sums amounts for each month and calculates total
5. **Return**: Returns dictionary of monthly payments + total

## Important Notes

### Amount Format
- Stripe stores amounts in **cents** (e.g., 15000 = $150.00)
- API returns amounts in **dollars** (e.g., 150.00)
- Automatic conversion: `amount / 100`

### Date Format
- Use **ISO 8601** format: `"2024-01-01T00:00:00Z"`
- Always use **UTC** timezone (Z suffix)
- Inclusive range (includes both start and end dates)

### Monthly Keys
- Format: **"YYYY-MM"** (e.g., "2024-01" for January 2024)
- Only months with payments are included
- Months without payments are **not** in the dictionary

### Status Filter
- Only **succeeded** payments are included
- Pending payments are **excluded**
- Failed/canceled payments are **excluded**

## Testing with Postman

### Step 1: Import Collection
1. Open Postman
2. Click "Import"
3. Select `PAYMENT_SUMMARY_POSTMAN.json`
4. Collection will be imported with 7 pre-configured requests

### Step 2: Set Variables
1. Click on the collection name
2. Go to "Variables" tab
3. Set `jwt_token` to your actual JWT token
4. Set `base_url` if different from default

### Step 3: Run Requests
- Try "Payment Summary - Full Year 2024" first
- Adjust dates as needed for your data
- Test error scenarios with invalid requests

## Common Use Cases

### 1. Display User's Payment History
```javascript
// Show monthly breakdown in a chart or table
const summary = await getPaymentSummary('2024-01-01', '2024-12-31');
displayChart(summary.monthlyPayments);
```

### 2. Calculate Year-to-Date Total
```javascript
const startOfYear = new Date(new Date().getFullYear(), 0, 1);
const today = new Date();
const summary = await getPaymentSummary(startOfYear, today);
console.log(`YTD Total: $${summary.totalPayment}`);
```

### 3. Compare Months
```javascript
const thisMonth = await getPaymentSummary('2024-11-01', '2024-11-30');
const lastMonth = await getPaymentSummary('2024-10-01', '2024-10-31');
const change = thisMonth.totalPayment - lastMonth.totalPayment;
console.log(`Change: $${change}`);
```

### 4. Generate Report
```javascript
const summary = await getPaymentSummary('2024-01-01', '2024-12-31');
generatePDF({
  title: 'Annual Payment Report',
  monthlyData: summary.monthlyPayments,
  total: summary.totalPayment,
  count: summary.paymentCount
});
```

## Error Responses

### 401 Unauthorized
```json
{
  "error": {
    "message": "User not logged in"
  }
}
```
**Fix**: Provide valid JWT token in Authorization header

### 400 Bad Request
```json
{
  "error": {
    "message": "End date must be greater than or equal to start date"
  }
}
```
**Fix**: Ensure endDate >= startDate

## Next Steps

1. **Read Full Documentation**: See `PAYMENT_SUMMARY_API.md` for complete details
2. **Test with Postman**: Use `PAYMENT_SUMMARY_POSTMAN.json` collection
3. **Integrate in Your App**: Use the examples in the documentation
4. **Review Implementation**: See `PAYMENT_SUMMARY_IMPLEMENTATION.md` for technical details

## Support

- **API Docs**: `PAYMENT_SUMMARY_API.md`
- **Postman Collection**: `PAYMENT_SUMMARY_POSTMAN.json`
- **Implementation Guide**: `PAYMENT_SUMMARY_IMPLEMENTATION.md`

## Quick Reference

| Aspect | Details |
|--------|---------|
| **Method** | POST |
| **URL** | `/api/mobile/payments/summary` |
| **Auth** | Required (JWT Bearer token) |
| **Request** | `{ startDate, endDate }` |
| **Response** | `{ monthlyPayments, totalPayment, currency, paymentCount }` |
| **Filters** | Current user + Succeeded status + Date range |
| **Grouping** | By month (YYYY-MM format) |
| **Amounts** | Converted from cents to dollars |

---

**Version**: 1.0  
**Last Updated**: 2024-11-27

