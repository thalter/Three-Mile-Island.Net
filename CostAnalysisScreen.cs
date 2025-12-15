using System;

namespace ThreeMileIsland;

/// <summary>
/// Screen 5: Cost Analysis view
/// Shows operating costs, maintenance, electric demand/output, and profit
/// </summary>
public class CostAnalysisScreen : GameScreen
{
    public CostAnalysisScreen(GameState state, LowResGraphics graphics, SoundSystem sound)
        : base(state, graphics, sound) { }

    public override void Draw()
    {
        Console.Clear();
        ShowCostAnalysis();
    }

    /// <summary>
    /// Display cost analysis (lines 8700-8785)
    /// </summary>
    private void ShowCostAnalysis()
    {
        Console.SetCursorPosition(14, 0);
        Console.WriteLine("COST ANALYSIS");

        Console.SetCursorPosition(0, 4);
        Console.WriteLine($"OPERATING COST:   $ {State.OperatingCost},000");
        Console.WriteLine();
        Console.Write($"MAINTENANCE COST: $ {State.MaintenanceCost}");
        if (State.MaintenanceCost > 0) Console.Write(",000");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        Console.Write($"ELECTRIC DEMAND: {State.ElectricDemand}");
        Console.SetCursorPosition(23, Console.CursorTop);
        Console.WriteLine("MEGAWATTS");
        Console.WriteLine();

        Console.Write($"ELECTRIC OUTPUT: {State.ElectricOutput}");
        Console.SetCursorPosition(23, Console.CursorTop);
        Console.WriteLine("MEGAWATTS");
        Console.WriteLine();

        Console.Write("(ELECTRIC DEMAND CHANGES AT ");
        int c = State.SimulationCount + State.DemandCount;
        Console.Write(State.FormatTime(c, true));
        Console.WriteLine(" )");
        Console.WriteLine();
        Console.WriteLine();

        Console.Write($"PROJECTED PROFIT: $ {State.ProjectedProfit}");
        if (State.ProjectedProfit > 0) Console.Write(",000");
        Console.WriteLine();
        Console.WriteLine();

        Console.Write("ACTUAL PROFIT:  ");
        if (State.ActualProfit < 0)
            Console.Write("< ");
        Console.SetCursorPosition(18, Console.CursorTop);
        Console.Write($"$ {Math.Abs(State.ActualProfit)}");
        if (State.ActualProfit != 0) Console.Write(",000");

        if (State.ActualProfit < 0)
        {
            Console.Write(" >");
            Console.SetCursorPosition(32, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("<LOSS>");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Check if rate increase petition is available (lines 8500-8520)
    /// </summary>
    private void CheckRateIncrease()
    {
        if (State.ActualProfit >= GameState.LossThreshold1) return;

        Console.SetCursorPosition(0, 21);
        Console.WriteLine("THE PUBLIC UTILITIES COMMISSION WILL");
        Console.WriteLine("ACCEPT A PETITION FOR A RATE INCREASE.");
    }

    /// <summary>
    /// Handle rate increase petition (lines 720-750)
    /// </summary>
    private void HandleRatePetition()
    {
        Console.SetCursorPosition(0, 21);

        if (State.Rnd.Next(100) > 89)
        {
            // Petition approved
            Console.WriteLine("PETITION APPROVED !");
            State.ActualProfit = 0;
            Sound.Alert();
        }
        else
        {
            // Petition denied
            Console.WriteLine("PETITION DENIED.");
            Sound.Beep();
        }
    }

    public override void Update()
    {
        ShowCostAnalysis();
        CheckRateIncrease();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        // Handle '$' for rate increase petition
        if (key.KeyChar == '$' && State.ActualProfit < GameState.LossThreshold1)
        {
            HandleRatePetition();
        }
    }

    public override void ShowLabel()
    {
        // Cost analysis is text-only, no special labels needed
    }

    public override void Render()
    {
        // Text-based screen, no graphics rendering needed
    }
}
