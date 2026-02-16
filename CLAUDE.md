# CLAUDE.md

## Project Overview

C# (.NET 8.0) conversion of the classic Three Mile Island nuclear reactor management simulation game, originally written in Integer BASIC for the Apple II (1979). Cross-platform console application.

## Build & Run

```bash
dotnet build          # Build the project
dotnet run            # Run the application
dotnet clean          # Clean build artifacts
dotnet restore        # Restore NuGet dependencies
```

## Project Structure

- `Program.cs` — Entry point, DI and logging setup
- `GameEngine.cs` — Main game loop, simulation ticker, screen management, input handling
- `GameState.cs` — All game state variables, arrays, and constants
- `GameScreen.cs` — Abstract base class for all screen implementations
- `*Screen.cs` — Concrete screen implementations (Turbine, Containment, Maintenance, PumpHouse, ReactorCore, OperationalStatus, CostAnalysis)
- `LowResGraphics.cs` — ASCII art rendering (mimics Apple II lo-res graphics)
- `SoundSystem.cs` — Audio feedback (console beeps)
- `ServiceCollectionExtensions.cs` — DI registration helpers
- `Original Source Code/` — Original BASIC source files for reference

## Architecture

- **Dependency Injection**: Microsoft.Extensions.DependencyInjection for service wiring
- **Logging**: Serilog with rolling file output (`logs/tmi-.log`)
- **Screen pattern**: Abstract `GameScreen` base class with screen-per-subsystem implementations
- **State management**: Centralized `GameState` class

## Code Conventions

- C# with nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Implicit usings enabled (C# 10+)
- PascalCase for public members, standard Microsoft C# naming conventions
- XML documentation comments (`///`) on public APIs
- Target framework: `net8.0`

## Testing

No test framework is currently configured.

## Linting / Formatting

No formal linting or code analysis tools are configured. Follow existing code style.
