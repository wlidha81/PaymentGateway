# PaymentGateway
 This is the implementation of PaymentGateway
 
# Solution design
 This solution was devided into multiple projects
 
 ![image](https://user-images.githubusercontent.com/32079370/128807214-a2052c82-a2b1-471d-af5c-4be773b5c460.png)
 
*Cko.PaymentGateway.csproj
- this is the main api project

*Cko.PaymentGateway.BankSimulator
- this is the project holding the acquiring bank code

*Cko.PaymentGateway.Business.csproj
- project used for defining business classes and utilities

*Cko.PaymentGateway.Business.Tests.csproj
- project used for testing Business.csproj project

*Cko.PaymentGateway.Data.Dtos.csproj
- project used to defines all dto contracts

*Cko.PaymentGateway.Data.Models.csproj
- project used to defines all data model contracts

*Cko.PaymentGateway.Data.Validators.csproj
- project used to defines all data models and dtos validators

*Cko.PaymentGateway.Data.Validators.Tests.csproj
- project used for testing Validators.csproj project

Cko.PaymentGateway.Tests.csproj
- project used for unit testing (controllers as example)

- Cko.PaymentGateway.Integration.Tests
- project used for integration testing (controllers as example)

 
# Solution Type
  Asp.net core web api

# Target Framework
  .netcore 3.1

# Testing Tool
 Xunit
 
#code coverage computed by Fine Code Coverage 
 -![image](https://user-images.githubusercontent.com/32079370/128808085-68084235-c6cb-44a3-9047-0016bf1b678d.png)
 
 -![image](https://user-images.githubusercontent.com/32079370/128808125-061fcf1b-3eac-43dc-83d8-5509d2f5f0e2.png)
 
 
 #Improvements
 -Usage of RetryPolicy (Polly)
 -Implement CacheDecorator pattern for services that retrieve payments details
 -Handle PaymentData Security (hashed data...)
 
 #Cloud Technologies that could be used
 -Disaster Recovery
 -Data Backup
 -Horizontal Scaling
 
 
 

 


 

 
