# 🌐 NetSentinel - Real-Time Uptime & Health Monitor

NetSentinel is a lightweight, real-time website and API health monitoring dashboard built with **ASP.NET Core MVC**. It continuously tracks the availability and response times of registered endpoints using an asynchronous background worker service.

## 🚀 Features

* **Real-Time Monitoring:** Continuously pings registered URLs in the background.
* **Background Worker Service:** Utilizes a hosted `BackgroundService` that runs independently from the web interface.
* **Smart Health Tracking:** Logs HTTP status codes, response times (in milliseconds), and tracks Up/Down states.
* **Modern Dashboard:** A responsive, dark-themed UI built with Bootstrap to display real-time statuses.
* **Exception Handling:** Gracefully handles DNS failures, timeouts, and invalid URLs without crashing the system.

## 🛠️ Tech Stack & Architecture

* **Framework:** .NET 8.0 / ASP.NET Core MVC
* **Database:** SQL Server
* **ORM:** Entity Framework Core (Code-First Approach)
* **Frontend:** HTML5, CSS3, Bootstrap 5

### 💡 Engineering Best Practices Implemented:
* **`IHttpClientFactory`:** Used to instantiate `HttpClient` instances within the background service to prevent socket exhaustion and manage connection pooling efficiently.
* **`IServiceScopeFactory`:** Implemented to safely resolve the scoped `DbContext` within the singleton Background Service, preventing memory leaks and concurrency issues.
* **Dependency Injection (DI):** Decoupled architecture for clean and maintainable code.

## ⚙️ Getting Started

### Prerequisites
* .NET SDK 6.0 or later
* SQL Server (LocalDB or SSMS)
* Visual Studio 2022 / VS Code

### Installation

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/Osama534-png/NetSentinel.git](https://github.com/Osama534-png/NetSentinel.git)
   cd NetSentinel
   ```

2. **Update the Connection String:**
   Open `appsettings.json` and verify the `DefaultConnection` matches your SQL Server instance.

3. **Apply Database Migrations:**
   Open the Package Manager Console (PMC) and run:
   ```powershell
   Update-Database
   ```
   *Alternatively, using .NET CLI:*
   ```bash
   dotnet ef database update
   ```

4. **Run the Application:**
   Press `F5` in Visual Studio or run `dotnet run` in the terminal. The background ping engine will start automatically.

## 📸 Screenshot
