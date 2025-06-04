# Flash Cards App
This repository is an interactive web application for studying educational material using flashcards, utilizing spaced repetition algorithms and the ability to generate cards based on a text document using AI.

## 🗝️ Key features
- User registration and authorization
- Creation of custom training units
- Generation of flashcards from a text file using AI
- Spaced repetition based on the Confidence-Based Repetition and SM-2 algorithms
- Card management: creating, editing, deleting

## 🛠️ Project technologies
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Frontend**: Blazor WebAssembly + MudBlazor
- **Database**: MS SQL Server
- **AI-service**: ChatGPT-4.1.-nano
- **DevOps**: GitHub, Azure VM, IIS

##  📂  Project structure
 ✨ Clean architecture ✨
```
root
├── sample-data                             <- JSON files for tests
├── src 
│   ├── FlashCards.Api                      <- API project 
│   ├── FlashCards.Core                     <- Core logic
│   ├── FlashCards.Infrastructure           <- Services and data layer
│   ├── FlashCards.WebUI                    <- Blazor app
│   ├── FlashCards.sln 
├── .gitignore 	
└── README.md                               <- you are here
```

## How to deploy
Use the folowing steps to deploy the app on your server: 
1. Install **MS SQL Server** using this link> https://www.microsoft.com/en-us/sql-server/sql-server-downloads 
2. Install **.NET 8 runtime**> https://dotnet.microsoft.com/en-us/download/dotnet/8.0 (choose **Hosting Bundle** installer for Windows)
3. Install **URL rewrite**> https://www.iis.net/downloads/microsoft/url-rewrite (for correct work of Blazor static files) 
4. Clone this repository 
```
git clone https://github.com/ihorrohatiuk/flash-cards.git
```
5. Publish projects

For **FlashCards.Api**: 
```
dotnet publish FlahCards.Api.csproj -c Release -o ./publish
```
and for **FlashCards.WebUI**:
```
dotnet publish FlahCards.WebUI.csproj -c Release -o ./publish
```
7. Create project database
```
CREATE DATABASE FlashCardsDB;
GO
``` 
8. Check FlashCards.Api **appsettings.json** files

Don't forget to put your OpenAI api key & JWT secure key
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtOptions": {
    "Issuer": "Issuer",
    "Audience": "Audience",
    "Key": "<add here your JWT secrets>",
    "ExpireHours": 6
  },
  "AiServiceOptions": {
    "ApiKey": "<add here your OpenAI API key>"
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost,1434;Database=FlashCardsDB;User ID=sa;Password=12345678;TrustServerCertificate=true"
  }
}
```
also check your connection string for **FlashCardsDb**

9. Add your server API in **appsettings.json** file for FlashCards.WebUI
For example
```
{
  "ServerUrl": "http://10.0.0.1:5000"
}
```

9. Add published apps to IIS Web Server:  Sites -> Add Website...