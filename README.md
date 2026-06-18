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

---

## 🎬 Gameplay Preview

https://github.com/user-attachments/assets/7827ad15-78a5-476e-b8bf-1f5141f13e76

---

## 📸 Screenshots

| Game Start | New Room Construction |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/ab435392-9407-4d89-8060-a24657fecc9a" width="500" alt="Game Start"/> | <img src="https://github.com/user-attachments/assets/a10a2ca2-8a88-4589-be97-88329a8760ed" width="500" alt="New Room Construction"/> |

| Room Built | Room Needs Cleaning |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/8545deed-f065-4990-b5e9-c1bc81483b38" width="500" alt="Room Built"/> | <img src="https://github.com/user-attachments/assets/0976b5fd-3be6-45cc-a758-3278453c70fd" width="500" alt="Room Needs Cleaning" /> |

| Construction Menu | Employee Menu |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/8b9f0897-d1e0-4962-b90a-587684112702" width="500" alt="Construction Menu"/> | <img src="https://github.com/user-attachments/assets/46b1bf02-27b7-4f01-919c-f905ce248687" width="500" alt="Employee Menu" /> |

---

## 🏗️ Gameplay Overview

Manage a growing pension by:

* Constructing new rooms and facilities.
* Expanding vertically with additional floors.
* Keeping rooms operational and usable.

Customers arrive throughout the day and request accommodations. Additional facilities improve the attractiveness of the pension and provide extra activities for guests.

---

## 🎯 Core Features

* **Dynamic Building:** Construct new building levels and modular rooms dynamically.

* **Diverse Rooms:** Includes distinct Single/Double Rooms, Bathrooms, Dining Rooms, Arcades, Gyms, and Receptions.

* **Customer AI:** Automate wave spawning, room assignments, facility visits, and departure routines.

* **Staff Management:** Hire and deploy Cleaners, Cooks, Technicians, and Coaches to handle specific maintenance tasks.

* **Progression & Economy:** Track daily cycles, hostel ratings, growing customer waves, and financial earnings.

* **Save/Load:** Serialize game states for saving/loading.

---

## 🧠 Technical Features

* **ScriptableObject Architecture:** Uses a modular event system to pass data and messages between the UI, AI, and game managers without messy dependencies.

* **NavMesh NPC Framework:** Powers both customer routines and worker tasks using a shared AI framework built on Unity NavMesh navigation.

* **JSON Save System:** Handles complete data persistence by serializing floor layouts, player cash, and staff data into structured JSON files.

---

## 🛠️ Built With

* **Game Engine:** Unity (2021.3.18f1)
* **Scripting Language:** C#
* **Architecture:** Scriptable Object Event System
* **AI Navigation:** Unity NavMesh
* **Data Persistence:** JSON-Based Save Data

---

**2023**
