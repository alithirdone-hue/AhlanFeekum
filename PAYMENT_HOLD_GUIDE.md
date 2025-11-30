# Payment Hold System - Complete Guide

## Overview

This system implements **Stripe Payment Intent with Manual Capture**, allowing you to:
- ✅ **Hold payments** for up to 24 hours (or 7 days max by Stripe)
- ✅ **Automatically capture or cancel** after 24 hours based on reservation status
- ✅ **Manual control** to capture or cancel before the 24-hour window

## How It Works

### Payment Flow

```
1. Customer Books Property
   └─> Create Payment Intent with Hold
       Status: "Pending"

2. Customer Confirms Payment (Frontend)
   └─> Confirm Payment Intent
       Status: "requires_capture" (Funds held on card)
       ⏰ 24-hour timer starts

3. Within 24 Hours - Two Options:

   Option A: Manual Action
   ├─> Host Approves → Capture Payment
   │   Status: "succeeded" (Money transferred)
   │   Reservation: "Approved"
   │
   └─> Host Rejects → Cancel Payment
       Status: "canceled" (Hold released)
       Reservation: "Rejected"

   Option B: Automatic Action (After 24 hours)
   ├─> If Reservation = "Confirmed/Approved" → Auto-Capture
   │   Status: "succeeded"
   │
   └─> If Reservation = Other Status → Auto-Cancel
       Status: "canceled"
```

## API Endpoints

### 1. Create Payment Intent with Hold

**Endpoint:** `POST /api/mobile/payments/create-intent-with-hold`

**Description:** Creates a payment intent that holds funds instead of capturing immediately.

**Request:**
```json
{
  "amount": 10000,
  "currency": "usd",
  "description": "Property booking - 3 nights",
  "receiptEmail": "customer@example.com",
  "metadata": {
    "bookingId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "propertyName": "Luxury Villa"
  }
}
```

**Response:**
```json
{
  "id": "pi_3QKxxx...",
  "clientSecret": "pi_3QKxxx..._secret_xxx",
  "status": "requires_payment_method",
  "amount": 10000,
  "currency": "usd",
  "description": "Property booking - 3 nights"
}
```

**cURL Example:**
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/create-intent-with-hold" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 10000,
    "currency": "usd",
    "description": "Property booking",
    "receiptEmail": "customer@example.com",
    "metadata": {
      "bookingId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  }'
```

---

### 2. Confirm Payment (Frontend)

**Endpoint:** `POST /api/mobile/payments/confirm`

**Description:** Confirms the payment with a payment method (holds the funds).

**Request:**
```json
{
  "paymentIntentId": "pi_3QKxxx...",
  "paymentMethodId": "pm_xxx..."
}
```

**Response:**
```json
{
  "id": "pi_3QKxxx...",
  "status": "requires_capture",
  "amount": 10000,
  "amountCapturable": 10000,
  "amountReceived": 0
}
```

**Note:** After this step, the payment status becomes `requires_capture` and the 24-hour timer starts.

---

### 3. Capture Payment (Complete Transaction)

**Endpoint:** `POST /api/mobile/payments/capture/{paymentIntentId}`

**Description:** Captures the held payment and transfers the money.

**Request:**
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/capture/pi_3QKxxx..." \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Response:**
```json
{
  "id": "pi_3QKxxx...",
  "status": "succeeded",
  "amount": 10000,
  "amountCapturable": 0,
  "amountReceived": 10000
}
```

**When to Use:**
- ✅ Host confirms the booking
- ✅ Service is verified
- ✅ Customer check-in completed

---

### 4. Cancel Payment (Release Hold)

**Endpoint:** `POST /api/mobile/payments/cancel/{paymentIntentId}`

**Description:** Cancels the held payment and releases the authorization.

**Request:**
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/cancel/pi_3QKxxx..." \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Response:**
```json
{
  "id": "pi_3QKxxx...",
  "status": "canceled",
  "amount": 10000,
  "amountCapturable": 0,
  "amountReceived": 0
}
```

**When to Use:**
- ❌ Host rejects the booking
- ❌ Property not available
- ❌ Customer cancels before confirmation

---

### 5. Process Pending Payments (Background Job)

**Endpoint:** `POST /api/mobile/payments/process-pending`

**Description:** Automatically processes all payments older than 24 hours.

**Request:**
```bash
curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/process-pending" \
  -H "Authorization: Bearer YOUR_ADMIN_JWT_TOKEN"
```

**Response:**
```json
{
  "success": true,
  "message": "Pending payments processed successfully"
}
```

**Setup as Cron Job (Linux):**
```bash
# Edit crontab
crontab -e

# Add this line to run every hour
0 * * * * curl -X POST "https://admin.srv954186.hstgr.cloud/api/mobile/payments/process-pending" -H "Authorization: Bearer YOUR_TOKEN"
```

**Setup with Hangfire (Recommended):**
```csharp
// In your Hangfire configuration
RecurringJob.AddOrUpdate(
    "process-pending-payments",
    () => ProcessPendingPaymentsJob(),
    Cron.Hourly);
```

---

## Frontend Integration

### Flutter Example

```dart
import 'package:flutter_stripe/flutter_stripe.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class PaymentService {
  final String baseUrl = 'https://admin.srv954186.hstgr.cloud';
  final String jwtToken = 'YOUR_JWT_TOKEN';

  // Step 1: Create Payment Intent with Hold
  Future<Map<String, dynamic>> createPaymentWithHold({
    required int amount,
    required String currency,
    required String bookingId,
    String? description,
  }) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/mobile/payments/create-intent-with-hold'),
      headers: {
        'Authorization': 'Bearer $jwtToken',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'amount': amount,
        'currency': currency,
        'description': description,
        'metadata': {
          'bookingId': bookingId,
        },
      }),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Failed to create payment intent');
    }
  }

  // Step 2: Confirm Payment with Card (Holds the funds)
  Future<void> confirmPaymentWithHold({
    required String clientSecret,
    required String cardNumber,
    required String expMonth,
    required String expYear,
    required String cvc,
  }) async {
    // Initialize payment sheet
    await Stripe.instance.initPaymentSheet(
      paymentSheetParameters: SetupPaymentSheetParameters(
        paymentIntentClientSecret: clientSecret,
        merchantDisplayName: 'Your App Name',
        style: ThemeMode.system,
      ),
    );

    // Present payment sheet
    await Stripe.instance.presentPaymentSheet();
    
    // Payment is now on hold (requires_capture status)
    print('Payment held successfully - awaiting capture');
  }

  // Complete booking flow
  Future<void> bookPropertyWithHold({
    required String bookingId,
    required int amount,
    required String cardNumber,
    required String expMonth,
    required String expYear,
    required String cvc,
  }) async {
    try {
      // Step 1: Create payment intent with hold
      final paymentIntent = await createPaymentWithHold(
        amount: amount,
        currency: 'usd',
        bookingId: bookingId,
        description: 'Property booking',
      );

      // Step 2: Confirm payment (holds the funds)
      await confirmPaymentWithHold(
        clientSecret: paymentIntent['clientSecret'],
        cardNumber: cardNumber,
        expMonth: expMonth,
        expYear: expYear,
        cvc: cvc,
      );

      print('Booking created! Payment on hold for 24 hours.');
      print('Host has 24 hours to confirm or it will auto-process.');
      
    } catch (e) {
      print('Booking failed: $e');
      rethrow;
    }
  }
}
```

### JavaScript Example

```javascript
// Step 1: Create Payment Intent with Hold
async function createPaymentWithHold(amount, bookingId) {
  const response = await fetch('https://admin.srv954186.hstgr.cloud/api/mobile/payments/create-intent-with-hold', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${jwtToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      amount: amount,
      currency: 'usd',
      description: 'Property booking',
      metadata: {
        bookingId: bookingId
      }
    })
  });
  
  return await response.json();
}

// Step 2: Confirm Payment with Stripe.js
async function confirmPaymentWithHold(clientSecret, cardElement) {
  const stripe = Stripe('pk_test_YOUR_PUBLISHABLE_KEY');
  
  const {error, paymentIntent} = await stripe.confirmCardPayment(clientSecret, {
    payment_method: {
      card: cardElement,
    }
  });
  
  if (error) {
    console.error('Payment failed:', error);
    throw error;
  }
  
  console.log('Payment held successfully:', paymentIntent.status);
  // Status will be "requires_capture"
  return paymentIntent;
}

// Complete flow
async function bookProperty(amount, bookingId, cardElement) {
  try {
    // Create payment intent with hold
    const paymentIntent = await createPaymentWithHold(amount, bookingId);
    
    // Confirm payment (holds the funds)
    await confirmPaymentWithHold(paymentIntent.clientSecret, cardElement);
    
    alert('Booking successful! Payment on hold for 24 hours.');
  } catch (error) {
    alert('Booking failed: ' + error.message);
  }
}
```

---

## Backend Admin Actions

### Capture Payment (Approve Booking)

```csharp
// When host approves the booking
public async Task ApproveBooking(Guid reservationId)
{
    // Get the payment for this reservation
    var payment = await _userPaymentRepository.FirstOrDefaultAsync(
        p => p.ReservationId == reservationId && 
        p.Status == UserPaymentStatus.requires_capture
    );
    
    if (payment != null)
    {
        // Capture the payment
        await _userProfilesAppService.CapturePaymentAsync(payment.StripPaymentId);
        
        // Reservation status is automatically updated to Approved
    }
}
```

### Cancel Payment (Reject Booking)

```csharp
// When host rejects the booking
public async Task RejectBooking(Guid reservationId)
{
    // Get the payment for this reservation
    var payment = await _userPaymentRepository.FirstOrDefaultAsync(
        p => p.ReservationId == reservationId && 
        p.Status == UserPaymentStatus.requires_capture
    );
    
    if (payment != null)
    {
        // Cancel the payment
        await _userProfilesAppService.CancelPaymentAsync(payment.StripPaymentId);
        
        // Reservation status is automatically updated to Rejected
    }
}
```

---

## Payment Status Flow

| Status | Description | Next Action |
|--------|-------------|-------------|
| `Pending` | Payment intent created | Customer confirms payment |
| `requires_capture` | Funds held on card | Capture or cancel within 24 hours |
| `succeeded` | Payment captured | Transaction complete |
| `canceled` | Payment canceled | Hold released |
| `failed` | Payment failed | Retry payment |

---

## Important Notes

### Hold Duration
- ⏰ **Your System:** 24 hours (configurable)
- ⏰ **Stripe Maximum:** 7 days
- ⏰ **Recommendation:** Keep it under 7 days to avoid automatic cancellation

### Automatic Processing
- ✅ Runs every hour (or as configured)
- ✅ Checks payments older than 24 hours
- ✅ Captures if reservation is Confirmed/Approved
- ✅ Cancels if reservation is any other status

### Best Practices
1. ✅ **Notify customers** when payment is on hold
2. ✅ **Notify hosts** to confirm within 24 hours
3. ✅ **Send reminders** at 12 hours and 23 hours
4. ✅ **Log all actions** for audit trail
5. ✅ **Monitor the background job** to ensure it runs

### Security
- 🔒 Capture/Cancel endpoints require authorization
- 🔒 Only authorized users can process payments
- 🔒 All actions are logged
- 🔒 Webhook signature verification enabled

---

## Testing

### Test with Stripe Test Cards

```
Card Number: 4242 4242 4242 4242
Exp: Any future date (e.g., 12/25)
CVC: Any 3 digits (e.g., 123)
```

### Test Flow

1. **Create payment with hold**
   ```bash
   curl -X POST ".../create-intent-with-hold" -d '{"amount":10000,...}'
   ```

2. **Confirm payment** (use Stripe CLI or frontend)
   ```bash
   stripe payment_intents confirm pi_xxx --payment-method pm_xxx
   ```

3. **Check status** - Should be `requires_capture`

4. **Capture payment**
   ```bash
   curl -X POST ".../capture/pi_xxx"
   ```

5. **Verify** - Status should be `succeeded`

---

## Troubleshooting

### Payment Not Captured After 24 Hours
- ✅ Check if background job is running
- ✅ Check logs for errors
- ✅ Verify reservation status

### Cannot Capture Payment
- ❌ Payment might be older than 7 days (Stripe auto-canceled)
- ❌ Payment already captured or canceled
- ❌ Invalid payment intent ID

### Customer Charged Immediately
- ❌ Used wrong endpoint (use `create-intent-with-hold`, not `create-intent`)
- ❌ CaptureMethod not set to "manual"

---

## Support

For issues or questions:
- 📧 Email: support@yourapp.com
- 📚 Stripe Docs: https://stripe.com/docs/payments/capture-later
- 🔧 API Reference: See PAYMENT_HOLD_API_REFERENCE.md

