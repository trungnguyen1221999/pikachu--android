# 🧩 Pikachu Classic Game

A fun and nostalgic **Pikachu tile-matching game**, built with ❤️ using **Unity** and **C#**, designed for Android devices.

> ⚠️ **For educational purposes only**  
> 🚫 Not for commercial use

---

## 📦 APK Download

📲 [**Download Game (APK)**](https://drive.google.com/file/d/1EwLd4-twvSn1Hx5ay0ZBt1srY4QkzFGX/view?usp=drive_link)  
🕹️ Version: **1.3**  
📱 Platform: **Android**

---
![image](https://imgur.com/FB2NbPQ)
![image](https://imgur.com/PQQN37R)


## 🎮 Game Description

The classic **Pikachu tile-matching puzzle** is back!  
Connect matching Pokémon tiles with up to **3 straight lines** to clear them.  
The game ends when all tiles are cleared — how fast can you go?

---

## 🧠 Algorithm Details

This game implements a **pathfinding algorithm** tailored to the Pikachu gameplay rules:

### 🧩 Matching Rules

- Two tiles can be matched if a path connects them with:
  - **No more than 3 straight lines** (maximum 2 turns)
  - **No blocking tiles** along the path

### 🔍 How It Works

- The board is a 2D **grid of nodes**, each representing a tile or empty space.
- For each selected tile, the game searches for all possible **neighboring paths**:
  - Straight in 4 main directions: **up, right, down, left**
  - Diagonal and corner paths are **not allowed** for direct moves, but used for turning logic
- A **Breadth-First Search (BFS)**-like algorithm is used:
  - It explores all valid paths from the first tile
  - Keeps track of the number of turns (corners) taken
  - Stops searching if more than **2 turns** are needed

### ✅ Optimization

- The algorithm stops early if a valid path is found
- Nodes cache neighbor references for faster lookup
- Matching logic avoids checking paths that clearly break the rules (e.g., blocked paths)

---

## 🛠️ Technologies

- 🎮 **Unity Engine**
- 💻 **C# Scripting**
- 🧱 2D Grid-based logic
- 🔊 Audio system with SFX
- 🧩 Object Pooling for spawning performance
- 📱 Responsive UI that fits most screen sizes (16:9 ratio optimized)

---

## 📘 Legal Notice

- I made this game for **learning and educational** use only.
- All assets (e.g., Pokémon images, sprites) belong to their respective owners.
- This game is **not intended for commercial distribution**.

---
