# OrderManagementSystem

#Logical Discount Rules that has been set based on customer segement/type and their purchases over specific months. 
| CustomerType | Base Discount  | Discount Eligibility Months |
|--------------|----------------:|:-----------------------------|
| New          |      10% (0.90M) | 5% at 50 orders, 10% at 100 orders (over 3 months only) |
| Regular      |       5% (0.95M) | 5% at 100 orders, 10% at 300 orders, 15% at 500 orders (over 6 months only) |
| Loyal        |      15% (0.85M) | 5% at 50 orders, 10% at 150 orders, 15% at 300 orders, 20% at 500 orders (over 12 months only) |
| VIP          |      20% (0.80M) | 5% at 25 orders, 10% at 75 orders, 15% at 150 orders, 20% at 300 orders, 25% at 500 orders (over 12 months only) 

Architecture:
- Organizing two solution folders has been created one for source code[src] and one for unit tests [test].

starting with src folder started creating the solution following clean architecture, 4 layers has been created :
- core layer to have the domain models.
- infrastructure to have the data logic, dbcontext has been added to infrastructure.
- application layer to have dtos if needed, business logic represented in interfaces and services.
- API layer : has all confiuration for registering services, controllers, handling exceptions


**Core Layer:**
for simulation discounting functionality classes have been introduce and they are Order, OrderItem, Customer, CustomerType to represent segments that customers categorized based on and OrderStatus 
- demonstration, CustomerTypes are New, Loyal, Regular, VIP 
- demonstration OrderStatus are Created, Pending, Processing.....etc

**Infrastructure Layer:**
- ef core in-momery database is introduced 
- dbcontext is created and dbsets added
- seed data for demonstration is added
- AsNoTracking is used to have for better performace.
- async operations are used 

**Application Layer:**
- has all business logic for discouting implementation, Startegy design pattern is used to implement different startegies for discount  based on customer types and order history along with Strategy Context
- introduced discountRules as mentioned in the above table showing and that for demonstration, it can have different business rules
- IOrderService/OrderService are implemented to calculate discount as it determine which startegy to use and pass it to the context to apply discount, also added feature update status of order with simple validation if status can be applied or custom exception is raised if transition is invalid 
- feature getting order analytics is introduced, getting order amount average, total orders count, fullfillment average time.
- added caching feature.

**API layer:**
- Organized services injections as extension methods to have a cleaned programs.cs
- RateLimiter is introduced to prevent abuse (to have better performance).
- Created GlobalExceptionHandler with the custom exception present.
- Added Jwt Brearer Token to authenticate use and authorize access of order controller.
- For documentation allow xml generaion and configure swagger to use it, added summeries over classes.


**Test Project:**
- MSTest project has been added
- Test scenarios have been added to test discount applied to differnt customer type each type has a test class

  **AppSettings:**
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWTsettings": {
    "SecretKey": "994cd75f2838969393264595f951ad78dfb06c2ce3cc3c9ef63da18c4cb55e31346ec331e146fcb8ceaef5b5ce371b05945ee58f8543b43036bddc621107088de60bca62f7b6eea76e78ae468f15c41664f89007994404e47838f420ba1c3f7bc87f2a96e8a4f4ecc7393769487b5d7589f5f59892d3eca65803d9b4605fc10ab229a6f8b647db158cc84bfc663e7079133935f9d3996f5d9c73aa1c18047760e5d8ad7e8b945d6001eb14b4ec02f5e7dc4720e0a58254448fc2292e3891deda814168efb6b433e20d5fce0589e78dbe7cd0f969f562173893b0fdb237cedca5ca6cff6d472ca97fd44f356331bf92d5522d3adbadd688e267aa079e9e8732a7",
    "Issuer": "https://localhost:7065",
    "Audience": "postman"
  },
  "Credentials": { 
    "Password": "admin", // for demo
    "Username": "admin" // for demo
  }
}

**Performance Considerations**
- Rate limiting is implemented.
- AsNoTracking() is used for analytics query.
- In-memory DB for development.
- Caching is implemented.
