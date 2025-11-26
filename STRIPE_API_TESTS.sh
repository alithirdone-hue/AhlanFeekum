#!/bin/bash

# Stripe Payment API Test Script
# Replace BASE_URL with your actual server URL

BASE_URL="https://admin.srv954186.hstgr.cloud"

echo "=========================================="
echo "Stripe Payment API Test Scripts"
echo "=========================================="
echo ""

# Test 1: Create Payment Intent
echo "Test 1: Create Payment Intent"
echo "----------------------------------------"
curl -X POST "${BASE_URL}/api/mobile/payments/create-intent" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 1099,
    "currency": "usd",
    "description": "Test payment for booking",
    "receiptEmail": "test@example.com",
    "metadata": {
      "propertyId": "123",
      "bookingId": "456"
    }
  }' | jq '.'

echo -e "\n"

# Save the payment intent ID from above response and use it below
PAYMENT_INTENT_ID="pi_xxxxx"  # Replace with actual ID from above

# Test 2: Get Payment Intent
echo "Test 2: Get Payment Intent Details"
echo "----------------------------------------"
curl -X GET "${BASE_URL}/api/mobile/payments/${PAYMENT_INTENT_ID}" \
  -H "Content-Type: application/json" | jq '.'

echo -e "\n"

# Test 3: Confirm Payment Intent
echo "Test 3: Confirm Payment Intent"
echo "----------------------------------------"
echo "Note: You need a valid payment method ID from Stripe.js on frontend"
echo "This is typically done on the client side, not via cURL"
# curl -X POST "${BASE_URL}/api/mobile/payments/confirm" \
#   -H "Content-Type: application/json" \
#   -d '{
#     "paymentIntentId": "'${PAYMENT_INTENT_ID}'",
#     "paymentMethodId": "pm_xxxxx"
#   }' | jq '.'

echo ""
echo "=========================================="
echo "Tests Complete!"
echo "=========================================="


