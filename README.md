# 🏨 Pension Tycoon

[![Unity Version](https://img.shields.io/badge/Unity-2021.3.18f1-blue.svg?style=flat&logo=unity)](https://unity.com/)
[![Language](https://img.shields.io/badge/Language-C%23-green.svg?style=flat\&logo=c-sharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Genre](https://img.shields.io/badge/Genre-Management%20Simulation-orange.svg?style=flat)]()

A hostel management simulation where players expand and operate a growing pension business by constructing rooms, hiring employees, serving customers, and improving overall facility performance.

---

## 🚀 Quick Start

Want to skip the code and just play?

1. **[Download the Repository](https://github.com/Frext/PensionTycoon/archive/refs/heads/main.zip)**
2. Extract the ZIP file.
3. Open the **`BuildWindows`** folder and run the game executable named **`PensionTycoon.exe`**.

---

## 🎮 Controls

|         Interaction        | Action                               |
| :------------------------: | :----------------------------------- |
|       **Left Click**       | Interact with rooms and UI elements. |
|       **Drag**             | Pan across the pension.              |
|       **Mouse Wheel**      | Zoom in and out.                     |
|       **Build Menu**       | Construct rooms, expand floors, and hire employees.   |

---

## 🏗️ Gameplay Overview

Manage a growing pension by:

* Constructing new rooms and facilities.
* Expanding vertically with additional floors.
* Keeping rooms operational and usable.
* Increasing pension value and customer satisfaction.

Customers arrive throughout the day and request accommodations. Additional facilities improve the attractiveness of the pension and provide extra activities for guests.

---

## 🎯 Core Features

### 🏢 Dynamic Floor Construction

* Expand the building by adding new floors.
* Maintain room slot data between sessions.
* Automatic roof and floor positioning.

### 🚪 Multiple Room Types

Available room categories include:

* Single Room
* Double Room
* Bathroom
* Dining Room
* Arcade
* Gym
* Reception

Each room type serves a unique gameplay purpose.

### 👥 Customer AI System

* Customers arrive in waves.
* Room assignment system.
* Optional facility visits (Arcade, Gym, Dining Room).
* Dynamic room availability checks.
* Automatic departure and room release.

### 👨‍🔧 Employee Management

Hire and assign specialized staff:

| Employee        | Responsibility       |
| --------------- | -------------------- |
| Cleaner         | Bedrooms & Bathrooms |
| Cook            | Dining Rooms         |
| Game Technician | Arcades              |
| Gym Coach       | Gyms                 |

Employees travel to assigned rooms, complete maintenance tasks, and become available again after a cooldown period.

### 💾 Save & Load System

Persistent data includes:

* Constructed rooms
* Floor layouts
* Currency
* Employee assignments
* Progression data

### 📈 Progression Systems

* Day progression
* Pension rating
* Economic growth
* Room expansion
* Employee scaling
* Customer wave scaling

### 🎥 Camera System

* Smooth movement
* Zoom controls
* Boundary restrictions
* Automatic repositioning during building placement

---

## 🧠 Technical Features

### Scriptable Object Architecture

The project heavily utilizes Scriptable Objects for:

* Global variables
* Event communication
* Employee data
* Room types
* Shop item information
* Configuration ranges

### Event-Driven Design

Custom ScriptableObject event channels provide decoupled communication between systems including:

* Room events
* Employee assignment events
* Customer events
* UI updates
* Building management events

### NPC Framework

Shared NPC base functionality supports:

* Navigation
* Waypoint movement
* Task execution
* Room interactions
* AI state management

---

## 📂 Main Systems

| System           | Purpose                                |
| ---------------- | -------------------------------------- |
| FloorManager     | Building generation and room placement |
| Room             | Room state and occupancy management    |
| NPCManager       | Customer and employee spawning         |
| Customer         | Guest AI behaviour                     |
| Employee         | Maintenance worker AI                  |
| CameraController | Camera movement and zoom               |
| DataManager      | Save/load operations                   |
| Shop System      | Building and upgrade purchases         |

---

## 🛠️ Built With

* **Game Engine:** Unity (2021.3.18f1)
* **Scripting Language:** C#
* **Architecture:** Scriptable Object Event System
* **AI Navigation:** Unity NavMesh
* **Persistence:** JSON-based save data

---

**2023**
