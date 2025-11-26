# Stripe Webhook Setup Guide

## Quick Setup (5 minutes)

### Step 1: Get Your Webhook Secret

1. **Go to Stripe Dashboard**
   - Visit: https://dashboard.stripe.com/

2. **Navigate to Webhooks**
   - Click **Developers** (left sidebar)
   - Click **Webhooks**

3. **Add Endpoint**
   - Click **"+ Add endpoint"** button
   - Enter this URL: `https://admin.srv954186.hstgr.cloud/api/mobile/payments/webhook`

4. **Select Events**
   Select these 3 events (minimum):
   - ✅ `payment_intent.succeeded`
   - ✅ `payment_intent.payment_failed`
   - ✅ `payment_intent.canceled`
   
   Or click **"Select all events"** to receive everything

5. **Click "Add endpoint"**

6. **Get the Signing Secret**
   - After creating, you'll see the webhook details
   - Find **"Signing secret"**
   - Click **"Click to reveal"**
   - Copy the value (starts with `whsec_`)

### Step 2: Update Configuration

1. Open: `src/AhlanFeekum.Blazor/appsettings.json`

2. Find the Stripe section:
```json
"Stripe": {
  "PublishableKey": "pk_test_xxxxx",
  "SecretKey": "sk_test_xxxxx",
  "ReturnUrl": "https://admin.srv954186.hstgr.cloud/payment/success",
  "WebhookSecret": "whsec_YOUR_WEBHOOK_SECRET_HERE"  // ← Paste here
}
```

3. Replace `whsec_YOUR_WEBHOOK_SECRET_HERE` with your actual secret

4. Save the file

### Step 3: Build & Deploy

```bash
# Build the solution
dotnet build

# Publish if needed
dotnet publish -c Release
```

### Step 4: Test the Webhook

1. **Create a test payment** using your app or the API
2. **Go back to Stripe Dashboard** → **Webhooks**
3. **Click on your endpoint**
4. **View the "Logs" tab** to see webhook delivery attempts
5. **Check your app logs** at `Logs/logs.txt` for webhook processing

---

## What Happens When a Payment Event Occurs?

### 1. Payment Succeeds
```
Stripe → Your Webhook → HandlePaymentSucceededAsync()
```
- ✅ Event logged
- ✅ User ID extracted from metadata
- 🔧 **Your custom logic runs here** (update database, send email, etc.)

### 2. Payment Fails
```
Stripe → Your Webhook → HandlePaymentFailedAsync()
```
- ⚠️ Event logged as warning
- 🔧 **Your custom logic runs here** (notify user, update status, etc.)

### 3. Payment Canceled
```
Stripe → Your Webhook → HandlePaymentCanceledAsync()
```
- ℹ️ Event logged
- 🔧 **Your custom logic runs here** (free inventory, update status, etc.)

---

## Adding Your Business Logic

The webhook handlers are in:  
**File:** `src/AhlanFeekum.Application/UserProfiles/UserProfilesAppService.Extended.cs`

### Example: Update Booking Status on Payment Success

```csharp
// Around line 667
private async Task HandlePaymentSucceededAsync(PaymentIntent paymentIntent)
{
    Log.Information("Payment succeeded for PaymentIntent: {PaymentIntentId}, Amount: {Amount}", 
        paymentIntent.Id, paymentIntent.Amount);

    // Get user and booking IDs from metadata
    if (paymentIntent.Metadata?.ContainsKey("user_id") == true)
    {
        var userId = Guid.Parse(paymentIntent.Metadata["user_id"]);
        var bookingId = paymentIntent.Metadata.ContainsKey("bookingId") 
            ? Guid.Parse(paymentIntent.Metadata["bookingId"]) 
            : (Guid?)null;

        // ✨ ADD YOUR CODE HERE ✨
        
        // Example: Update booking status
        // var booking = await _bookingRepository.GetAsync(bookingId.Value);
        // booking.Status = BookingStatus.Confirmed;
        // booking.PaymentIntentId = paymentIntent.Id;
        // await _bookingRepository.UpdateAsync(booking);

        // Example: Send confirmation email
        // await _emailSender.SendAsync(
        //     paymentIntent.ReceiptEmail,
        //     "Booking Confirmed",
        //     $"Your booking has been confirmed! Payment ID: {paymentIntent.Id}"
        // );

        Log.Information("Booking confirmed for user: {UserId}", userId);
    }

    await Task.CompletedTask;
}
```

---

## Testing Locally (Optional)

If you're developing locally, use **Stripe CLI** to forward webhooks:

### Install Stripe CLI
```bash
# Windows (with Scoop)
scoop bucket add stripe https://github.com/stripe/scoop-stripe-cli.git
scoop install stripe

# Mac
brew install stripe/stripe-cli/stripe

# Or download from: https://stripe.com/docs/stripe-cli
```

### Forward Webhooks to Localhost
```bash
# Login to Stripe
stripe login

# Forward webhooks
stripe listen --forward-to https://localhost:5001/api/mobile/payments/webhook

# You'll get a webhook secret like: whsec_xxxxx
# Use this in your local appsettings.json
```

### Trigger Test Events
```bash
# Trigger a successful payment event
stripe trigger payment_intent.succeeded

# Trigger a failed payment event
stripe trigger payment_intent.payment_failed
```

---

## Verification

### ✅ Webhook is Working If:
1. Stripe Dashboard shows **"200 OK"** responses in webhook logs
2. Your app logs show: `"Stripe webhook received: payment_intent.succeeded"`
3. Your custom logic executes (check database, emails sent, etc.)

### ❌ Webhook is NOT Working If:
- Stripe shows **400** or **500** errors
- App logs show: `"Stripe webhook signature verification failed"`
- No logs appear when payment events occur

### Common Issues:

1. **"Invalid webhook signature"**
   - ❌ Wrong `WebhookSecret` in config
   - ✅ Double-check the secret from Stripe Dashboard

2. **"Webhook secret is not configured"**
   - ❌ Missing or empty `WebhookSecret`
   - ✅ Add the secret to `appsettings.json`

3. **No webhook received**
   - ❌ Wrong URL in Stripe Dashboard
   - ✅ Verify URL: `https://admin.srv954186.hstgr.cloud/api/mobile/payments/webhook`
   - ✅ Check firewall/security settings

4. **SSL/HTTPS errors**
   - ❌ Stripe requires HTTPS for webhooks
   - ✅ Ensure your server has valid SSL certificate

---

## Webhook Security

The webhook implementation includes:
- ✅ **Signature verification** - Ensures events are from Stripe
- ✅ **Automatic retries** - Stripe retries failed webhooks
- ✅ **Idempotency** - Safe to process same event multiple times
- ✅ **Error logging** - All failures are logged

---

## Need Help?

1. Check **Stripe Dashboard** → **Webhooks** → **Your Endpoint** → **Logs**
2. Check your **app logs** at `Logs/logs.txt`
3. Review the webhook code in `UserProfilesAppService.Extended.cs`
4. Test with Stripe CLI for local debugging

---

## Summary

✅ Webhook endpoint: `/api/mobile/payments/webhook`  
✅ Handles: succeeded, failed, canceled events  
✅ Verified: Signature checked on every request  
✅ Logged: All events and errors recorded  
✅ Customizable: Add your logic in handler methods  

**You're all set!** 🎉

