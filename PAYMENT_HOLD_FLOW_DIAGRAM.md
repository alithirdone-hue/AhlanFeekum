# Payment Hold System - Flow Diagrams

## 🔄 Complete Payment Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│                        PAYMENT HOLD SYSTEM                          │
│                    (24-Hour Automatic Processing)                   │
└─────────────────────────────────────────────────────────────────────┘

┌──────────────┐
│   Customer   │
│  Books Room  │
└──────┬───────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  POST /api/mobile/payments/create-intent-with-hold           │
│  • Amount: $100.00                                           │
│  • Metadata: { bookingId: "xxx" }                           │
│  • CaptureMethod: "manual" ⭐                                │
└──────┬───────────────────────────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  Response: Payment Intent Created                            │
│  • Status: "requires_payment_method"                         │
│  • ClientSecret: "pi_xxx_secret_yyy"                        │
└──────┬───────────────────────────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  Customer Enters Card Details (Frontend - Stripe SDK)        │
│  • Card: 4242 4242 4242 4242                                │
│  • Exp: 12/25, CVC: 123                                     │
└──────┬───────────────────────────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  POST /api/mobile/payments/confirm                           │
│  • PaymentIntentId: "pi_xxx"                                │
│  • PaymentMethodId: "pm_xxx"                                │
└──────┬───────────────────────────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  ✅ Payment Confirmed & Held                                 │
│  • Status: "requires_capture"                               │
│  • Funds: HELD on customer's card                          │
│  • Timer: 24 hours starts NOW ⏰                            │
└──────┬───────────────────────────────────────────────────────┘
       │
       ├─────────────────────┬─────────────────────┐
       │                     │                     │
       ▼                     ▼                     ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│  OPTION 1:   │    │  OPTION 2:   │    │  OPTION 3:   │
│ Host Approves│    │ Host Rejects │    │ Auto-Process │
│ (< 24 hours) │    │ (< 24 hours) │    │ (= 24 hours) │
└──────┬───────┘    └──────┬───────┘    └──────┬───────┘
       │                   │                    │
       ▼                   ▼                    ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│ POST /capture│    │ POST /cancel │    │ Background   │
│ /{id}        │    │ /{id}        │    │ Job Runs     │
└──────┬───────┘    └──────┬───────┘    └──────┬───────┘
       │                   │                    │
       ▼                   ▼                    ├────────┬────────┐
┌──────────────┐    ┌──────────────┐           │        │        │
│ ✅ SUCCEEDED │    │ ❌ CANCELED  │           ▼        ▼        ▼
│ • Money      │    │ • Hold       │    ┌──────────┐ ┌────────┐
│   Transferred│    │   Released   │    │Confirmed?│ │Pending?│
│ • Reservation│    │ • Reservation│    └────┬─────┘ └───┬────┘
│   Approved   │    │   Rejected   │         │           │
└──────────────┘    └──────────────┘         │           │
                                              ▼           ▼
                                        ┌──────────┐ ┌────────┐
                                        │ CAPTURE  │ │ CANCEL │
                                        └──────────┘ └────────┘
```

---

## 📱 Frontend Integration Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    MOBILE APP (Flutter)                     │
└─────────────────────────────────────────────────────────────┘

1️⃣  User selects property & dates
    ↓
2️⃣  App calculates total amount
    ↓
3️⃣  User clicks "Book Now"
    ↓
    ┌─────────────────────────────────────────────────────┐
    │  final payment = await createPaymentWithHold(       │
    │    amount: 10000,                                   │
    │    bookingId: bookingId,                           │
    │  );                                                 │
    └─────────────────────────────────────────────────────┘
    ↓
4️⃣  Show card input form (Stripe SDK)
    ↓
5️⃣  User enters card details
    ↓
    ┌─────────────────────────────────────────────────────┐
    │  await Stripe.instance.confirmPayment(              │
    │    paymentIntentClientSecret: payment.clientSecret, │
    │  );                                                 │
    └─────────────────────────────────────────────────────┘
    ↓
6️⃣  ✅ Success! Show confirmation screen
    ┌─────────────────────────────────────────────────────┐
    │  "Booking Confirmed! 🎉"                            │
    │  "Your payment of $100 is on hold."                │
    │  "You won't be charged until the host confirms."   │
    │  "The host has 24 hours to respond."              │
    └─────────────────────────────────────────────────────┘
```

---

## 🏢 Admin Dashboard Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    HOST DASHBOARD                           │
└─────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│  📋 Pending Bookings                                     │
├──────────────────────────────────────────────────────────┤
│  Booking #1234                                           │
│  • Guest: John Doe                                       │
│  • Amount: $100.00 (ON HOLD)                            │
│  • Time Remaining: 18 hours                             │
│  • Property: Luxury Villa                               │
│                                                          │
│  [✅ Approve]  [❌ Reject]                               │
└──────────────────────────────────────────────────────────┘
       │              │
       ▼              ▼
┌──────────────┐  ┌──────────────┐
│ Host Clicks  │  │ Host Clicks  │
│ "Approve"    │  │ "Reject"     │
└──────┬───────┘  └──────┬───────┘
       │                 │
       ▼                 ▼
┌──────────────────┐  ┌──────────────────┐
│ Backend calls:   │  │ Backend calls:   │
│ CapturePayment() │  │ CancelPayment()  │
└──────┬───────────┘  └──────┬───────────┘
       │                     │
       ▼                     ▼
┌──────────────────┐  ┌──────────────────┐
│ ✅ Payment       │  │ ❌ Payment       │
│    Captured      │  │    Canceled      │
│ • Guest charged  │  │ • Hold released  │
│ • Booking active │  │ • Booking closed │
│ • Email sent     │  │ • Email sent     │
└──────────────────┘  └──────────────────┘
```

---

## ⏰ Background Job Flow

```
┌─────────────────────────────────────────────────────────────┐
│              BACKGROUND JOB (Runs Every Hour)               │
└─────────────────────────────────────────────────────────────┘

                    ⏰ Cron Job Triggers
                            ↓
        ┌───────────────────────────────────────┐
        │ POST /api/mobile/payments/            │
        │      process-pending                  │
        └───────────────┬───────────────────────┘
                        ↓
        ┌───────────────────────────────────────┐
        │ Query Database:                       │
        │ • Status = "requires_capture"         │
        │ • CreatedAt < (Now - 24 hours)       │
        └───────────────┬───────────────────────┘
                        ↓
                ┌───────────────┐
                │ Found 5       │
                │ Payments      │
                └───────┬───────┘
                        ↓
        ┌───────────────────────────────────────┐
        │ For Each Payment:                     │
        │ 1. Get Reservation                    │
        │ 2. Check Status                       │
        │ 3. Capture or Cancel                  │
        └───────────────┬───────────────────────┘
                        ↓
        ┌───────────────────────────────────────┐
        │ Payment #1:                           │
        │ • Reservation: "Confirmed" ✅         │
        │ • Action: CAPTURE                     │
        │ • Result: Payment Succeeded           │
        └───────────────┬───────────────────────┘
                        ↓
        ┌───────────────────────────────────────┐
        │ Payment #2:                           │
        │ • Reservation: "Pending" ⏳           │
        │ • Action: CANCEL                      │
        │ • Result: Payment Canceled            │
        └───────────────┬───────────────────────┘
                        ↓
        ┌───────────────────────────────────────┐
        │ Log Results:                          │
        │ • Processed: 5 payments               │
        │ • Captured: 2 payments                │
        │ • Canceled: 3 payments                │
        │ • Errors: 0                           │
        └───────────────────────────────────────┘
```

---

## 🔄 Status Transitions

```
┌─────────────────────────────────────────────────────────────┐
│                  PAYMENT STATUS LIFECYCLE                   │
└─────────────────────────────────────────────────────────────┘

    [Pending]
       │
       │ Customer confirms payment
       ▼
[requires_capture] ⏰ 24-hour timer
       │
       ├──────────────┬──────────────┐
       │              │              │
       │ Manual       │ Manual       │ Auto-process
       │ Capture      │ Cancel       │ (24 hours)
       ▼              ▼              ▼
  [succeeded]    [canceled]    [Check Reservation]
       │              │              │
       │              │              ├──────────┬──────────┐
       │              │              │          │          │
       │              │              ▼          ▼          ▼
       │              │         Confirmed   Pending    Other
       │              │              │          │          │
       │              │              ▼          ▼          ▼
       │              │         [succeeded] [canceled] [canceled]
       │              │
       └──────────────┴──────────────┘
                      │
                      ▼
              [Transaction Complete]
```

---

## 📊 Timeline View

```
┌─────────────────────────────────────────────────────────────┐
│                    24-HOUR TIMELINE                         │
└─────────────────────────────────────────────────────────────┘

Hour 0  ├──────────────────────────────────────────────────┤  Hour 24
        │                                                  │
        ▼                                                  ▼
   Payment Held                                    Auto-Process
   (requires_capture)                              (if no action)
        │                                                  │
        │                                                  │
        ├──────────┬──────────┬──────────┬────────────────┤
        │          │          │          │                │
        ▼          ▼          ▼          ▼                ▼
     Hour 0     Hour 6     Hour 12    Hour 18         Hour 24
        │          │          │          │                │
        │          │          │          │                │
    ✅ Can      ✅ Can      ✅ Can      ✅ Can          ⚠️ Auto
    Capture    Capture    Capture    Capture        Process
    ✅ Can      ✅ Can      ✅ Can      ✅ Can          
    Cancel     Cancel     Cancel     Cancel         

Notifications:
    │          │          │          │                │
    ▼          ▼          ▼          ▼                ▼
  "Booking   "12h      "6h        "2h            "Processing
   Pending"   left"     left"      left"          automatically"
```

---

## 🎯 Decision Tree

```
                    Payment Created with Hold
                            │
                            ▼
                    Customer Confirms?
                            │
                ┌───────────┴───────────┐
                │                       │
                ▼                       ▼
              YES                      NO
                │                       │
                ▼                       ▼
        Status: requires_capture    Expires after 24h
                │                       │
                ▼                       ▼
        Host responds < 24h?        [canceled]
                │
    ┌───────────┴───────────┐
    │                       │
    ▼                       ▼
  YES                      NO
    │                       │
    ├──────┬──────┐         ▼
    │      │      │    Auto-Process
    ▼      ▼      ▼         │
Approve  Reject  ?          ▼
    │      │      │    Reservation
    ▼      ▼      ▼    Confirmed?
Capture Cancel  Wait      │
    │      │      │    ┌──┴──┐
    ▼      ▼      │    │     │
succeeded canceled│   YES   NO
                  │    │     │
                  │    ▼     ▼
                  │ Capture Cancel
                  │    │     │
                  │    ▼     ▼
                  │succeeded canceled
                  │
                  └──────────┘
```

---

## 🔔 Notification Flow

```
┌─────────────────────────────────────────────────────────────┐
│                  NOTIFICATION TIMELINE                      │
└─────────────────────────────────────────────────────────────┘

Payment Held (Hour 0)
├─> 📧 Customer: "Booking pending - payment on hold"
└─> 📧 Host: "New booking request - confirm within 24h"

12 Hours Later
└─> 📧 Host: "Reminder: 12 hours left to confirm booking"

18 Hours Later
└─> 📧 Host: "Urgent: 6 hours left to confirm booking"

23 Hours Later
└─> 📧 Host: "Final reminder: 1 hour left!"

24 Hours - Host Approved
├─> 📧 Customer: "Booking confirmed! Payment processed"
└─> 📧 Host: "Booking confirmed - guest will arrive soon"

24 Hours - Host Didn't Respond
├─> 📧 Customer: "Booking canceled - payment hold released"
└─> 📧 Host: "Booking expired - you didn't respond in time"
```

---

## 📱 Mobile App Screens

```
┌─────────────────────────────────────────────────────────────┐
│                    BOOKING FLOW SCREENS                     │
└─────────────────────────────────────────────────────────────┘

Screen 1: Property Details
┌──────────────────────────┐
│  Luxury Villa            │
│  $100/night              │
│  [Book Now]              │
└──────────────────────────┘
         ↓
Screen 2: Payment Details
┌──────────────────────────┐
│  Enter Card Details      │
│  ┌────────────────────┐  │
│  │ 4242 4242 4242 ... │  │
│  └────────────────────┘  │
│  [Confirm Booking]       │
└──────────────────────────┘
         ↓
Screen 3: Processing
┌──────────────────────────┐
│  Processing...           │
│  🔄                      │
└──────────────────────────┘
         ↓
Screen 4: Success
┌──────────────────────────┐
│  ✅ Booking Confirmed!   │
│                          │
│  Your payment of $100    │
│  is on hold.             │
│                          │
│  You won't be charged    │
│  until the host confirms.│
│                          │
│  Time remaining: 24h     │
│  [View Booking]          │
└──────────────────────────┘
```

This visual documentation should help everyone understand how the payment hold system works! 🎉

