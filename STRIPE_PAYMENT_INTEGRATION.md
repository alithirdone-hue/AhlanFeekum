# Stripe Payment Integration Documentation

## Overview
This document describes the Stripe payment integration added to the AhlanFeekum application. The implementation follows the Stripe PaymentIntent API pattern.

## What Was Added

### 1. NuGet Package
- **Stripe.net v46.5.0** added to `AhlanFeekum.Application` project

### 2. DTOs Created
Located in `src/AhlanFeekum.Application.Contracts/UserProfiles/`:

#### CreatePaymentIntentDto.cs
```csharp
public class CreatePaymentIntentDto
{
    public long Amount { get; set; }              // Amount in cents (e.g., 1099 = $10.99)
    public string Currency { get; set; }          // e.g., "usd", "eur"
    public string? Description { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? ReceiptEmail { get; set; }
    public Guid? UserId { get; set; }
}
```

#### PaymentIntentResponseDto.cs
Contains all payment intent details including:
- Id, ClientSecret, Amount, Currency
- Status, PaymentMethodTypes
- Metadata, and more

#### ConfirmPaymentDto.cs
```csharp
public class ConfirmPaymentDto
{
    public string PaymentIntentId { get; set; }
    public string? PaymentMethodId { get; set; }
}
```

### 3. Service Methods
Added to `UserProfilesAppService.Extended.cs`:

- **CreatePaymentIntentAsync()** - Creates a new payment intent
- **GetPaymentIntentAsync()** - Retrieves an existing payment intent
- **ConfirmPaymentIntentAsync()** - Confirms a payment intent

### 4. API Endpoints
Added to `PaymentController.cs` at route `/api/mobile/payments`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/create-intent` | Create a new payment intent |
| GET | `/{paymentIntentId}` | Get payment intent details |
| POST | `/confirm` | Confirm a payment intent |

## Configuration

### Update appsettings.json
Replace the placeholder values in `src/AhlanFeekum.Blazor/appsettings.json`:

```json
"Stripe": {
  "PublishableKey": "pk_test_YOUR_PUBLISHABLE_KEY_HERE",
  "SecretKey": "sk_test_YOUR_SECRET_KEY_HERE",
  "ReturnUrl": "https://admin.srv954186.hstgr.cloud/payment/success",
  "WebhookSecret": "whsec_YOUR_WEBHOOK_SECRET_HERE"
}
```

**Get your Stripe keys from:**
1. Go to https://dashboard.stripe.com/
2. Navigate to Developers > API keys
3. Copy your **Publishable key** (starts with `pk_test_` or `pk_live_`)
4. Copy your **Secret key** (starts with `sk_test_` or `sk_live_`)

## Usage Examples

### Example 1: Create a Payment Intent

**Request:**
```http
POST /api/mobile/payments/create-intent
Content-Type: application/json

{
  "amount": 1099,
  "currency": "usd",
  "description": "Property booking payment",
  "receiptEmail": "customer@example.com",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "metadata": {
    "propertyId": "123",
    "bookingId": "456"
  }
}
```

**Response:**
```json
{
  "id": "pi_xxxxx",
  "object": "payment_intent",
  "amount": 1099,
  "currency": "usd",
  "clientSecret": "pi_xxxxx_secret_xxxxx",
  "status": "requires_payment_method",
  "description": "Property booking payment",
  "receiptEmail": "customer@example.com",
  "metadata": {
    "propertyId": "123",
    "bookingId": "456",
    "user_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  },
  "paymentMethodTypes": ["card"]
}
```

### Example 2: Get Payment Intent

**Request:**
```http
GET /api/mobile/payments/pi_xxxxx
```

**Response:**
Same structure as create response with updated status.

### Example 3: Confirm Payment Intent

**Request:**
```http
POST /api/mobile/payments/confirm
Content-Type: application/json

{
  "paymentIntentId": "pi_xxxxx",
  "paymentMethodId": "pm_xxxxx"
}
```

**Response:**
Payment intent with status updated to `succeeded`, `requires_action`, etc.

## Payment Statuses

| Status | Description |
|--------|-------------|
| `requires_payment_method` | Waiting for payment method to be attached |
| `requires_confirmation` | Payment method attached, needs confirmation |
| `requires_action` | Additional action required (e.g., 3D Secure) |
| `processing` | Payment is processing |
| `succeeded` | Payment succeeded |
| `canceled` | Payment was canceled |

## Frontend Integration

### Basic Flow

1. **Create Payment Intent** on your backend:
```javascript
const response = await fetch('/api/mobile/payments/create-intent', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    amount: 1099,
    currency: 'usd',
    description: 'Booking payment'
  })
});
const paymentIntent = await response.json();
```

2. **Use Stripe.js on Frontend** to collect payment:
```javascript
const stripe = Stripe('pk_test_YOUR_PUBLISHABLE_KEY');
const elements = stripe.elements();
const cardElement = elements.create('card');
cardElement.mount('#card-element');

// Confirm payment with Stripe
const { error, paymentIntent } = await stripe.confirmCardPayment(
  clientSecret,
  {
    payment_method: {
      card: cardElement,
      billing_details: {
        email: 'customer@example.com'
      }
    }
  }
);

if (error) {
  console.error(error.message);
} else if (paymentIntent.status === 'succeeded') {
  console.log('Payment succeeded!');
}
```

## Testing

### Test Card Numbers
Use these in test mode:

| Card Number | Description |
|-------------|-------------|
| 4242 4242 4242 4242 | Succeeds |
| 4000 0025 0000 3155 | Requires 3D Secure |
| 4000 0000 0000 9995 | Declined |

- **Expiry Date:** Any future date
- **CVC:** Any 3 digits
- **ZIP:** Any 5 digits

## Security Notes

1. ✅ **Never expose Secret Key** - Keep it in appsettings.json/secrets
2. ✅ **Use HTTPS** in production
3. ✅ **Validate amounts** on the server before creating payment intents
4. ✅ **Use Stripe webhooks** to handle payment events reliably
5. ✅ **Store payment intent IDs** in your database for reconciliation

## Webhook Integration (✅ Implemented)

The webhook endpoint is already implemented and ready to use!

### Webhook Endpoint
- **URL:** `https://admin.srv954186.hstgr.cloud/api/mobile/payments/webhook`
- **Method:** POST
- **Authentication:** Webhook signature verification

### Events Handled

The webhook automatically handles these Stripe events:

| Event | Handler | Description |
|-------|---------|-------------|
| `payment_intent.succeeded` | `HandlePaymentSucceededAsync()` | Payment completed successfully |
| `payment_intent.payment_failed` | `HandlePaymentFailedAsync()` | Payment failed |
| `payment_intent.canceled` | `HandlePaymentCanceledAsync()` | Payment was canceled |
| `payment_intent.created` | Logged | New payment intent created |

### Setup in Stripe Dashboard

1. Go to **Developers** → **Webhooks**
2. Click **Add endpoint**
3. Enter endpoint URL: `https://admin.srv954186.hstgr.cloud/api/mobile/payments/webhook`
4. Select events to listen:
   - `payment_intent.succeeded`
   - `payment_intent.payment_failed`
   - `payment_intent.canceled`
5. Copy the **Signing secret** (starts with `whsec_`)
6. Add it to your `appsettings.json`

### Customizing Webhook Handlers

You can add your business logic in these methods in `UserProfilesAppService.Extended.cs`:

```csharp
// Line ~667
private async Task HandlePaymentSucceededAsync(PaymentIntent paymentIntent)
{
    // TODO: Add your custom logic here
    // - Update order status
    // - Send confirmation email
    // - Update booking status
    // - Credit user account
}

// Line ~694
private async Task HandlePaymentFailedAsync(PaymentIntent paymentIntent)
{
    // TODO: Add your custom logic here
    // - Notify user
    // - Update order status
    // - Send retry link
}

// Line ~717
private async Task HandlePaymentCanceledAsync(PaymentIntent paymentIntent)
{
    // TODO: Add your custom logic here
    // - Free inventory
    // - Update status
}
```

## Troubleshooting

### Common Issues

1. **"Invalid API Key"**
   - Verify your Secret Key is correct in appsettings.json
   - Ensure you're using test key in test mode

2. **"Amount must be at least $0.50 usd"**
   - Stripe requires minimum amounts (50 cents for USD)
   - Amount must be in cents (e.g., 1099 = $10.99)

3. **CORS issues**
   - Ensure your domain is added to Stripe Dashboard settings
   - Check CORS configuration in your application

## Production Checklist

Before going live:
- [ ] Replace test keys with live keys
- [ ] Test with real cards (small amounts)
- [ ] Set up webhook endpoint
- [ ] Enable logging for payment failures
- [ ] Add proper error handling and user feedback
- [ ] Test 3D Secure authentication flow
- [ ] Review Stripe Dashboard settings
- [ ] Ensure HTTPS is enabled

## Additional Resources

- [Stripe Documentation](https://stripe.com/docs)
- [Stripe API Reference](https://stripe.com/docs/api)
- [Payment Intents Guide](https://stripe.com/docs/payments/payment-intents)
- [Testing Guide](https://stripe.com/docs/testing)

## Support

For issues or questions:
1. Check Stripe Dashboard logs
2. Review application logs
3. Consult Stripe documentation
4. Contact development team


