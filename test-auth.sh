#!/bin/bash

# Smart Plant Care API - JWT Authentication Test Script
# Make sure your API is running on http://localhost:5177

BASE_URL="http://localhost:5177"
TEST_EMAIL="test@example.com"
TEST_PASSWORD="password123"

echo "🌿 Smart Plant Care API - JWT Authentication Test"
echo "=================================================="
echo ""

# Test 1: Health Check (No Auth Required)
echo "1️⃣ Testing health check endpoint..."
HEALTH_RESPONSE=$(curl -s "$BASE_URL/api/plant/ping")
echo "   Response: $HEALTH_RESPONSE"
echo ""

# Test 2: User Registration
echo "2️⃣ Testing user registration..."
REGISTER_RESPONSE=$(curl -s -X POST "$BASE_URL/api/auth/register" \
  -H "Content-Type: application/json" \
  -d "{
    \"email\": \"$TEST_EMAIL\",
    \"password\": \"$TEST_PASSWORD\",
    \"firstName\": \"John\",
    \"lastName\": \"Doe\"
  }")

echo "   Registration Response: $REGISTER_RESPONSE"
echo ""

# Test 3: User Login
echo "3️⃣ Testing user login..."
LOGIN_RESPONSE=$(curl -s -X POST "$BASE_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{
    \"email\": \"$TEST_EMAIL\",
    \"password\": \"$TEST_PASSWORD\"
  }")

echo "   Login Response: $LOGIN_RESPONSE"
echo ""

# Extract JWT token from login response
JWT_TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

if [ -n "$JWT_TOKEN" ]; then
    echo "✅ JWT Token obtained: ${JWT_TOKEN:0:20}..."
    echo ""

    # Test 4: Get User Profile (Authenticated)
    echo "4️⃣ Testing authenticated profile endpoint..."
    PROFILE_RESPONSE=$(curl -s -X GET "$BASE_URL/api/auth/profile" \
      -H "Authorization: Bearer $JWT_TOKEN")
    
    echo "   Profile Response: $PROFILE_RESPONSE"
    echo ""

    # Test 5: Create Plant (Authenticated)
    echo "5️⃣ Testing plant creation (authenticated)..."
    PLANT_RESPONSE=$(curl -s -X POST "$BASE_URL/api/plant" \
      -H "Authorization: Bearer $JWT_TOKEN" \
      -H "Content-Type: application/json" \
      -d '{
        "name": "Test Plant",
        "species": "Test Species",
        "wateringFrequencyDays": 7,
        "sunlight": "Bright",
        "location": "Test Location",
        "notes": "Test Notes",
        "fertilizingFrequencyDays": 30
      }')
    
    echo "   Plant Creation Response: $PLANT_RESPONSE"
    echo ""

    # Test 6: Get User's Plants (Authenticated)
    echo "6️⃣ Testing get user plants (authenticated)..."
    PLANTS_RESPONSE=$(curl -s -X GET "$BASE_URL/api/plant" \
      -H "Authorization: Bearer $JWT_TOKEN")
    
    echo "   Plants Response: $PLANTS_RESPONSE"
    echo ""

    # Test 7: Test Unauthorized Access
    echo "7️⃣ Testing unauthorized access (should fail)..."
    UNAUTHORIZED_RESPONSE=$(curl -s -X GET "$BASE_URL/api/plant" \
      -H "Authorization: Bearer invalid-token")
    
    echo "   Unauthorized Response: $UNAUTHORIZED_RESPONSE"
    echo ""

else
    echo "❌ Failed to obtain JWT token"
fi

echo "🏁 Testing complete!"
echo ""
echo "📝 Next steps:"
echo "   - Check the responses above for any errors"
echo "   - Verify JWT token is working correctly"
echo "   - Test additional endpoints using the token"
echo "   - Update your frontend to use authentication" 