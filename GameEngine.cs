using Microsoft.Extensions.Logging;

namespace ThreeMileIsland;

/// <summary>
/// Main game engine that runs the Three Mile Island simulation.
/// Contains all the core simulation logic converted from the BASIC source.
/// </summary>
public class GameEngine
{
    private readonly GameState _state;
    private readonly LowResGraphics _graphics;
    private readonly ILogger<GameEngine> _logger;
    private readonly SoundSystem _sound;
    private readonly Dictionary<int, GameScreen> _screens;

    private bool _running = true;       
    private DateTime _lastUpdate = DateTime.Now;
    private const int SimulationTickMs = 1000; // Simulation speed

    // Pipe Status of Empty
    public const int Empty = 10;

    public GameEngine(
        GameState state,
        LowResGraphics graphics,
        SoundSystem sound,
        ILogger<GameEngine> logger,
        ContainmentScreen containmentScreen,
        TurbineScreen turbineScreen,
        ReactorCoreScreen reactorCoreScreen,
        PumpHouseScreen pumpHouseScreen,
        MaintenanceScreen maintenanceScreen,
        CostAnalysisScreen costAnalysisScreen,
        OperationalStatusScreen operationalStatusScreen)
    {
        _state = state;
        _graphics = graphics;
        _sound = sound;
        _logger = logger;

        _screens = new Dictionary<int, GameScreen>
        {
            { 0, containmentScreen },
            { 1, turbineScreen },
            { 2, reactorCoreScreen },
            { 3, pumpHouseScreen },
            { 4, maintenanceScreen },
            { 5, costAnalysisScreen },
            { 6, operationalStatusScreen }
        };

        logger.LogInformation("GameEngine initialized");
    }

    /// <summary>
    /// Run the game
    /// </summary>
    public void Run()
    {
        Console.CursorVisible = false;
        Console.Title = "Three Mile Island";

        try
        {
            Console.SetWindowSize(80, 30);
            Console.SetBufferSize(80, 30);
        }
        catch { /* Ignore if resize fails */ }

        _state.Initialize();
        ShowWelcome();
        ShowMainMenu();

        while (_running)
        {
            ProcessInput();

            if (_state.SimulationRunning && _state.CurrentScreen >= 0)
            {
                var now = DateTime.Now;
                if ((now - _lastUpdate).TotalMilliseconds >= SimulationTickMs)
                {
                    RunSimulationTick();
                    _lastUpdate = now;
                }
            }

            Thread.Sleep(10);
        }

        Console.CursorVisible = true;
        Console.Clear();
    }

    /// <summary>
    /// Show welcome screen (lines 32700-32746)
    /// </summary>
    private void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("WELCOME TO ...");
        Console.SetCursorPosition(3, 4);
        Console.WriteLine("THREE MILE ISLAND NUCLEAR FACILITY");
        Console.SetCursorPosition(0, 9);
        Console.WriteLine("THE REACTOR IS IN A COLD SHUT-DOWN");
        Console.WriteLine();
        Console.WriteLine("STATE WITH ALL SYSTEMS IN-ACTIVE.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("THE DATE IS DECEMBER 30, 1978, AND THE");
        Console.WriteLine();
        Console.WriteLine("REACTOR HAS BEEN CERTIFIED OPERATIONAL.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("INITIALIZE ACCORDING TO OFFICIAL MUSE");
        Console.WriteLine();
        Console.WriteLine("GUIDELINES (Y OR N) ?");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Y)
                {
                    InitializeReactor();
                    break;
                }
                if (key.Key == ConsoleKey.N || key.Key == ConsoleKey.Escape)
                    break;
            }
            Thread.Sleep(10);
        }
    }

    /// <summary>
    /// Initialize reactor to running state per MUSE guidelines.
    /// Replays the original BASIC command string from lines 32740-32746:
    /// "1JM&lt;ctrl-V&gt;KBCIJ&lt;ctrl-F&gt;AB&lt;ctrl-T&gt;A0DE&lt;ctrl-V&gt;GFH" + control rod raises.
    /// </summary>
    private void InitializeReactor()
    {
        // Screen 1 (Turbine) - Turn on pumps J(10), M(13)
        _state.PumpStatus[10] = PumpStatus.On;  // J
        _state.PumpStatus[13] = PumpStatus.On;  // M

        // Screen 1 - Open valves K(11), B(2), C(3), I(9), J(10)
        _state.ValveStatus[11] = ValveStatus.Open; // K
        _state.ValveStatus[2] = ValveStatus.Open;  // B
        _state.ValveStatus[3] = ValveStatus.Open;  // C
        _state.ValveStatus[9] = ValveStatus.Open;  // I
        _state.ValveStatus[10] = ValveStatus.Open; // J

        // Screen 1 - Activate filters A(1), B(2) (set to clean)
        _state.FilterStatus[1] = FilterStatus.Clean;
        _state.FilterStatus[2] = FilterStatus.Clean;

        // Screen 1 - Activate turbine A(1)
        _state.TurbineActive[1] = TurbineStatus.Online;
        _state.TurbineCount = 1;

        // Screen 0 (Containment) - Turn on pumps D(4), E(5)
        _state.PumpStatus[4] = PumpStatus.On;   // D
        _state.PumpStatus[5] = PumpStatus.On;   // E

        // Screen 0 - Open valves G(7), F(6), H(8)
        _state.ValveStatus[7] = ValveStatus.Open;  // G
        _state.ValveStatus[6] = ValveStatus.Open;  // F
        _state.ValveStatus[8] = ValveStatus.Open;  // H

        // Screen 2 (Reactor Core) - Raise all 18 control rods to position 30
        for (int r = 1; r <= 18; r++)
        {
            _state.ControlRodPosition[r] = 30;
            _state.ControlRodColor[r] = 9;
            _state.ControlRodTemp += 30;
        }

        // Update cluster counts
        _state.ControlRodClusterCount[1] = 9 * 30;
        _state.ControlRodClusterCount[2] = 9 * 30;

        // Set initial core temperature
        _state.CoreTemperature = 200;

        // Update pump cluster counts and pipe statuses to reflect the new state
        UpdatePumpClusters();
        UpdateTurbineCount();
        UpdatePipeStatuses();
    }

    /// <summary>
    /// Show main menu (lines 8000-8099)
    /// </summary>
    private void ShowMainMenu()
    {
        Console.Clear();
        _state.CurrentScreen = -1;

        Console.SetCursorPosition(11, 2);
        Console.WriteLine("THREE MILE ISLAND");
        Console.SetCursorPosition(15, 4);
        Console.WriteLine("MAIN MENU");

        Console.SetCursorPosition(4, 6);
        Console.WriteLine("0  CONTAINMENT");
        Console.SetCursorPosition(4, 8);
        Console.WriteLine("1  TURBINE, FILTER, CONDENSER");
        Console.SetCursorPosition(4, 10);
        Console.WriteLine("2  REACTOR CORE");
        Console.SetCursorPosition(4, 12);
        Console.WriteLine("3  PUMP HOUSE");
        Console.SetCursorPosition(4, 14);
        Console.WriteLine("4  MAINTENANCE SCHEDULE");
        Console.SetCursorPosition(4, 16);
        Console.WriteLine("5  COST ANALYSIS");
        Console.SetCursorPosition(4, 18);
        Console.WriteLine("6  OPERATIONAL STATUS");
        Console.SetCursorPosition(4, 20);
        Console.WriteLine("ESC  EXIT GAME");

        WriteTimeLine();
    }

    /// <summary>
    /// Write time line at bottom
    /// </summary>
    private void WriteTimeLine()
    {
        Console.SetCursorPosition(16, 25);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(_state.FormatTime(_state.SimulationCount, true));
        Console.ResetColor();
        WriteStatusLine();
    }

    /// <summary>
    /// Helper to write status line at bottom
    /// </summary>
    private void WriteStatusLine()
    {
        Console.SetCursorPosition(0, 24);
        Console.Write($"TEMP={_state.CoreTemperature}  ");
        Console.SetCursorPosition(19, 24);
        Console.Write($"CNT={_state.PumpsRequired}");
    }

    /// <summary>
    /// Process keyboard input
    /// </summary>
    private void ProcessInput()
    {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true);

        // ESC toggles simulation or returns to menu
        if (key.Key == ConsoleKey.Escape)
        {
            if (_state.CurrentScreen >= 0)
            {
                _state.SimulationRunning = !_state.SimulationRunning;
                if (_state.CurrentScreen == -1)
                {
                    ShowMainMenu();
                }
            }
            else
            {
                _running = false;
            }
            return;
        }

        // Handle menu selection
        if (_state.CurrentScreen == -1)
        {
            if (key.KeyChar >= '0' && key.KeyChar <= '6')
            {
                _state.CurrentScreen = key.KeyChar - '0';
                _state.CurrentLabel = 1;
                SwitchToScreen(_state.CurrentScreen);
            }
            return;
        }

        // Number keys return to menu then switch screen
        if (key.KeyChar >= '0' && key.KeyChar <= '6')
        {
            _state.CurrentScreen = key.KeyChar - '0';
            _state.CurrentLabel = 1;
            SwitchToScreen(_state.CurrentScreen);
            return;
        }

        // Enter returns to main menu
        if (key.Key == ConsoleKey.Enter)
        {
            ShowMainMenu();
            return;
        }

        // Pass input to current screen
        if (_screens.TryGetValue(_state.CurrentScreen, out var screen))
        {
            screen.HandleInput(key);
        }
    }

    /// <summary>
    /// Switch to a specific screen
    /// </summary>
    private void SwitchToScreen(int screenIndex)
    {
        if (_screens.TryGetValue(screenIndex, out var screen))
        {
            Console.Clear();
            screen.Draw();
            screen.Render();
            screen.ShowLabel();
            WriteTimeLine();
        }
    }

    /// <summary>
    /// Run one simulation tick (lines 1000-1099)
    /// </summary>
    private void RunSimulationTick()
    {
        _sound.Click();
        _logger.LogDebug("Simulation tick started");
        _logger.LogDebug("Old temperature: {OldTemperature}", _state.OldTemperature);

        // Store old temperature
        _state.OldTemperature = _state.CoreTemperature;

        // Calculate temperature changes
        CalculateCoreTemperature();

        // Run main simulation
        RunCoreSimulation();

        // Check for day change
        _state.SimulationCount++;
        if (_state.SimulationCount >= 1440)
        {
            DayChange();
        }

        // Check for demand change
        if (_state.DemandCount > 0)
        {
            _state.DemandCount--;
            if (_state.DemandCount == 0)
            {
                DemandChange();
            }
        }

        // Update displays
        _state.UpdateBuildingOld();
        UpdateCurrentScreen();

        // Check for failures
        CheckFailures();

        // Check for catastrophic events
        CheckCatastrophicEvents();
    }

    /// <summary>
    /// Calculate core temperature (lines 1002-1006)
    /// </summary>
    private void CalculateCoreTemperature()
    {
        // Line 1002: IF NOT TEMP THEN 1010 - skip calculation when reactor is cold
        if (_state.CoreTemperature == 0)
            return;

        // Temperature increases from control rods, decreases from cooling
        _state.CoreTemperature += (_state.BuildingBuffer[2] < _state.PumpsRequired ? 1 : 0) * _state.PumpsRequired;
        _state.CoreTemperature += (_state.PipeStatus[3] == 10 || _state.BuildingBuffer[4] == 14 ? 1 : 0) * _state.PumpsRequired;
        _state.CoreTemperature += Math.Sign(_state.PumpsRequired -
            ((_state.PipeStatus[11] != 10 ? 1 : 0) * _state.PumpCluster1 +
             (_state.PipeStatus[8] != 10 ? 1 : 0) * _state.PumpCluster2)) * _state.PumpsRequired;
        _state.CoreTemperature -= (_state.BuildingBuffer[4] < 14 && _state.BuildingBuffer[6] < 12 &&
                               _state.PipeStatus[3] != 10 ? 1 : 0);
    }

    /// <summary>
    /// Run core simulation logic (lines 1030-1094)
    /// </summary>
    private void RunCoreSimulation()
    {
        // Valve 1 automatic control
        int v = 1;
        if ((_state.BuildingOld[3] > 11 && _state.ValveStatus[v] == ValveStatus.Shut) ||
            (_state.BuildingOld[3] < 2 && _state.ValveStatus[v] == ValveStatus.Open))
        {
            ToggleValve(v);
        }

        // ECCS pump activation check
        if (_state.BuildingBuffer[2] == 0 && _state.PumpCluster1 == 0 && _state.SimulationCount % 10 == 0)
        {
            for (int u = 1; u <= 3; u++)
            {
                if (_state.PumpStatus[u] == PumpStatus.Off)
                    TogglePump(u);
            }
            // Trigger emergency notifications
            _state.EmergencyFlag = true;
        }

        // Containment water pump check
        if (_state.BuildingOld[7] > 0 && _state.PumpCluster8 == 0)
        {
            for (int u = 22; u <= 24; u++)
            {
                if (_state.PumpCluster8 > 0) break;
                if (_state.PumpStatus[u] == PumpStatus.Off)
                    TogglePump(u);
            }
        }

        // Update building buffers
        UpdateBuildingBuffers();

        // Update pipe statuses based on current state (lines 10770, 1062-1064)
        UpdatePipeStatuses();

        // Calculate electrical output
        CalculateElectricOutput();

        // Update counters
        UpdateCounters();
    }

    /// <summary>
    /// Update building buffer values (lines 1040-1094)
    /// </summary>
    private void UpdateBuildingBuffers()
    {
        // Buffer 1: Containment pressure
        _state.BuildingBuffer[1] = _state.BuildingOld[1] +
            ((_state.ValveStatus[1] == ValveStatus.Open && _state.CoreTemperature > GameState.TempThreshold1 ? 1 : 0) -
             (_state.CoreTemperature < GameState.TempThreshold3 && _state.BuildingOld[1] > 9 ? 10 : 0) +
             (_state.PrimaryLeak ? 1 : 0));

        // Buffer 7: Containment water
        _state.BuildingBuffer[7] = _state.BuildingOld[7] +
            ((_state.PipeStatus[9] != 10 && (_state.PrimaryLeak || (_state.ValveStatus[1] == ValveStatus.Open && _state.BuildingOld[3] > 23)) ? 1 : 0) +
             (_state.BuildingOld[1] > 9 && _state.CoreTemperature < GameState.TempThreshold3 ? 1 : 0) -
             (_state.PipeStatus[12] != 10 && _state.BuildingOld[7] > 0 ? 1 : 0));

        // Clamp values
        _state.BuildingBuffer[1] = Math.Clamp(_state.BuildingBuffer[1], 0, 100);
        _state.BuildingBuffer[7] = Math.Clamp(_state.BuildingBuffer[7], 0, 100);

        // Buffer 2: PCS Pressure
        if (_state.ValveStatus[1] == ValveStatus.Open)
        {
            _state.BuildingBuffer[2] = _state.BuildingOld[2] -
                ((_state.BuildingOld[3] == 0 && _state.BuildingOld[2] > 1) ? 1 : 0);
        }
        else if (_state.PipeStatus[9] == 10)
        {
            _state.BuildingBuffer[2] = _state.BuildingOld[2] -
                ((_state.CoreTemperature > GameState.TempThreshold1 && _state.BuildingOld[3] == 0 && _state.BuildingOld[2] > 0) ? 1 : 0);
        }
        else if (_state.CoreTemperature >= GameState.TempThreshold3)
        {
            _state.BuildingBuffer[2] = _state.BuildingOld[2] +
                ((_state.BuildingOld[2] > 11 && _state.BuildingOld[3] == 12) ? 1 : 0) +
                ((_state.BuildingOld[2] < 12) ? 1 : 0);
        }
        else
        {
            _state.BuildingBuffer[2] = _state.BuildingOld[2] +
                ((_state.BuildingOld[2] < 12) ? 1 : 0) -
                ((_state.BuildingOld[2] > 12) ? 1 : 0);
        }

        // Buffer 2 leak adjustment
        _state.BuildingBuffer[2] -= (_state.PrimaryLeak && _state.PipeStatus[10] == Empty && _state.BuildingOld[2] > 0) ? 1 : 0;
        _state.BuildingBuffer[2] = Math.Clamp(_state.BuildingBuffer[2], 0, 100);

        // Buffer 5: Core vessel steam
        _state.BuildingBuffer[5] = _state.BuildingOld[5] +
            ((_state.BuildingOld[2] == 0 && _state.CoreTemperature > GameState.TempThreshold3) ? 1 : 0) -
            ((_state.BuildingOld[5] > 0 && _state.BuildingOld[2] > 0) ? 1 : 0);
        _state.BuildingBuffer[5] = Math.Clamp(_state.BuildingBuffer[5], 0, 100);

        // Buffer 3: Pressurizer water
        if (_state.ValveStatus[1] == ValveStatus.Open)
        {
            _state.BuildingBuffer[3] = _state.BuildingOld[3] -
                ((_state.BuildingOld[2] > 1 && _state.BuildingOld[3] > 0) ? 1 : 0) +
                ((_state.PipeStatus[9] != 10 && _state.BuildingOld[2] < 2 && _state.BuildingOld[3] < 24) ? 1 : 0);
        }
        else if (_state.PipeStatus[9] == 10)
        {
            _state.BuildingBuffer[3] = _state.BuildingOld[3] -
                ((_state.CoreTemperature > GameState.TempThreshold1 && _state.BuildingOld[3] > 0) ? 1 : 0);
        }
        else if (_state.CoreTemperature >= GameState.TempThreshold3)
        {
            _state.BuildingBuffer[3] = _state.BuildingOld[3] +
                ((_state.BuildingOld[3] < 12 && _state.BuildingOld[2] == 12 && _state.CoreTemperature > _state.OldTemperature) ? 1 : 0);
        }
        else
        {
            _state.BuildingBuffer[3] = _state.BuildingOld[3] +
                ((_state.BuildingOld[2] == 12 && _state.BuildingOld[3] < 6) ? 1 : 0) -
                ((_state.BuildingOld[2] == 12 && _state.BuildingOld[3] > 6) ? 1 : 0);
        }
        _state.BuildingBuffer[3] = Math.Clamp(_state.BuildingBuffer[3], 0, 24);

        // Buffer 4: Steamer level (line 1060)
        // Original: BU(4) = BO(4) + (PI(2) = 10 AND TEMP > TMP1 AND BO(4) < 14) - (PI(2) # 10 AND BO(4))
        _state.BuildingBuffer[4] = _state.BuildingOld[4] +
            ((_state.PipeStatus[2] == Empty && _state.CoreTemperature > GameState.TempThreshold1 && _state.BuildingOld[4] < 14) ? 1 : 0) -
            ((_state.PipeStatus[2] != Empty && _state.BuildingOld[4] > 0) ? 1 : 0);
        _state.BuildingBuffer[4] = Math.Clamp(_state.BuildingBuffer[4], 0, 14);

        // Buffer 6: Condenser level
        _state.BuildingBuffer[6] = _state.BuildingOld[6] +
            (((_state.PipeStatus[17] == 10 || _state.PipeStatus[1] == 10) && _state.CoreTemperature > GameState.TempThreshold1 && _state.BuildingOld[6] < 10) ? 1 : 0) -
            ((_state.PipeStatus[17] != 10 && _state.PipeStatus[1] != 10 && _state.BuildingOld[6] > 0) ? 1 : 0);
        _state.BuildingBuffer[6] = Math.Clamp(_state.BuildingBuffer[6], 0, 10);

        // Buffer 8-11: Pump house water and radiation
        _state.BuildingBuffer[8] = _state.BuildingOld[8] +
            ((_state.PumpCluster8 > 0 && _state.ValveStatus[8] == ValveStatus.Open && _state.BuildingOld[7] > 0 && _state.BuildingOld[8] < 50) ? 1 : 0);

        _state.BuildingBuffer[9] = _state.BuildingOld[9] +
            ((_state.PumpCluster7 > 0 && _state.ValveStatus[19] == ValveStatus.Open && _state.BuildingOld[10] > 0 && _state.BuildingOld[9] < 75) ? 1 : 0);

        _state.BuildingBuffer[10] = _state.BuildingOld[10] +
            ((_state.PumpCluster8 > 0 && _state.ValveStatus[8] == ValveStatus.Open && _state.BuildingOld[7] > 0 && _state.BuildingOld[8] == 50) ? 1 : 0) -
            ((_state.PumpCluster7 > 0 && _state.ValveStatus[19] == ValveStatus.Open && _state.BuildingOld[10] > 0 && _state.BuildingOld[9] < 75) ? 1 : 0);
        _state.BuildingBuffer[10] = Math.Clamp(_state.BuildingBuffer[10], 0, 100);

        _state.BuildingBuffer[11] = _state.BuildingOld[11] +
            ((_state.BuildingOld[10] > 0) ? 1 : 0) + _state.BuildingOld[10] / 10 -
            ((_state.BuildingOld[10] == 0 && _state.BuildingOld[11] > 0) ? 1 : 0) -
            ((_state.BuildingOld[11] > 1) ? 1 : 0);
        _state.BuildingBuffer[11] = Math.Clamp(_state.BuildingBuffer[11], 0, 1000);
    }

    /// <summary>
    /// Calculate electrical output (lines 1200, 2360, 2102)
    /// </summary>
    private void CalculateElectricOutput()
    {
        // Output based on turbine count and steamer status
        _state.ElectricOutput = 0;
        if (_state.TurbineCount > 0 && _state.BuildingBuffer[4] > 10)
        {
            _state.ElectricOutput = Math.Min(_state.ElectricDemand, _state.TurbineCount * 300);
        }

        // Update profit calculations
        int revenue = _state.ElectricOutput / 10;
        int costs = (_state.OperatingCost + _state.MaintenanceCost) / 10;
        _state.ProjectedProfit = (_state.ElectricDemand / 10) - costs;
        _state.ActualProfit += revenue - costs / 1440; // Per-tick profit
    }

    /// <summary>
    /// Update countdown timers
    /// </summary>
    private void UpdateCounters()
    {
        // Decrement valve countdowns
        for (int v = 1; v <= 19; v++)
        {
            if (_state.ValveCountdown[v] > 0)
                _state.ValveCountdown[v]--;
        }

        // Decrement pump countdowns
        for (int u = 1; u <= 24; u++)
        {
            if (_state.PumpCountdown[u] > 0)
                _state.PumpCountdown[u]--;
        }

        // Decrement turbine countdowns
        for (int t = 1; t <= 4; t++)
        {
            if (_state.TurbineCountdown[t] > 0)
                _state.TurbineCountdown[t]--;
        }

        // Decrement gauge countdowns
        for (int g = 1; g <= 11; g++)
        {
            if (_state.GaugeCountdown[g] > 0)
                _state.GaugeCountdown[g]--;
        }

        // Decrement filter countdowns
        for (int f = 1; f <= 3; f++)
        {
            if (_state.FilterCondition[f] > 0)
                _state.FilterCondition[f]--;
        }

        // Flush countdown
        if (_state.AirLeak && _state.FlushCountdown > 0)
            _state.FlushCountdown--;
    }

    /// <summary>
    /// Handle day change (lines 1100-1120)
    /// </summary>
    private void DayChange()
    {
        _state.SimulationCount = 0;
        _state.Day++;
        _state.OperatingCost += _state.Rnd.Next(10) + 1;
        _state.MaintenanceCost = 0;

        _sound.DayChange();
    }

    /// <summary>
    /// Handle demand change (lines 1900-1950)
    /// </summary>
    private void DemandChange()
    {
        _state.DemandCount = GameState.DemandChange0 + _state.Rnd.Next(GameState.DemandChange1) * (_state.Rnd.Next(3) - 1);

        int change = GameState.DemandMod0 + _state.Rnd.Next(GameState.DemandMod1);
        _state.ElectricDemand += (_state.SimulationCount < 720 ? change : -change / 2);
        _state.ElectricDemand = Math.Clamp(_state.ElectricDemand, 100, 1000);

        _sound.DemandChange();
    }

    /// <summary>
    /// Check for equipment failures (lines 1300, 1800, 3500)
    /// </summary>
    private void CheckFailures()
    {
        // Check valve failures
        for (int v = 1; v <= 19; v++)
        {
            if (_state.ValveCountdown[v] <= 0)
            {
                if (_state.ValveStatus[v] == ValveStatus.Repair)
                {
                    // Valve repaired
                    _state.ValveCountdown[v] = _state.Rnd.Next(GameState.ValveFailure1) + GameState.ValveFailure0;
                    _state.ValveStatus[v] = ValveStatus.Shut;
                    _sound.Alert();
                }
                else
                {
                    // Valve failed
                    _state.ValveCountdown[v] = 32767;
                    _sound.Beep();
                }
            }
        }

        // Check pump failures
        for (int u = 1; u <= 24; u++)
        {
            if (_state.PumpCountdown[u] <= 0)
            {
                if (_state.PumpStatus[u] == PumpStatus.Repair)
                {
                    _state.PumpCountdown[u] = _state.Rnd.Next(GameState.PumpFailure1) + GameState.PumpFailure0;
                    _state.PumpStatus[u] = PumpStatus.Off;
                    _sound.Alert();
                }
                else
                {
                    _state.PumpCountdown[u] = 32767;
                    _sound.Beep();
                }
                UpdatePumpClusters();
            }
        }

        // Check turbine failures
        for (int t = 1; t <= 4; t++)
        {
            if (_state.TurbineCountdown[t] <= 0)
            {
                if (_state.TurbineActive[t] == TurbineStatus.Repair)
                {
                    _state.TurbineCountdown[t] = GameState.TurbineFailure0 + _state.Rnd.Next(GameState.TurbineFailure1);
                    _state.TurbineActive[t] = TurbineStatus.Offline;
                    _sound.Alert();
                }
                else
                {
                    _state.TurbineCountdown[t] = 32767;
                    _sound.Beep();
                }
                UpdateTurbineCount();
            }
        }

        // Check filter soot buildup
        for (int f = 1; f <= 3; f++)
        {
            if (_state.FilterStatus[f] == FilterStatus.Dirty && _state.FilterCondition[f] <= 0)
            {
                _state.FilterSootLevel[f]++;
                _state.FilterSootPressure[f] = _state.Rnd.Next(10 + _state.FilterSootLevel[f]) + _state.FilterSootLevel[f];

                if (_state.FilterSootLevel[f] > 10)
                {
                    // Filter clogged
                    _state.FilterCondition[f] = 0;
                    _state.FilterSootPressure[f] = _state.Rnd.Next(26) + 10;
                    _sound.Beep();
                }
                else
                {
                    _state.FilterCondition[f] = _state.Rnd.Next(GameState.SootRepair1) + GameState.SootRepair0;
                }
            }
        }
    }

    /// <summary>
    /// Update pump cluster counts
    /// </summary>
    private void UpdatePumpClusters()
    {
        _state.PumpCluster1 = ((_state.PumpStatus[1] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[2] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[3] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster2 = ((_state.PumpStatus[4] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[5] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[6] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster3 = ((_state.PumpStatus[7] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[8] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[9] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster4 = ((_state.PumpStatus[10] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[11] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[12] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster5 = ((_state.PumpStatus[13] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[14] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[15] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster6 = ((_state.PumpStatus[16] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[17] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[18] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster7 = ((_state.PumpStatus[19] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[20] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[21] == PumpStatus.On) ? 1 : 0);
        _state.PumpCluster8 = ((_state.PumpStatus[22] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[23] == PumpStatus.On) ? 1 : 0) + ((_state.PumpStatus[24] == PumpStatus.On) ? 1 : 0);
    }

    /// <summary>
    /// Update turbine count
    /// </summary>
    private void UpdateTurbineCount()
    {
        _state.TurbineCount = ((_state.TurbineActive[1] == TurbineStatus.Online) ? 1 : 0) +
                              ((_state.TurbineActive[2] == TurbineStatus.Online) ? 1 : 0) +
                              ((_state.TurbineActive[3] == TurbineStatus.Online) ? 1 : 0) +
                              ((_state.TurbineActive[4] == TurbineStatus.Online) ? 1 : 0);
    }

    /// <summary>
    /// Check for catastrophic events (lines 20000-22115)
    /// </summary>
    private void CheckCatastrophicEvents()
    {
        // Check for game over due to losses
        if (_state.ActualProfit < GameState.LossThreshold2)
        {
            ShowGameOver("EXCESSIVE LOSSES");
            return;
        }

        // Check for containment pressure exceeded
        if (_state.BuildingBuffer[1] > _state.ContainmentPressure)
        {
            ContainmentSealed();
        }

        // Check for primary cooling leak
        if (_state.BuildingBuffer[2] > _state.LeakThreshold && !_state.PrimaryLeak)
        {
            PrimaryLeakEvent();
        }

        // Check for radiation emergency
        if (_state.BuildingBuffer[11] > _state.RadiationLevel1)
        {
            RadiationEmergency();
        }

        // Check for fuel rod damage
        _state.FuelRodDamage = _state.FuelRodDamage || (_state.BuildingBuffer[2] == 0 && _state.BuildingBuffer[5] > 0);

        // Check for meltdown
        if (_state.CoreTemperature > GameState.TempMeltdown)
        {
            Meltdown();
        }
    }

    /// <summary>
    /// Handle containment sealed event (lines 20100-20190)
    /// </summary>
    private void ContainmentSealed()
    {
        _sound.EmergencyAlarm();
        ShowEmergencyNotice();

        Console.WriteLine("THE CONTAINMENT HAS JUST SEALED");
        Console.WriteLine();
        Console.WriteLine("ITSELF AS A RESULT OF EXCESSIVE");
        Console.WriteLine();
        Console.WriteLine("PRESSURE DUE TO STEAM ESCAPING FROM");
        Console.WriteLine();
        Console.WriteLine("PRESSURIZER RELIEF VALVE A.");

        // Scram reactor
        ScramReactor();
        _state.ContainmentPressure = 32767;

        WaitForKey();
    }

    /// <summary>
    /// Handle primary leak event (lines 20600-20650)
    /// </summary>
    private void PrimaryLeakEvent()
    {
        _sound.EmergencyAlarm();
        ShowEmergencyNotice();

        Console.WriteLine("EXCESSIVE PRESSURE IN THE PRIMARY CORE");
        Console.WriteLine();
        Console.WriteLine("COOLING SYSTEM HAS CAUSED A LEAK.");

        _state.PrimaryLeak = true;
        WaitForKey();
    }

    /// <summary>
    /// Handle radiation emergency (lines 20800-20845)
    /// </summary>
    private void RadiationEmergency()
    {
        _sound.EmergencyAlarm();
        ShowEmergencyNotice();

        Console.WriteLine($"EXTREME DANGER TO ALL PEOPLE IN {_state.RadiationLevel1}");
        Console.WriteLine();
        Console.WriteLine("MILE AREA SURROUNDING THE REACTOR.  AN");
        Console.WriteLine();
        Console.WriteLine("IMMEDIATE EVACUATION IS NOW REQUIRED");
        Console.WriteLine();
        Console.WriteLine("TO LIMIT THE POTENTIAL INCREASE IN");
        Console.WriteLine();
        Console.WriteLine("FUTURE LIABILITY INSURANCE PREMIUMS.");

        _state.RadiationLevel1 += 50;
        WaitForKey();
    }

    /// <summary>
    /// Handle meltdown (lines 22000-22115)
    /// </summary>
    private void Meltdown()
    {
        ShowEmergencyNotice();

        Console.SetCursorPosition(14, 11);
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write("MELT-DOWN");
        Console.ResetColor();

        _sound.Meltdown();

        // Dramatic visual effect
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 0; i < 100; i++)
        {
            Console.SetCursorPosition(_state.Rnd.Next(40), _state.Rnd.Next(24));
            Console.Write("*");
            Thread.Sleep(50);
        }

        ShowGameOver("MELTDOWN");
    }

    /// <summary>
    /// Show emergency notice header (lines 20700-20760)
    /// </summary>
    private void ShowEmergencyNotice()
    {
        Console.Clear();
        _state.EmergencyNoticeNumber++;

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(7, 0);
        Console.Write("EMERGENCY");
        Console.SetCursorPosition(17, 0);
        Console.Write("NOTICE");
        Console.SetCursorPosition(24, 0);
        Console.Write("NUMBER");
        Console.SetCursorPosition(31, 0);
        Console.Write(_state.EmergencyNoticeNumber);
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine();
    }

    /// <summary>
    /// Scram the reactor (lines 20400-20430)
    /// </summary>
    private void ScramReactor()
    {
        Console.SetCursorPosition(0, 19);
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("REACTOR HAS BEEN SCRAMMED");
        Console.ResetColor();

        // Insert all control rods
        for (int r = 1; r <= 18; r++)
        {
            _state.ControlRodPosition[r] = 0;
            _state.ControlRodColor[r] = 12;
        }

        _state.ControlRodTemp = 0;
        _state.ControlRodClusterCount[1] = 0;
        _state.ControlRodClusterCount[2] = 0;
    }

    /// <summary>
    /// Show game over screen (lines 20000-20036)
    /// </summary>
    private void ShowGameOver(string reason)
    {
        Console.Clear();
        Console.SetCursorPosition(9, 0);
        Console.WriteLine("OFFICIAL NOTIFICATION");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("FROM: PUBLIC UTILITIES COMMISSION");
        Console.WriteLine();
        Console.WriteLine("TO  : MANAGER, THREE MILE ISLAND");
        Console.WriteLine();
        Console.WriteLine($"RE  : {reason}");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("PLEASE BE ADVISED THAT DUE TO YOUR");
        Console.WriteLine();
        Console.WriteLine("DEMONSTRATED OPERATIONAL INCOMPETENCE,");
        Console.WriteLine();
        Console.WriteLine("THE PUBLIC UTILITIES COMMISSION IS");
        Console.WriteLine();
        Console.WriteLine("FORCED TO RELIEVE YOU OF YOUR LICENSE");
        Console.WriteLine();
        Console.WriteLine("TO OPERATE, EFFECTIVE IMMEDIATELY.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");

        Console.ReadKey(true);
        _running = false;
    }

    /// <summary>
    /// Wait for a key press
    /// </summary>
    private void WaitForKey()
    {
        while (!Console.KeyAvailable)
            Thread.Sleep(10);
        Console.ReadKey(true);
    }

    /// <summary>
    /// Update the current screen display
    /// </summary>
    private void UpdateCurrentScreen()
    {
        if (_screens.TryGetValue(_state.CurrentScreen, out var screen))
        {
            screen.Update();

            if (_state.CurrentScreen <= 3)
            {
                screen.Render();
                WriteTimeLine();
            }
        }
    }

    /// <summary>
    /// Toggle a valve
    /// </summary>
    private void ToggleValve(int v)
    {
        if (_state.ValveStatus[v] == ValveStatus.Repair || 
            _state.ValveCountdown[v] > GameState.ValveFailure1 + GameState.ValveFailure0)
            return;

        _state.ValveStatus[v] = _state.ValveStatus[v] == ValveStatus.Shut
            ? ValveStatus.Open
            : ValveStatus.Shut;
        _state.ValveCountdown[v] -= _state.Rnd.Next(GameState.ValveAdjust1) + GameState.ValveAdjust0;
        UpdatePipeStatuses();
    }

    /// <summary>
    /// Toggle a pump
    /// </summary>
    private void TogglePump(int u)
    {
        if (_state.PumpStatus[u] == PumpStatus.Repair ||
            _state.PumpCountdown[u] > GameState.PumpFailure1 + GameState.PumpFailure0)
            return;

        _state.PumpStatus[u] = _state.PumpStatus[u] == PumpStatus.Off
            ? PumpStatus.On
            : PumpStatus.Off;
        _state.PumpCountdown[u] -= _state.Rnd.Next(GameState.PumpAdjust1) + GameState.PumpAdjust0;
        UpdatePumpClusters();
        UpdatePipeStatuses();
    }

    /// <summary>
    /// Update pipe statuses based on current valve, pump, and buffer states.
    /// Consolidates the pipe flow logic from the original BASIC subroutines
    /// (4000-4975 for valves, 5000-5600 for pumps, 10770 for pipe 3,
    /// and lines 1062-1064 for pipe 1 dynamic updates).
    /// </summary>
    private void UpdatePipeStatuses()
    {
        // Pipe 3: Hot leg from core to steamer - based on PCS pressure (lines 10770-10775)
        if (_state.BuildingBuffer[2] > 0 && _state.PipeStatus[3] == Empty)
            _state.PipeStatus[3] = 2;
        else if (_state.BuildingBuffer[2] == 0 && _state.PipeStatus[3] != Empty)
            _state.PipeStatus[3] = Empty;

        // Pipe 10: ECCS pump cluster 1 output (lines 5000-5012)
        _state.PipeStatus[10] = _state.PumpCluster1 > 0 ? 2 : Empty;

        // Pipe 11: Through valve D(4) from ECCS (lines 5004-5006, 4075-4093)
        _state.PipeStatus[11] = (_state.PumpCluster1 > 0 && _state.ValveStatus[4] == ValveStatus.Open)
            ? _state.PipeStatus[10] : Empty;

        // Pipe 4: Pump cluster 2 output (lines 5075-5077)
        _state.PipeStatus[4] = _state.PumpCluster2 > 0 ? 2 : Empty;

        // Pipes 5, 6, 7: Through valves E(5), F(6), G(7) from secondary loop (lines 5078-5084)
        for (int i = 5; i <= 7; i++)
        {
            _state.PipeStatus[i] = (_state.PumpCluster2 > 0 && _state.ValveStatus[i] == ValveStatus.Open)
                ? _state.PipeStatus[4] : Empty;
        }

        // Pipe 8: Secondary cooling output - flows when any of valves 5/6/7 open with pumps (lines 5085)
        _state.PipeStatus[8] = (_state.PumpCluster2 > 0 &&
            (_state.ValveStatus[5] == ValveStatus.Open ||
             _state.ValveStatus[6] == ValveStatus.Open ||
             _state.ValveStatus[7] == ValveStatus.Open))
            ? _state.PipeStatus[4] : Empty;

        // Pipe 9: Pressurizer loop - combines ECCS (pipe 11) and secondary (pipe 8) (lines 5007, 5086)
        if (_state.PipeStatus[8] != Empty)
            _state.PipeStatus[9] = _state.PipeStatus[8];
        else if (_state.PipeStatus[11] != Empty)
            _state.PipeStatus[9] = _state.PipeStatus[11];
        else
            _state.PipeStatus[9] = Empty;

        // Pipe 20: ESCS pump cluster 3 output (lines 5150-5151)
        _state.PipeStatus[20] = _state.PumpCluster3 > 0 ? 2 : Empty;

        // Pipes 14, 15: Through valves I(9), J(10) from ESCS (lines 5153-5157, 4200-4225)
        _state.PipeStatus[14] = (_state.PumpCluster3 > 0 && _state.ValveStatus[9] == ValveStatus.Open)
            ? _state.PipeStatus[20] : Empty;
        _state.PipeStatus[15] = (_state.PumpCluster3 > 0 && _state.ValveStatus[10] == ValveStatus.Open)
            ? _state.PipeStatus[20] : Empty;

        // Pipe 13: ESCS combined output (lines 5157-5158, 5170-5172)
        if (_state.PipeStatus[14] != Empty || _state.PipeStatus[15] != Empty)
            _state.PipeStatus[13] = 7;
        else
            _state.PipeStatus[13] = _state.PipeStatus[16]; // Falls back to filter bypass

        // Pipe 2: Cold leg input through valve C(3) (lines 4050-4060, 5159-5160)
        _state.PipeStatus[2] = (_state.ValveStatus[3] == ValveStatus.Open && _state.PipeStatus[13] != Empty)
            ? _state.PipeStatus[13] : Empty;

        // Pipe 18: Pump cluster 4 output (lines 5225-5230)
        _state.PipeStatus[18] = _state.PumpCluster4 > 0 ? 6 : Empty;

        // Pipe 21: Through valve K(11) - uses PumpCluster5 (lines 4250-4265)
        _state.PipeStatus[21] = (_state.ValveStatus[11] == ValveStatus.Open && _state.PumpCluster5 > 0)
            ? 6 : Empty;

        // Pipe 22: Through valve L(12) (lines 4275→4255)
        _state.PipeStatus[22] = (_state.ValveStatus[12] == ValveStatus.Open && _state.PumpCluster5 > 0)
            ? 6 : Empty;

        // Pipe 17: Condenser feed - combines pump cluster 4 and valve K/L outputs (lines 4271-4272, 5231)
        if (_state.PipeStatus[21] != Empty || _state.PipeStatus[22] != Empty)
            _state.PipeStatus[17] = 6;
        else if (_state.PumpCluster4 > 0)
            _state.PipeStatus[17] = _state.PipeStatus[18];
        else
            _state.PipeStatus[17] = Empty;

        // Pipe 1: Steam pipe from steamer to turbine (lines 1062-1064, 4025-4040)
        if (_state.BuildingOld[4] >= 14 && _state.PipeStatus[1] != Empty)
            _state.PipeStatus[1] = Empty;
        else if (_state.BuildingOld[4] < 14 && _state.PipeStatus[1] == Empty && _state.ValveStatus[2] == ValveStatus.Open)
            _state.PipeStatus[1] = 14;

        // Pipe 12: Containment drain through valve H(8) (lines 4175-4192)
        _state.PipeStatus[12] = (_state.ValveStatus[8] == ValveStatus.Open && _state.PumpCluster8 > 0 && _state.BuildingBuffer[7] > 0)
            ? 2 : Empty;

        // Pipe 19: Filter pump output (lines 5300-5306)
        _state.PipeStatus[19] = _state.PumpCluster5 > 0 ? 6 : Empty;

        // Pipe 23: Pump house discharge through valve S(19) (lines 4450-4462)
        _state.PipeStatus[23] = (_state.ValveStatus[19] == ValveStatus.Open && _state.PumpCluster7 > 0 && _state.BuildingBuffer[10] > 0)
            ? 2 : Empty;
    }
}
