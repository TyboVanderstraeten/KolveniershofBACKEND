# Kolveniershof: back-end
## Installation
Clone the repository
 ```
 git clone https://github.com/HoGent-Projecten3/projecten3-1920-backend-kolveniershof05
 ```
 In [appsettings.json](KolveniershofBACKEND/appsettings.json) change connectionstring to match your SQL server instance name
 
 Add a user secret
 ```
{
  "Tokens": { "Key": "ThisIsOurUserSecretToBeAbleToGenerateAJWTTokenAndValidateAgainstIt" }
}
 ```
 Change directory into [KolveniershofBACKEND](KolveniershofBACKEND)
 
 Create database through migration
 ```
 dotnet ef database update OR Update-Database
 ```
 Run [offlineScript.sql](offlineScript.sql)
 
 ## Postman tests
 Postman tests can be found [here](Tests.postman_collection.json)
