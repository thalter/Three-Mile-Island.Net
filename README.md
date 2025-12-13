# Three Mile Island - C# Conversion

<p align="center">
<img width="560" height="384" alt="three-mile-island_1" src="https://github.com/user-attachments/assets/45cee4df-aabf-4aeb-8a89-9de5a566132a" />
</p>

This is a C# conversion of the classic Three Mile Island nuclear reactor simulation game originally written in Integer BASIC for the Apple II computer.

## Background
It has been my life-long dream to bring one of my favorite Apple II games into the 21st century, but converting the original Integer BASIC code always seemed to be a huge lift. I decided to turn this into an exercise in "Vibe Coding" to see if AI could complete what I never had time to do, and I'm so glad I did. Thanks to the help of Claude Sonnet, I was able to do much of the work in a matter of minutes.

Like most AI generated code, Claude's first iteration wouldn't even compile, much less run. However, it gave me a huge leg up, and didn't take much effort on my part to get the code ship-shape and running.

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
- .NET 8.0 SDK or later. Will run on Windows, MacOS, or Linux.

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


## Future Plans

- Continue to modernize the code with improved method, property, and variable naming.
- Separate game logic from presentation layer to allow for a variety of front-ends (Blazor, Maui, Native App, etc.)
- Restore original low-res graphics and sounds.

## Original Source

This conversion is based on the Apple II Integer BASIC source code from the original Three Mile Island game. I've included the [original source code](https://github.com/thalter/Three-Mile-Island.Net/blob/main/tmi.bas) in this repo, however I make no claims of ownership of it.

## License

This is a fan conversion for educational purposes. Original game rights belong to their respective owners.
<p align="center">
<img width="560" height="384" alt="three-mile-island_7" src="https://github.com/user-attachments/assets/3814839f-f1c0-4317-9497-11ebd3d62ceb" />
</p>
