# Postman API Tests

## Collection Setup
1. Import the collection:
   - OrderManagementSystem.postman_collection.json
2. Import environment :
   - OrderManagementSystem.postman_environment.json

## Running Tests
1. Set your base URL in environment variables (its already there but change if needed)
2. Run individual requests
3. View test results in Postman's "Test Results" tab

## Test Coverage
- Authentication token requesting login api.
- Update Order status there two requests one represent invalid update and another one for successful upddate. 
- Analytics endpoint and observe response time if its reduced when it hits the cache.