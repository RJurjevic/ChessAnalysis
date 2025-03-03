# **ChessAnalysis**  

ChessAnalysis is a **lightweight chess analysis tool** that annotates PGN files using **Stockfish 17** and **Vafra Cfish clones** (e.g., Cfish 12.7). It provides **clear, standardized chess glyphs** for evaluating game quality, detecting mistakes, and identifying key turning points.  

## **⚡ Features:**  
- **Engine Support:** Works with **Stockfish 17** and **Vafra Cfish clones**.  
- **Standardized Chess Glyphs:** Clearly marks **inaccuracies, mistakes, blunders, and positional evaluations**.  
- **Customizable Margins:** Set thresholds for blunders, inaccuracies, and evaluation swings.  
- **Configurable Analysis Settings:** Adjust engine depth, and hash size.  
- **Fast and Lightweight:** No dependencies; simple command-line interface for efficiency.  

## **🔧 Configuration:**  
ChessAnalysis uses a `config.txt` file for full customization of engine behavior and analysis parameters.  

### **Configuration Settings**  
Each line in `config.txt` corresponds to a specific setting:  

| **Setting**                 | **Description**                                                                 |
|-----------------------------|-------------------------------------------------------------------------------|
| **Engine Selection**        | Choose between **Stockfish 17** or **Vafra Cfish clones**.                    |
| **Engine Hash (MB)**        | Set memory allocation for engine transposition table.                         |
| **Engine Threads**          | Number of CPU threads used for analysis.                                      |
| **Syzygy Path**             | Path to **Syzygy endgame tablebases** (optional).                             |
| **Move Margins**            | Define how inaccuracies, mistakes, and blunders are classified.               |
| **Good Move (`!`) Margin**    | If a move **improves the position** by this many centipawns, it gets a `!`.   |
| **Excellent (`!!`) Margin**   | If a move **greatly improves the position**, it gets a `!!`.                  |
| **Score Evaluation Margins**| Define when a position is considered **equal, better, or winning**.           |
| **Halfmove Range**          | Analyze only moves within a certain range (useful for filtering long games).  |
| **Move Time (Seconds)**     | Set time per move for evaluation.                                              |

---

### **Example `config.txt`**
```
stockfish-windows-x86-64-avx2.exe
1024
6
C:\Chess\Syzygy
-50    # Dubious move threshold
-100   # Bad move threshold
-200   # Blunder threshold
10     # Good move (!) threshold
30     # Excellent move (!!) threshold
30     # Equal position margin
100    # Edge (small advantage) margin
200    # Better position margin
17     # Halfmove start (first move to analyze)
999    # Halfmove end (last move to analyze)
60     # Time per move in seconds
```

---

### **📝 How Move Glyphs Are Assigned**
ChessAnalysis determines move quality by comparing **before and after evaluations**:
- If a move causes a **loss in evaluation**, it may be classified as:
  - **Dubious (`?!`)**
  - **Mistake (`?`)**
  - **Blunder (`??`)**
- If a move **improves evaluation**, it may be classified as:
  - **Good move (`!`)** if it **increases the evaluation** by at least the `Good Move (!) Margin`.
  - **Excellent move (`!!`)** if it **increases the evaluation** by at least the `Excellent (!!) Margin`.
- If a move is **expected and does not significantly change the position**, it remains unannotated.

To ensure accuracy, ChessAnalysis **re-evaluates** a position if the first pass suggests a good or excellent move. If evaluation keeps **improving**, it re-searches with **double the time**, repeating the process until a stable evaluation is found.

---

### **🏆 Understanding Score Margins**
These thresholds define how the **engine classifies the position**:
- **Equal (`$10`)**: Score difference is **≤30 centipawns**.
- **Edge (`$14/$15`)**: Score difference is between **31 and 100 centipawns**.
- **Better (`$16/$17`)**: Score difference is between **101 and 200 centipawns**.
- **Winning (`$18/$19`)**: Score difference **exceeds 200 centipawns**.

This allows players to **track gradual positional shifts**, rather than only seeing sudden major changes.

---

### **📌 Summary**
- **Fully configurable engine and analysis settings.**  
- **Allows fine-tuning move classification and position evaluation.**  
- **Uses smart move re-evaluation to confirm good (`!`) and excellent (`!!`) moves.**  
- **Customizable time control, ensuring balance between speed and accuracy.**

## **🛠️ Build Instructions (Visual Studio 2022)**  
To build ChessAnalysis from source:  
1. **Clone the repository:**  
   ```sh
   git clone https://github.com/RJurjevic/ChessAnalysis.git
   ```  
2. **Open the solution in Visual Studio 2022:**  
   - Open `ChessAnalysis.sln` in **Visual Studio 2022**.  
3. **Build the solution:**  
   - Select **Release** configuration.  
   - Build the entire **ChessAnalysis** solution.  
4. **Publish the executable:**  
   - Right-click the `ChessAnalysis` project → **Publish** → Select **Publish**.  

## **📦 Pre-Built Executable**  
A **pre-built version** of `ChessAnalysis.exe`, along with compatible **chess engine executables** and **network files**, will be available in the [GitHub Releases](https://github.com/RJurjevic/ChessAnalysis/releases) section as a **Zipped archive** for easy setup.  

## **🚀 Usage:**  
Run ChessAnalysis with a PGN file:  
```sh
ChessAnalysis.exe <game.pgn>
```

## **📜 License & Acknowledgments**  
ChessAnalysis includes portions of code from **Geras1mleo**, licensed under **MIT License**.  
The project itself is licensed under **GNU GPL v3.0**, requiring derivative works to remain open-source.  

## **📫 Contact & Contributions**  
- **Issues & Feature Requests:** Report them on [GitHub Issues](https://github.com/RJurjevic/ChessAnalysis/issues).  
- **Contributions:** Fork the repo, create a branch, and submit a pull request.
