#   **ImMicro(UnderDevelopment)**
    
This is a simple/reusable microservice template/playgorund project.  

#### Features
- Will be added

#### Structure
- **API:** Endpoint project for client usage  
- **Business:** Project for business logic        
- **Common:** Cross cutting consern items (like cache, lock...)   
- **Container:** DI configuration project   
- **Contract:** Dtos, layer transfer and api response - request objects   
- **Data:** Database layer files (EfCore implementations and repositories)    
- **Model:** Database entity models   
![alt tag](Files/solutiondiagram.jpg)  

#### Technologies

* .Net 5.0 - C# 9.0
* Asp .Net Health Check  
* EF Core 5.0  
* Postgres Sql  
* Redis  
* Serilog  
* NWebSec  
* Autofac  
* FluentValidation  
* Swagger  
* RedLock  
* AutoMapper  
* Docker

#### Before Usage
* If you want to use local environment, you need to update Redis and PostgreSql connection strings in API project  (appsettings.json)    
    * **DBConnectionString** field for PostgreSql
    * **RedisConnectionString** field for DistributedCache
    * **Distributed Lock Settings**
        * RedLockHostAddress
        * RedLockHostPort
        * RedLockHostPassword > if you dont have pass you need to set it null
        * RedLockHostSsl
* Also project has ready to run **Docker support**
    * **docker-compose file store in solution directory**
    * All connection string stores in appsettings.DockerCompose.json environment file

#### Usages
* will be added

#### Swagger Endpoint   
* http://localhost:5000/swagger/index.html

#### Healt Check Endpoints   
* http://localhost:5000/health-check    > json response endpoint for health information
* http://localhost:5000/health-check-ui > Dashboard for see application services health (Redis, NpgSql)    

#### Files Folder
* will be added

#### Release Notes
* will be added

#### Code Coverage
* will be added

#### SonarQube Test Results
* will be added
