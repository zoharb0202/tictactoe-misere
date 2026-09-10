# Tic-Tac-Toe Misère

[![build](https://github.com/zoharb0202/tictactoe-misere/actions/workflows/build.yml/badge.svg)](https://github.com/zoharb0202/tictactoe-misere/actions/workflows/build.yml)

A Windows desktop game of **reverse tic-tac-toe**: whoever completes a full row, column or diagonal **loses**.
Play on any board from 4×4 to 10×10, against a friend or against a computer opponent that looks ahead to avoid traps and set them.

## Features

- **N×N board** (4–10), chosen on a settings screen before the game starts
- **Two modes**: player vs player, or player vs computer
- **Multi-round scoring**: scores carry over between rounds until you quit
- **Computer opponent** with a one-move lookahead:
  1. never completes its own line when any other move exists
  2. prefers moves that leave the opponent *only* losing moves
  3. otherwise picks the move that leaves the opponent the fewest safe replies (random tie-break)

## Architecture

```
TicTacToeMisere.sln
├── src/
│   ├── TicTacToeMisere.GameLogic    # .NET Standard 2.0 — no UI dependencies
│   │   ├── Board.cs                 # N×N grid, O(N) line check from the last move, CellChanged event
│   │   ├── GameManager.cs           # turns, move validation, win/tie detection, scores
│   │   ├── ComputerAI.cs            # lookahead heuristic
│   │   └── Player.cs, eEnums.cs
│   └── TicTacToeMisere.WinFormsUI   # .NET Framework 4.8 WinForms front-end
│       ├── GameSettingsForm.cs      # names, opponent type, board size
│       └── GameForm.cs              # dynamic button grid, score bar, round dialogs
└── tests/
    └── TicTacToeMisere.Tests        # xUnit tests for the board, game rules and AI
```

The logic layer knows nothing about the UI. The form subscribes to the board's `CellChanged` event,
so every change — human move, computer move or board reset — is drawn through the same path.
Because the logic is a separate library, the same engine could drive a console, WPF or web front-end without changes.

## Run it

Requirements: Windows, .NET SDK 8+ (builds the .NET Framework 4.8 app).

```bash
dotnet build TicTacToeMisere.sln -c Release
dotnet test tests/TicTacToeMisere.Tests
```

The game is at `src/TicTacToeMisere.WinFormsUI/bin/Release/net48/TicTacToeMisere.exe`.
Every push also builds the game on GitHub Actions — download it from the latest run's artifacts.

## Credits

Built in a pair with **Jonathan Katsav** for the C#/.NET course at the Academic College of Tel Aviv-Yaffo (MTA).
