# **ChessAnalysis**  

ChessAnalysis is a **lightweight chess analysis tool** that annotates PGN files using **Stockfish 17** and **Vafra Cfish clones** (e.g., Cfish 12.7). It provides **clear, standardized chess glyphs** for evaluating game quality, detecting mistakes, and identifying key turning points.  

## **⚡ Features:**  
- **Engine Support:** Works with **Stockfish 17** and **Vafra Cfish clones**.  
- **Standardized Chess Glyphs:** Clearly marks **inaccuracies, mistakes, blunders, and positional evaluations**.  
- **Customizable Margins:** Set thresholds for blunders, inaccuracies, and evaluation swings.  
- **Configurable Analysis Settings:** Adjust engine depth, hash size, and move time.  
- **Fast and Lightweight:** No dependencies; simple command-line interface for efficiency.  

## **🔧 Configuration:**  
ChessAnalysis uses a `config.txt` file for customization:  
- **Engine Selection:** Choose between Stockfish 17 and Cfish clones.  
- **Move Time per Position:** Define how long the engine analyzes each move.  
- **Evaluation Margins:** Set thresholds for classifying moves as dubious, bad, or blunders.  

Example `config.txt`:  
```
stockfish-windows-x86-64-avx2.exe
1024
6
C:\Chess\Syzygy
-50
-100
-200
10
30
30
100
200
17
999
60
```

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
