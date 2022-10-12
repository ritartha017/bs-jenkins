## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [.NET Core SDK 6](https://dotnet.microsoft.com/download/dotnet-core/6.0)
* [Postgresql](https://www.postgresql.org/) (version 13 or later)

### Setup
Follow these steps to get your development environment set up:

1. Clone the repository
2. At the presentation project directory, restore required packages by running:
    ```
   dotnet restore
   ```
3. Next, build the solution by running:
   ```
   dotnet build
   ```
4. Next, within the `src/Endava.BookSharing.Presentation` directory, launch the migrations process by running:
    ```
   dotnet ef database update
   ```
5. Once the database has created, within the `src/Endava.BookSharing.Presentation` directory, launch the back end by running:
   ```
   dotnet run
   ```
6. Launch [URL](http://localhost:5027/swagger/index.html) in your browser to view the Swagger (test api client and documentation)

