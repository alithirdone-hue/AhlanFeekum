# Payment Hold System - Quick Reference

## 🚀 Quick Start

### 1. Create Payment with Hold
```bash
POST /api/mobile/payments/create-intent-with-hold
```
```json
{
  "amount": 10000,
  "currency": "usd",
  "metadata": { "bookingId": "xxx" }
}
```
**Result:** Payment intent created, waiting for customer confirmation

---

### 2. Confirm Payment (Frontend)
```bash
POST /api/mobile/payments/confirm
```
```json
{
  "paymentIntentId": "pi_xxx",
  "paymentMethodId": "pm_xxx"
}
```
**Result:** Funds held on card, status = `requires_capture`, 24-hour timer starts

---

### 3A. Capture (Approve Booking)
```bash
POST /api/mobile/payments/capture/{paymentIntentId}
```
**Result:** Money transferred, status = `succeeded`, reservation approved

---

### 3B. Cancel (Reject Booking)
```bash
POST /api/mobile/payments/cancel/{paymentIntentId}
```
**Result:** Hold released, status = `canceled`, reservation rejected

---

## 📊 Payment Status Flow

```
Pending → requires_capture → succeeded ✅
                          └→ canceled ❌
```

## ⏰ Automatic Processing

**After 24 hours:**
- ✅ **Reservation Confirmed** → Auto-Capture
- ❌ **Reservation Not Confirmed** → Auto-Cancel

**Setup Cron Job:**
```bash
# Run every hour
0 * * * * curl -X POST "https://your-api.com/api/mobile/payments/process-pending" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## 🔑 Key Differences

| Feature | Regular Payment | Payment with Hold |
|---------|----------------|-------------------|
| Endpoint | `/create-intent` | `/create-intent-with-hold` |
| Capture | Automatic | Manual (24 hours) |
| Status After Confirm | `succeeded` | `requires_capture` |
| Can Cancel? | No | Yes (within 24 hours) |
| Use Case | Instant purchase | Booking confirmation |

---

## 📱 Frontend Integration

### Flutter
```dart
// 1. Create payment with hold
final payment = await createPaymentWithHold(
  amount: 10000,
  bookingId: bookingId,
);

// 2. Confirm with Stripe SDK
await Stripe.instance.confirmPayment(
  paymentIntentClientSecret: payment['clientSecret'],
);

// ✅ Done! Payment is on hold for 24 hours
```

### JavaScript
```javascript
// 1. Create payment with hold
const payment = await createPaymentWithHold(10000, bookingId);

// 2. Confirm with Stripe.js
const {paymentIntent} = await stripe.confirmCardPayment(
  payment.clientSecret,
  { payment_method: { card: cardElement } }
);

// ✅ Done! Payment is on hold for 24 hours
```

---

## 🧪 Testing

**Test Card:** `4242 4242 4242 4242`

**Test Flow:**
1. Create payment with hold
2. Confirm payment → Status: `requires_capture`
3. Wait or manually capture/cancel
4. Verify final status

---

## ⚠️ Important Notes

- ⏰ Stripe max hold: **7 days**
- ⏰ Your system: **24 hours** (configurable)
- 🔒 Capture/Cancel require authorization
- 📧 Notify customers about hold status
- 🔔 Remind hosts to confirm within 24 hours

---

## 🆘 Troubleshooting

| Problem | Solution |
|---------|----------|
| Payment captured immediately | Use `/create-intent-with-hold` endpoint |
| Cannot capture after 24 hours | Check if > 7 days (Stripe auto-canceled) |
| Auto-process not working | Verify cron job is running |
| Status stuck at `requires_capture` | Run `/process-pending` manually |

---

## 📚 Full Documentation

- **Complete Guide:** [PAYMENT_HOLD_GUIDE.md](PAYMENT_HOLD_GUIDE.md)
- **Postman Collection:** [PAYMENT_HOLD_POSTMAN.json](PAYMENT_HOLD_POSTMAN.json)
- **Stripe Docs:** https://stripe.com/docs/payments/capture-later

---

## 🎯 Use Cases

✅ **Perfect For:**
- Property bookings (Airbnb-style)
- Service confirmations
- Reservation deposits
- Fraud prevention

❌ **Not Suitable For:**
- Instant purchases
- Digital products
- Subscriptions

