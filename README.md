# 🎥 TrackRoom - Google Meet Clone

TrackRoom is a real-time video meeting platform built using **ASP.NET Core**, **SignalR**, and **Entity Framework Core**, following a clean **N-Tier architecture**.

It enables users to:
- ✅ Create and join meetings
- ✅ Connect with others in real-time using WebRTC and SignalR
- ✅ Join as a guest or authenticate with email/password
- 🔜 Google Sign-In support
- 🔮 Future integration with an AI-powered attendance system

---

## 📁 Project Structure (N-Tier Architecture)

| Layer | Description |
|-------|-------------|
| `TrackRoom.Api` | The main entry point (Web API & SignalR Hub) |
| `TrackRoom.Services` | Business logic and service contracts |
| `TrackRoom.Infrastructure` | Repository implementations, file handling, etc. |
| `TrackRoom.DataAccess` | EF Core models and DbContext |
| `TrackRoom.Utilities` | Common helpers, constants, and utilities |

---

## 🚀 Features

- 👥 Join/leave meetings in real-time
- 📡 Peer-to-peer WebRTC connection handling via SignalR
- 🔐 JWT Authentication & Authorization
- 🧠 User tracking (joined/left)
- 📌 Organizer permissions (end meeting, kick users)
- ⚙️ Repository Pattern + Clean architecture
- 💬 Ready for chat/video/audio extensions
- 🔗 Frontend-friendly APIs with predictable contracts

---

## 🔐 Authentication

- 📨 Register/Login using email and password
- 🔄 JWT-based authentication with refresh token mechanism
- 🟢 Google Sign-In support coming soon

---

## 📊 Upcoming Features

- 🧠 **AI Attendance Tracking**  
  Integrate an AI-powered module to track participants via webcam for automated attendance monitoring.

- 🖥️ Admin Dashboard  
  For meeting metrics, logs, and member controls.

- 📦 Deployment-Ready with Docker and CI/CD via GitHub Actions

---

## 💡 Tech Stack

- **.NET 8 / .NET 9**
- **SignalR**
- **EF Core**
- **SQL Server / PostgreSQL**
- **JWT Auth**
- **CORS** + frontend integration-ready
- **WebRTC** (on the client-side)

---

## 🛠️ Getting Started

```bash
git clone https://github.com/yourusername/TrackRoom.git
cd TrackRoom

# Add migrations & update DB
cd TrackRoom.Api
dotnet ef database update

# Run the app
dotnet run
```


🧪 Environment Setup
1. Create a appsettings.Development.json file in TrackRoom.Api:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String"
  },
  "Jwt": {
    "Key": "SuperSecretKey",
    "Issuer": "TrackRoom",
    "Audience": "TrackRoomUser"
  },
  "Frontend": {
    "BaseUrl": "http://localhost:3000"
  },
  "GoogleAuth": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

2. Enable Google authentication when ready.

🤝 Contributing
Feel free to fork the repo and open pull requests! If you’re interested in collaborating on the AI Attendance Model, reach out.

📄 License
MIT License - free to use, modify, and distribute.


## ✨ Authors

- **[Abdullah Azmy](https://github.com/abdullahazmy)**  
  *Backend .NET Developer | Linux Administrator*  
  [🌐 Portfolio](https://abdullahazmy.github.io/) • [🔗 LinkedIn](https://www.linkedin.com/in/abdullahazmyelsherbini/)

- **[Abdelrahman Alaa](https://github.com/Abdelrahman984)**  
  *Backend Developer*  
  [🔗 LinkedIn](https://www.linkedin.com/in/abdelrahman-alaa-backend)


