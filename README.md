# **ChessAnalysis**

ChessAnalysis is a **lightweight chess analysis tool** that annotates PGN files using **Stockfish 18** and **Vafra Cfish clones** (e.g., Vafra Cfish 15.0). It provides **clear, standardized chess glyphs** for evaluating move quality, detecting mistakes, and identifying key turning points in a game.

---

## **⚡ Features:**

- **Engine Support:** Works with **Stockfish 18** and **Vafra Cfish clones**.
- **Standardized Chess Glyphs:** Clearly marks **inaccuracies, mistakes, blunders, and positional evaluations** using standard PGN Numeric Annotation Glyphs (NAGs).
- **Crushing Advantage Detection:** Supports `$20` and `$21` for resignation-level advantages.
- **Forced-Mate Blunder Override:** Starting with `v1.0.4.0`, marks a move as `$4` if it changes the engine assessment from non-mate to forced mate for the opponent.
- **Configurable Margins:** Fine-tune thresholds for blunders, inaccuracies, good moves, excellent moves, and evaluation bands.
- **Flexible Analysis Settings:** Adjust engine hash size, CPU threads, Syzygy tablebase path, halfmove range, and time per move.
- **Comment Handling in Config:** Supports `#` comments in `config.txt` for better customization.
- **Fast and Lightweight:** Simple command-line interface for efficient PGN analysis.

---

## **🔧 Configuration**

ChessAnalysis uses a `config.txt` file for full customization of engine behavior and analysis parameters. Comments (`#`) can be added for clarity and are ignored when reading values.

### **Configuration Settings**

Each non-empty, non-comment line in `config.txt` corresponds to a specific setting:

| **Setting** | **Description** |
| --- | --- |
| **Engine Selection** | Choose a UCI chess engine executable, such as **Stockfish 18** or **Vafra Cfish 15.0**. |
| **Engine Hash (MB)** | Set memory allocation for the engine transposition table. |
| **Engine Threads** | Number of CPU threads used for analysis. |
| **Syzygy Path** | Path to **Syzygy endgame tablebases** (optional). |
| **Move Margins** | Define thresholds for dubious moves (`?!`), mistakes (`?`), and blunders (`??`). |
| **Good Move (`!`) Margin** | If a move improves the position by at least this many centipawns, it may receive `$1` / `!`. |
| **Excellent Move (`!!`) Margin** | If a move greatly improves the position, it may receive `$3` / `!!`. |
| **Score Evaluation Margins** | Define when a position is considered equal, slight advantage, moderate advantage, decisive advantage, or crushing advantage. |
| **Crushing Advantage Margin** | Score threshold for `$20` / `$21`, indicating one side has a crushing advantage. |
| **Halfmove Range** | Analyze only moves within a certain range, useful for filtering long games. |
| **Move Time (Seconds)** | Set time per move for engine evaluation. |

---

### **Example `config.txt`**

```txt
stockfish-windows-x86-64-avx2.exe  # Chess engine executable
1024                               # Engine hash size (MB)
6                                  # Number of CPU threads for engine calculations
C:\Chess\Syzygy                    # Path to Syzygy endgame tablebases (optional)
-50                                # Dubious move (?!) threshold (centipawns)
-100                               # Bad move (?) threshold (centipawns)
-200                               # Blunder (??) threshold (centipawns)
10                                 # Good move (!) threshold (centipawns)
30                                 # Excellent move (!!) threshold (centipawns)
30                                 # Equal position margin (=) (centipawns, considered an even game)
100                                # Edge (+/= or =/+) (small advantage) margin (centipawns)
200                                # Better (+= or =+) (moderate advantage) margin (centipawns)
800                                # Crushing advantage margin (centipawns, produces $20 / $21)
17                                 # Halfmove start (first move to analyze)
999                                # Halfmove end (last move to analyze)
60                                 # Time per move in seconds (engine evaluation time per move)
```

> **Note:** Starting with `v1.0.3.0`, the config file uses **16 settings**. The crushing advantage margin line is required.

---

## **📝 How Move Glyphs Are Assigned**

ChessAnalysis determines move quality by comparing **before and after evaluations**:

- If a move causes a **loss in evaluation**, it may be classified as:
  - **Dubious (`?!`)** / `$6`
  - **Mistake (`?`)** / `$2`
  - **Blunder (`??`)** / `$4`
- If a move **improves evaluation**, it may be classified as:
  - **Good move (`!`)** / `$1` if it increases the evaluation by at least the `Good Move (!) Margin`.
  - **Excellent move (`!!`)** / `$3` if it increases the evaluation by at least the `Excellent (!!) Margin`.
- If a move is expected and does not significantly change the position, it remains unannotated.

To improve accuracy, ChessAnalysis **re-evaluates** a position if the first pass suggests a good or excellent move. If evaluation keeps improving, it re-searches with **double the time**, repeating the process until a stable evaluation is found.

### **Forced-Mate Blunder Override**

Starting with `v1.0.4.0`, ChessAnalysis includes a narrow tactical override for forced mates. If the engine's best line before the played move is **not** a forced mate, but after the played move the opponent has a forced mate, the played move is marked as a blunder (`$4`).

This catches important practical mistakes in already winning or lost positions, where the normal centipawn/evaluation-band logic may not add another move glyph because the position is already marked as decisive or crushing.

Example:

```pgn
10... Rb8 $4 $18 11. Ng5 d5 $4 $20 12. Qh5 Be6 $4 13. Qxh7#
```

Here, Black was already in serious trouble, but `12... Be6` is still marked as `$4` because it allows a forced mate.

---

## **🏆 Understanding Score Margins**

These thresholds define how the **engine classifies the position**:

| **Score Band** | **NAG** | **Meaning** |
| --- | --- | --- |
| Equal | `$10` | Drawish position or even game. |
| Edge | `$14` / `$15` | White / Black has a slight advantage. |
| Better | `$16` / `$17` | White / Black has a moderate advantage. |
| Decisive | `$18` / `$19` | White / Black has a decisive advantage. |
| Crushing | `$20` / `$21` | White / Black has a crushing advantage. |

Using the default example config:

- **Equal (`=`)**: score difference is **≤30 centipawns**.
- **Edge (`+/=` or `=/+`)**: score difference is between **31 and 100 centipawns**.
- **Better (`+=` or `=+`)**: score difference is between **101 and 200 centipawns**.
- **Decisive (`+-` or `-+`)**: score difference is between **201 and 800 centipawns**.
- **Crushing (`$20` or `$21`)**: score difference **exceeds 800 centipawns**.

This allows players to **track gradual positional shifts**, rather than only seeing sudden major mistakes.

---

## **♟️ Supported PGN Numeric Annotation Glyphs**

ChessAnalysis can emit terse PGN NAGs which are recognized by many chess GUIs, including Fritz and ChessBase-style interfaces.

| **NAG** | **Meaning** |
| --- | --- |
| `$1` | Good move (`!`) |
| `$2` | Poor move or mistake (`?`) |
| `$3` | Very good or excellent move (`!!`) |
| `$4` | Very poor move or blunder (`??`) |
| `$5` | Speculative or interesting move (`!?`) |
| `$6` | Questionable or dubious move (`?!`) |
| `$10` | Drawish position or even game |
| `$14` | White has a slight advantage |
| `$15` | Black has a slight advantage |
| `$16` | White has a moderate advantage |
| `$17` | Black has a moderate advantage |
| `$18` | White has a decisive advantage |
| `$19` | Black has a decisive advantage |
| `$20` | White has a crushing advantage |
| `$21` | Black has a crushing advantage |

Example:

```pgn
17... Nf6 $4 $20
```

This means Black made a blunder (`$4`) and White now has a crushing advantage (`$20`).

Another example from the forced-mate override:

```pgn
12... Be6 $4
```

This means the move allowed a forced mate for the opponent, even if the position was already evaluated as crushing.

---

## **🚀 Usage**

Run ChessAnalysis with a PGN file:

```sh
ChessAnalysis.exe <game.pgn>
```

ChessAnalysis writes the analysed game to:

```txt
<game>_annotated.pgn
```

If an output file with the same name already exists, it is overwritten.

---

## **📌 Summary**

- **Fully configurable engine and analysis settings.**
- **Allows fine-tuning move classification and position evaluation.**
- **Supports standard PGN/NAG glyphs for move quality and positional assessment.**
- **Adds `$20` / `$21` crushing advantage detection in `v1.0.3.0`.**
- **Adds forced-mate blunder override in `v1.0.4.0`.**
- **Uses smart move re-evaluation to confirm good (`!`) and excellent (`!!`) moves.**
- **Customizable time control, ensuring balance between speed and accuracy.**

---

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
   - Right-click the ChessAnalysis project → **Publish** → Select **Publish**.

---

## **📦 Pre-Built Executable**

A **pre-built version** of `ChessAnalysis.exe`, along with compatible **chess engine executables** and **network files**, is available in the [GitHub Releases](https://github.com/RJurjevic/ChessAnalysis/releases) section as a **zipped archive** for easy setup.

---

## **📜 License & Acknowledgments**

ChessAnalysis includes portions of code from **Geras1mleo**, licensed under the **MIT License**.

The project itself is licensed under **GNU GPL v3.0**, requiring derivative works to remain open-source.

---

## **📫 Contact & Contributions**

- **Issues & Feature Requests:** Report them on [GitHub Issues](https://github.com/RJurjevic/ChessAnalysis/issues).
- **Contributions:** Fork the repo, create a branch, and submit a pull request.
