# Three Mile Island - C# Conversion

This is a C# conversion of the classic Three Mile Island nuclear reactor simulation game originally written in Applesoft BASIC for the Apple II computer.

## About the Original Game

Three Mile Island was a nuclear reactor management simulation game that challenged players to safely operate a nuclear power plant while managing various systems including:
- Reactor core and control rods
- Primary and secondary cooling systems
- Turbines and generators
- Pumps and valves
- Emergency systems

The game featured real-time simulation of reactor physics, equipment failures, and economic considerations.

## Building and Running

### Prerequisites
- .NET 6.0 SDK or later

### To Build
```bash
dotnet build
```

### To Run
```bash
dotnet run
```

## How to Play

The game presents a main menu with numbered options:

0. **CONTAINMENT** - View containment vessel status
1. **TURBINE, FILTER, CONDENSER** - Manage power generation systems
2. **REACTOR CORE** - Monitor and control the reactor core
3. **PUMP HOUSE** - View pump house status
4. **MAINTENANCE SCHEDULE** - Check equipment maintenance status
5. **COST ANALYSIS** - View financial information
6. **OPERATIONAL STATUS** - View all system status and warnings
7. **SAVE / RESET STATE** - Save or load game state

Press the number keys (0-7) to navigate to different screens.
Press ESC to return to the main menu from any screen.

### Reactor Core Controls
- **+** or **=**: Insert control rods (increase reactivity)
- **-** or **_**: Withdraw control rods (decrease reactivity)

## Conversion Notes

This C# version is a modernized conversion that maintains the core game logic and simulation while adapting it to:
- Modern console I/O instead of Apple II text/graphics modes
- Object-oriented C# structure
- Cross-platform compatibility
- Simplified graphics rendering (text-based instead of lo-res graphics)

Some features from the original that are simplified or not yet implemented:
- Lo-res graphics displays (converted to text representations)
- Sound effects (Apple II speaker pokes)
- Save/load state functionality (planned for future update)
- Some advanced graphical displays

## Game Objective

Manage the nuclear power plant safely and profitably. Balance:
- Power output to meet demand
- Reactor temperature control
- Equipment maintenance
- Financial performance

Avoid:
- Meltdown (temperature too high)
- Containment breach
- Excessive financial losses
- Radiation leaks

## Original Source

This conversion is based on the Apple II BASIC source code from the original Three Mile Island game.

## License

This is a fan conversion for educational purposes. Original game rights belong to their respective owners.
