# GetCms

GetCms is light weight set of .NET Core libraries designed to add Content Management functionality to APS.NET Core project. 

## Why GetCms

It aims to make development easier while reducing developemnt time.

### Features

* Multilanguage
* Easy customization
* Markup is separated from the engine

## Getting Started

There is WebApp [sample](https://github.com/alexlvovich/getcms/tree/master/samples) project which can be used for demo/learning purposes.

### Prerequisites

GetCms (ASP.NET Core MVC) version only works in Visual Studio 2017 or higher.

Please install .NET Core SDK 2.2 for your Visual Studio from:

[https://www.microsoft.com/net/download/core](https://www.microsoft.com/net/download/core)

GetCms works with Microsoft SQL Server, so you will need a working database.

### Installing

Run DB schema creation script: 

```cmd

sqlcmd.exe -S DBserverName -U username -P p@ssword -i <path>getcms_db.sql -o "c:\output.txt"
```

Update appsettings.json file with proper connection string and run WebApp project.

### Seed data

Demo site data is included however generating should be enabled in appsettings.json:

```json
"SeedData": true
```

## Contributing

You may contribute to the project by opening a pull request, adding documentation, sample source code, asking a question or suggesting an improvement etc. Just open an issue here with relevant links.

### Give a Star! `:star:`

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

### Roadmap

There is a lot of work ahead! `:smiley:`

* Nuget packages
* Additional data providers
  * EF
  * MongoDB
  * MySql
  * PostgreSQL
  * etc
* Management app

## Authors

* **Alexander Lvovich** - *Initial work* - [alexlvovich](https://github.com/alexlvovich)

## License

GetCms is a free and open source project with MIT licence, which permits usage in commercial applications.

## Acknowledgments

* Built on ASP.NET Web project
* Readme.md file inspired by [https://gist.github.com/PurpleBooth/109311bb0361f32d87a2](https://gist.github.com/PurpleBooth/109311bb0361f32d87a2)
