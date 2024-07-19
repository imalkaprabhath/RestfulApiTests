# RestfulApiTests

This project contains automated tests for the [restful-api.dev](https://restful-api.dev/) API.

## Prerequisites

- .NET SDK
- Visual Studio Code

## Setting Up the Project

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd RestfulApiTests
   ```


2. **Restore the packages**:
   ```bash
   dotnet restore
   ```


## Running the Tests using Visual Studio Code

- Open the project in Visual Studio Code.
- Open a terminal in Visual Studio Code.
- Run dotnet test to execute the tests.


## Test Scenarios

- Get list of all objects
- Add an object using POST
- Get a single object using the added ID
- Update the object added using PUT
- Delete the object using DELETE

Each test validates the success of the corresponding API operation using relevant assertions.
