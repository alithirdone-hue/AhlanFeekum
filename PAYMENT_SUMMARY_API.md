# Payment Summary API Documentation

## Overview
This endpoint retrieves successful payment data for the current authenticated user, grouped by month within a specified date range.

## Endpoint

### Get Payment Summary
**POST** `/api/mobile/payments/summary`

Returns a dictionary of monthly payments and the total payment amount for the current user.

## Authentication
**Required**: Yes - User must be authenticated with a valid JWT token.

## Request

### Headers
```
Authorization: Bearer {your_jwt_token}
Content-Type: application/json
```

### Body (PaymentSummaryRequestDto)
```json
{
  "startDate": "2024-01-01",
  "endDate": "2024-12-31"
}
```

#### Fields
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| startDate | DateOnly | Yes | Start date of the date range (format: "YYYY-MM-DD") |
| endDate | DateOnly | Yes | End date of the date range (format: "YYYY-MM-DD") |

## Response

### Success Response (200 OK)

#### PaymentSummaryResponseDto
```json
{
  "monthlyPayments": {
    "2024-01": 150.00,
    "2024-02": 300.50,
    "2024-03": 225.75,
    "2024-04": 450.00
  },
  "totalPayment": 1126.25,
  "currency": "usd",
  "paymentCount": 12
}
```

#### Response Fields
| Field | Type | Description |
|-------|------|-------------|
| monthlyPayments | Dictionary<string, decimal> | Monthly payment totals grouped by year-month (format: "YYYY-MM") |
| totalPayment | decimal | Total payment amount across all months in the date range |
| currency | string | Currency code (e.g., "usd", "eur") - most common currency if multiple exist |
| paymentCount | int | Total number of successful payments in the date range |

### Error Responses

#### 401 Unauthorized
User is not authenticated.

```json
{
  "error": {
    "message": "User not logged in",
    "code": "UserFriendlyException"
  }
}
```

#### 400 Bad Request
Invalid date range.

```json
{
  "error": {
    "message": "End date must be greater than or equal to start date",
    "code": "UserFriendlyException"
  }
}
```

#### 500 Internal Server Error
Server error during processing.

```json
{
  "error": {
    "message": "Failed to retrieve payment summary: {error details}",
    "code": "UserFriendlyException"
  }
}
```

## Business Logic

### Filtering Criteria
- Only **successful payments** (status = `succeeded`) are included
- Only payments belonging to the **current authenticated user** are included
- Only payments within the specified **date range** (inclusive) are included
- Payments are grouped by the **creation date** (when the payment was created)

### Amount Conversion
- Stripe stores amounts in cents (smallest currency unit)
- The API automatically converts amounts to dollars/main currency unit
- Example: Stripe amount `15000` cents = `150.00` dollars in response

### Monthly Grouping
- Payments are grouped by year and month
- Format: `"YYYY-MM"` (e.g., "2024-01" for January 2024)
- Months are sorted chronologically in the response
- Months with no payments are **not included** in the dictionary

### Currency Handling
- If a user has payments in multiple currencies, the most common currency is returned
- All amounts are shown in their original values (no currency conversion)

## Example Usage

### cURL Example
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/summary" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "startDate": "2024-01-01",
    "endDate": "2024-12-31"
  }'
```

### JavaScript/Fetch Example
```javascript
const getPaymentSummary = async (startDate, endDate) => {
  const response = await fetch('https://admin.srv954186.hstgr.cloud/api/mobile/payments/summary', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${yourJwtToken}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      startDate: startDate,
      endDate: endDate
    })
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  const data = await response.json();
  return data;
};

// Usage
getPaymentSummary('2024-01-01', '2024-12-31')
  .then(summary => {
    console.log('Monthly Payments:', summary.monthlyPayments);
    console.log('Total Payment:', summary.totalPayment);
    console.log('Payment Count:', summary.paymentCount);
  })
  .catch(error => console.error('Error:', error));
```

### Flutter/Dart Example
```dart
import 'dart:convert';
import 'package:http/http.dart' as http;

class PaymentSummaryResponse {
  final Map<String, double> monthlyPayments;
  final double totalPayment;
  final String currency;
  final int paymentCount;

  PaymentSummaryResponse({
    required this.monthlyPayments,
    required this.totalPayment,
    required this.currency,
    required this.paymentCount,
  });

  factory PaymentSummaryResponse.fromJson(Map<String, dynamic> json) {
    return PaymentSummaryResponse(
      monthlyPayments: Map<String, double>.from(json['monthlyPayments']),
      totalPayment: json['totalPayment'].toDouble(),
      currency: json['currency'],
      paymentCount: json['paymentCount'],
    );
  }
}

Future<PaymentSummaryResponse> getPaymentSummary(
  String token,
  DateTime startDate,
  DateTime endDate,
) async {
  final response = await http.post(
    Uri.parse('https://admin.srv954186.hstgr.cloud/api/mobile/payments/summary'),
    headers: {
      'Authorization': 'Bearer $token',
      'Content-Type': 'application/json',
    },
    body: jsonEncode({
      'startDate': '${startDate.year}-${startDate.month.toString().padLeft(2, '0')}-${startDate.day.toString().padLeft(2, '0')}',
      'endDate': '${endDate.year}-${endDate.month.toString().padLeft(2, '0')}-${endDate.day.toString().padLeft(2, '0')}',
    }),
  );

  if (response.statusCode == 200) {
    return PaymentSummaryResponse.fromJson(jsonDecode(response.body));
  } else {
    throw Exception('Failed to load payment summary: ${response.body}');
  }
}

// Usage
void main() async {
  final token = 'your_jwt_token_here';
  final startDate = DateTime(2024, 1, 1);
  final endDate = DateTime(2024, 12, 31);

  try {
    final summary = await getPaymentSummary(token, startDate, endDate);
    print('Monthly Payments: ${summary.monthlyPayments}');
    print('Total Payment: \$${summary.totalPayment}');
    print('Payment Count: ${summary.paymentCount}');
  } catch (e) {
    print('Error: $e');
  }
}
```

### Postman Collection
```json
{
  "info": {
    "name": "Payment Summary API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Get Payment Summary",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Authorization",
            "value": "Bearer {{jwt_token}}",
            "type": "text"
          },
          {
            "key": "Content-Type",
            "value": "application/json",
            "type": "text"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"startDate\": \"2024-01-01T00:00:00Z\",\n  \"endDate\": \"2024-12-31T23:59:59Z\"\n}"
        },
        "url": {
          "raw": "https://admin.srv954186.hstgr.cloud/api/mobile/payments/summary",
          "protocol": "https",
          "host": [
            "admin",
            "srv954186",
            "hstgr",
            "cloud"
          ],
          "path": [
            "api",
            "mobile",
            "payments",
            "summary"
          ]
        }
      },
      "response": []
    }
  ]
}
```

## Use Cases

### 1. Display User's Payment History
Show a monthly breakdown of payments in a dashboard or profile page.

### 2. Generate Financial Reports
Create monthly or yearly financial reports for users.

### 3. Analytics and Insights
Analyze payment patterns and trends over time.

### 4. Tax Reporting
Generate data for tax purposes by filtering specific date ranges.

### 5. Subscription Tracking
Track recurring payments month by month.

## Notes

### Date Range Best Practices
- Use date-only format (no time component needed): "YYYY-MM-DD"
- For a full year: `startDate: "2024-01-01"`, `endDate: "2024-12-31"`
- For a single month: `startDate: "2024-03-01"`, `endDate: "2024-03-31"`
- For last 30 days: Calculate dynamically in your client application
- The API automatically includes the entire day (00:00:00 to 23:59:59) for each date

### Performance Considerations
- Large date ranges may take longer to process
- Consider pagination or limiting date ranges for better performance
- The API loads all matching payments into memory for grouping

### Empty Results
- If no successful payments exist in the date range, the API returns:
  - Empty `monthlyPayments` dictionary
  - `totalPayment`: 0
  - `paymentCount`: 0
  - Default `currency`: "usd"

## Related APIs

- **POST** `/api/mobile/payments/create-intent` - Create a new payment intent
- **POST** `/api/mobile/payments/confirm` - Confirm a payment intent
- **GET** `/api/mobile/payments/{paymentIntentId}` - Get payment intent details
- **POST** `/api/mobile/payments/webhook` - Stripe webhook endpoint

## Support

For issues or questions, please contact the development team or refer to the main API documentation.

## Version History

- **v1.0** (2024-11-27): Initial release
  - Added payment summary endpoint
  - Monthly grouping by creation date
  - Successful payments only
  - Current user filtering

