# TerraShop – Your Eco-Friendly Marketplace

<img width="1901" height="905" alt="Demo Image" src="https://raw.githubusercontent.com/Constantin-Stamate/TerraShop/main/eUseControl.Web/Assets/img/demo.png" />

## Overview

TerraShop is an eco-friendly online store that offers sustainable and high-quality products. Users can browse, add products to their cart, and complete purchases in a seamless and secure environment. The platform emphasizes environmental responsibility and provides a smooth shopping experience. Enjoy fast checkout, personalized recommendations, and reliable customer support.

## Features

- **Shop Pages**: Browse products with detailed information, images, top products, recommended items, and user reviews.  
- **Search & Filters**: Quickly find products with search and filter options for a smooth shopping experience.  
- **Cart & Wishlist**: Add products to your cart or wishlist for easy management and future purchases.  
- **Checkout & Payment**: Seamless checkout process with multiple payment options.  
- **User Profile Management**: Manage your account, personal details, and order history.  
- **Admin Management**: Full administrative control over products, users, categories, orders, coupons, requests, and reviews.  
- **TerraAI Assistant**: AI-powered assistant for personalized product recommendations and support.  
- **Authentication & Authorization**: Secure login and registration for all users.  
- **Product Reviews**: Users can leave reviews for products, view others' reviews, and interact with feedback.  
- **Information Pages**: About, Terms of Use, Privacy Policy, and Refunds pages for transparency.  
- **Error Handling**: Custom 404 Not Found page to guide users when a resource is missing or unavailable.  
- **Action Confirmations**: Confirm actions such as order creation, request submissions, or other sensitive operations.
- **Unit Testing**: Comprehensive unit tests to ensure the correctness of business logic, services, and critical functionalities throughout the application.

## Technologies

- **Framework & Architecture**: ASP.NET Core MVC, Razor
- **AI Integration**: Ollama 3.2
- **Database**: SQL Server, Entity Framework Core
- **Testing**: Unit tests
- **Development Tools**: Visual Studio 2022
- **Version Control & Repository**: Git, GitHub

## Resources

- [ASP.NET Core Documentation (Microsoft)](https://learn.microsoft.com/en-us/aspnet/core)  
- [Entity Framework Core Documentation (Microsoft)](https://learn.microsoft.com/en-us/ef/core)  
- [MSTest Documentation (Microsoft)](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)  
- [SQL Server Documentation (Microsoft)](https://learn.microsoft.com/en-us/sql/sql-server)  
- [Visual Studio Documentation (Microsoft)](https://learn.microsoft.com/en-us/visualstudio/)  

## Installation
To install the application, follow these steps:

1. **Clone this repository:**
```bash
   git clone https://github.com/Constantin-Stamate/TerraShop
```

2. **Navigate to the project directory:**
```bash
   cd TerraShop
```

3. **Restore dependencies:**
```bash
   dotnet restore
```

4. **Configure the database connection:**
- Open the `Web.config` file in the project and replace the connection string with your own database details:
```xml
<connectionStrings>
    <add name="YOUR_DATABASE_NAME" connectionString="Data Source=YOUR_SERVER_NAME; Initial Catalog=YOUR_DATABASE_NAME; Integrated Security=True; MultipleActiveResultSets=True; App=EntityFramework; TrustServerCertificate=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```
- In the `DBModel` folder inside `BusinessLogic`, update the `BaseName` property to match your database name.

5. **Run the application:**
```bash
   dotnet run --project eUseControl.Web
```

6. **Running tests:**
- Create a separate test database.
- Configure the test database connection in both Web.config (for the main application) and App.config inside the eUseControl.Tests project.
- Populate the test database with example data.

7. **Run tests:**
```bash
   dotnet test eUseControl.Tests
```

## Contributors

**TerraShop** was developed as part of the **Web Technologies course** at **Technical University of Moldova (UTM)**. This project welcomes contributions from developers interested in improving functionality, adding features, or fixing bugs.  

- GitHub: [Constantin-Stamate](https://github.com/Constantin-Stamate)
- Email: constantinstamate.r@gmail.com