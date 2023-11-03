# WebApiDemo
A Web Api demonstration that manages datasets from a single table from a database and demonstrates CRUD function.
# Database Setup
Open the solution in visual studio 2022, and note the addition of a database (WebApiDatabase) in the ProjectModels
Then Double Click CreateScripts.sql file in the WebApiDatabase project Database folder, 
and ensure it is opened in sqlcmd mode.

Running the script each time, will result in the table being rebuilt with each run, 
and the default values reset.

# WebApi
Ensure the correct WebApiDemo.Api project is set as startup, and then click run(https or http if working with the front end).
The swagger interface should apepar momentarily.
THe Api is split between three layers, the data access layer, context, and configuration; The service layer principally associated with Mediatr,
Domain Objects and Mapping from the Database layer; and the Api Service Itself, focused on controllers and input validation.

The Api demonstrates SOLID principles, specifically Single Responsability, Open/Closed structures, Liskov substitution, Interface Segregation, and Dependency inversion.
The api is built around database access following Dependency Injection, Mediatr, EF Core, Serilog, OpenUI, and FluentValidation libraries to faciliate some design patterns.

# Tests
Unit tests are performed for both integration testing (although this will not test the database itself, which would require a separate set of tests instead of in-memory EF core), and unit testing of specific classes.
The unit testing is not exhaustive, and the context is abstracted behind an interface to facilitate unit testing more effectively.  
xUnit was used, along with FluentAssertion, Bogus, and Moq.

# Intent
This web api, is standalone, and can work independently of the front end user experience, using swagger, or postman, 
but it was designed to interact with a front end repository at FrontEndDemo. the front end uses http instead of https, 
so this api should be run in http if it is to run with the front end.  It is intended to demonstrate basic CRUD operations on a database, but with a soft delete, rather than a hard delete.
