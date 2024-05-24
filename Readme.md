# Requirements
 - .Net 8.0.101+
 - Entity Framework Core 8
 - Node.js v20.11.1
 - npm 6.14.18
 - Angular 17.3.6
 
# How to run the application ?
## Running server
In the project folder, execute the following commands
``` bash
dotnet restore
cd src/Api
dotnet ef database update
dotnet run
```
This will create and seed an sqlite database in the Dal folder
## Running client
In the project folder, execute the following commands
``` bash
cd src/Client
npm install
npm start
```