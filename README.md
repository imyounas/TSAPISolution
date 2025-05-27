# TS API Coding Task

## _RESTful Web API using .NET 8_

## Solution Architecture

I generally prefer using Clean Architecture for my API projects. However, since this is a simple and small project, I opted for a folder-based structure to maintain separation of concerns and define dependency layers.

Because there are only a few controllers and endpoints, I chose not to use MediatR (also considering its change in license type, I'm avoiding it in new projects).

For logging, I used **Serilog** with only Console and File sinks. In a commercial project, I would have used Serilog with Seq or Elastic sink, or preferred using **OpenTelemetry** for logging and telemetry data collection.

For input validation, I used **FluentValidation** with simple rule definitions.

To easily manage the status of Mock API calls to my API Controller, I used the Result pattern via the **Ardalis.Result** library. This library provides a simple way to handle success and failure results, making it easier to manage the flow of data and errors in the application.

To make external HTTP calls to the mock API, I used a **typed HTTP client** (`MockExternalAPIService : IExternalAPIService`).

For JSON serialization and deserialization, I chose **Newtonsoft.Json**, as there were some scenarios where it provided better flexibility than the default .NET JSON library.

The mock RESTful API returns JSON responses where the `data` property contains key-value pairs depending on the product type. Here keys are always string, but value types vary depending upon (key)attribute type. Example:{
  "data": {
    "year": 2019,
    "price": 1849.99,
     "isAvailable": true,
    "CPU model": "Intel Core i9",
    "Hard disk size": "1 TB",
 }
}To deserialize this loosely typed data, I used:Dictionary<string, object>? Data { get; init; }
### Project Structure Overview

![Project Structure Overview](./TSWebAPI/Screenshots/TSWebAPISolutionStructure.png "Project Structure Overview")


### API Endpoints/Methods

The `TSProductController` contains the following endpoints:

-   `[GET] /api/TSProduct` – Retrieves all products.
    
-   `[GET] /api/TSProduct/filter` – Retrieves filtered products based on `name`, `pageNo`, and `pageSize`. If the `name` is null or empty, all products are returned.
    
-   `[POST] /api/TSProduct` – Creates a new product.
    
-   `[DELETE] /api/TSProduct` – Deletes a product by the provided `productId`.

![Swagger API Endpoints](./TSWebAPI/Screenshots/Swagger.jpg "Swagger API Endpoints")

### Validation

The **FluentValidation** library is used for all incoming request DTO validations. This is handled in the service layer within the `ProductService` class. The class first checks the validation status of the request DTO; if validation fails, it returns an invalid result. Otherwise, it proceeds to call the external API.

### Error Handling

The `ProductService` class wraps all external API calls in a `try/catch` block. Based on the outcome, it returns the appropriate result to the controller. To send responses to the user, I used the **Result pattern** via the `Ardalis.Result` library.
Besides that there is Global Exception Handler middleware that handles all unhandled exceptions and returns a generic error message to the user. This middleware is registered in the `Program.cs` file.

### Tests

Integration tests are implemented in the `TSIntegration.Test` project using xUnit, FluentAssertions, and Microsoft.AspNetCore.Mvc.Testing's `WebApplicationFactory`. These tests cover the main REST API endpoints of the `TSProductController`:

- `GetAllProducts_ReturnsSuccessAndProducts`: Tests `[GET] /api/TSProduct` to verify retrieval of all products returns a non-empty list and a successful response.
- `GetProductsByFilter_ReturnsFilteredProducts`: Tests `[GET] /api/TSProduct/filter` with a name filter and pagination, ensuring the correct number of filtered products is returned.
- `GetProductsByFilter_WithEmptyName_ReturnsFilteredProducts`: Tests `[GET] /api/TSProduct/filter` with an empty name, verifying pagination and that all products are returned as expected.
- `CreateProduct_ReturnsCreatedProduct`: Tests `[POST] /api/TSProduct` by sending a new product DTO and verifying the created product is returned with the correct name.
- `CreateProduct_WithJsonPayload_ReturnsCreatedProduct`: Tests `[POST] /api/TSProduct` by sending a **JSON payload with mix of different data types, like string, int, decimal, bool etc. in the `data` property, verifying the product is created and returned correctly**.
- `DeleteProduct_ReturnsSuccess`: Tests `[DELETE] /api/TSProduct` by first creating a product, then deleting it by its ID, and verifying the deletion was successful.

The tests use an in-memory test server to simulate real HTTP requests and responses, ensuring the API behaves as expected end-to-end. You can run the integration tests with:
dotnet test TSIntegration.Test/TSIntegration.Test.csproj
This will execute all integration tests and report the results in the console.

## Running the Web API and Integration Tests

### Running the Web API Project

To build and run the Web API project from the command line using the .NET CLI:

1. Open a terminal and navigate to the solution directory.
2. Run the following command to build and start the API:
```
dotnet build TSWebAPI/TSWebAPI.csproj

dotnet run --project TSWebAPI/TSWebAPI.csproj
```

The API will start and listen on the configured ports. You can access the Swagger UI (for API documentation and testing) at:
http://localhost:5050/swagger/index.html

### Running the Integration Test Project

To build and run the integration tests from the command line using the .NET CLI:

1. Open a terminal and navigate to the solution directory.
2. Run the following commands:

```
dotnet build TSIntegration.Test/TSIntegration.Test.csproj

dotnet test TSIntegration.Test/TSIntegration.Test.csproj
```
This will build the test project and execute all integration tests, displaying the results in the console.

---
