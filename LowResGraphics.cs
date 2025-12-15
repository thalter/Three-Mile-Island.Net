namespace ThreeMileIsland;

/// <summary>
/// ASCII art rendering system to mimic Apple II low-resolution graphics.
/// The Apple II low-res mode was 40x48 pixels with 16 colors.
/// We use Unicode block characters and ANSI colors to approximate this.
/// </summary>
public class LowResGraphics
{
    // Screen dimensions (Apple II low-res was 40x48, but we use 40x40 for the graphics area)
    public const int Width = 40;
    public const int Height = 40;

    // The virtual screen buffer - each cell holds a color index
    private readonly int[,] _screen = new int[Width, Height];

    // Current drawing color
    private int _currentColor = 15;

    // Apple II low-res color palette mapped to console colors
    private static readonly ConsoleColor[] ColorMap =
    [
        ConsoleColor.Black,        // 0 - Black
        ConsoleColor.DarkMagenta,  // 1 - Magenta (dark red/purple)
        ConsoleColor.DarkBlue,     // 2 - Dark Blue
        ConsoleColor.DarkMagenta,  // 3 - Purple/Violet
        ConsoleColor.DarkGreen,    // 4 - Dark Green
        ConsoleColor.DarkGray,     // 5 - Gray 1
        ConsoleColor.Blue,         // 6 - Medium Blue
        ConsoleColor.Cyan,         // 7 - Light Blue
        ConsoleColor.DarkYellow,   // 8 - Brown
        ConsoleColor.Red,          // 9 - Orange (approximated as red)
        ConsoleColor.Gray,         // 10 - Gray 2
        ConsoleColor.Magenta,      // 11 - Pink
        ConsoleColor.Green,        // 12 - Green
        ConsoleColor.Yellow,       // 13 - Yellow
        ConsoleColor.DarkCyan,     // 14 - Aqua
        ConsoleColor.White         // 15 - White
    ];

    // Color names for display
    private static readonly string[] ColorNames =
    [
        "BLK", "MAG", "DBL", "PUR", "DGR", "GR1", "BLU", "LBL",
        "BRN", "ORG", "GR2", "PNK", "GRN", "YEL", "AQU", "WHT"
    ];

    /// <summary>
    /// Clear the screen to black
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _screen[x, y] = 0;
    }

    /// <summary>
    /// Set the current drawing color (0-15)
    /// </summary>
    public void SetColor(int color)
    {
        _currentColor = Math.Clamp(color, 0, 15);
    }

    /// <summary>
    /// Plot a single pixel at x,y
    /// </summary>
    public void Plot(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            _screen[x, y] = _currentColor;
    }

    /// <summary>
    /// Draw a horizontal line from x1 to x2 at y
    /// </summary>
    public void HLine(int x1, int x2, int y)
    {
        if (y < 0 || y >= Height) return;
        int minX = Math.Max(0, Math.Min(x1, x2));
        int maxX = Math.Min(Width - 1, Math.Max(x1, x2));
        for (int x = minX; x <= maxX; x++)
            _screen[x, y] = _currentColor;
    }

    /// <summary>
    /// Draw a vertical line from y1 to y2 at x
    /// </summary>
    public void VLine(int y1, int y2, int x)
    {
        if (x < 0 || x >= Width) return;
        int minY = Math.Max(0, Math.Min(y1, y2));
        int maxY = Math.Min(Height - 1, Math.Max(y1, y2));
        for (int y = minY; y <= maxY; y++)
            _screen[x, y] = _currentColor;
    }

    /// <summary>
    /// Get the color at a specific position
    /// </summary>
    public int GetPixel(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return _screen[x, y];
        return 0;
    }

    /// <summary>
    /// Render the graphics buffer to the console.
    /// Uses Unicode half-block characters to display two rows per console line.
    /// </summary>
    public void Render(int startRow = 0)
    {
        Console.SetCursorPosition(0, startRow);

        // Use half-block characters: ▀ (upper half) and ▄ (lower half)
        // Each console line represents 2 graphics rows
        for (int y = 0; y < Height; y += 2)
        {
            for (int x = 0; x < Width; x++)
            {
                int topColor = _screen[x, y];
                int bottomColor = y + 1 < Height ? _screen[x, y + 1] : 0;

                if (topColor == bottomColor)
                {
                    // Both halves same color - use full block
                    Console.ForegroundColor = ColorMap[topColor];
                    Console.Write('█');
                }
                else
                {
                    // Different colors - use half block
                    Console.ForegroundColor = ColorMap[topColor];
                    Console.BackgroundColor = ColorMap[bottomColor];
                    Console.Write('▀');
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    /// <summary>
    /// Render a portion of the screen (for partial updates)
    /// </summary>
    public void RenderRegion(int startX, int startY, int width, int height, int consoleRow)
    {
        for (int y = startY; y < startY + height && y < Height; y += 2)
        {
            Console.SetCursorPosition(startX, consoleRow + (y - startY) / 2);
            for (int x = startX; x < startX + width && x < Width; x++)
            {
                int topColor = _screen[x, y];
                int bottomColor = y + 1 < Height ? _screen[x, y + 1] : 0;

                if (topColor == bottomColor)
                {
                    Console.ForegroundColor = ColorMap[topColor];
                    Console.Write('█');
                }
                else
                {
                    Console.ForegroundColor = ColorMap[topColor];
                    Console.BackgroundColor = ColorMap[bottomColor];
                    Console.Write('▀');
                }
            }
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Get console color for a low-res color index
    /// </summary>
    public static ConsoleColor GetConsoleColor(int colorIndex)
    {
        return ColorMap[Math.Clamp(colorIndex, 0, 15)];
    }

    /// <summary>
    /// Write text at a specific position with optional inverse video
    /// </summary>
    public static void WriteAt(int col, int row, string text, bool inverse = false)
    {
        Console.SetCursorPosition(col, row);
        if (inverse)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Write colored text at a specific position
    /// </summary>
    public static void WriteColoredAt(int col, int row, string text, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Clear the text area of the console
    /// </summary>
    public static void ClearText()
    {
        Console.Clear();
    }

    /// <summary>
    /// Clear a specific line
    /// </summary>
    public static void ClearLine(int row)
    {
        Console.SetCursorPosition(0, row);
        Console.Write(new string(' ', Console.WindowWidth > 0 ? Console.WindowWidth : 80));
        Console.SetCursorPosition(0, row);
    }

    /// <summary>
    /// Draw a filled rectangle
    /// </summary>
    public void FillRect(int x1, int y1, int x2, int y2)
    {
        int minX = Math.Max(0, Math.Min(x1, x2));
        int maxX = Math.Min(Width - 1, Math.Max(x1, x2));
        int minY = Math.Max(0, Math.Min(y1, y2));
        int maxY = Math.Min(Height - 1, Math.Max(y1, y2));

        for (int y = minY; y <= maxY; y++)
            for (int x = minX; x <= maxX; x++)
                _screen[x, y] = _currentColor;
    }

    /// <summary>
    /// Draw a box outline
    /// </summary>
    public void DrawBox(int x1, int y1, int x2, int y2)
    {
        HLine(x1, x2, y1);
        HLine(x1, x2, y2);
        VLine(y1, y2, x1);
        VLine(y1, y2, x2);
    }

    /// <summary>
    /// Simple ASCII art display for systems without Unicode support
    /// </summary>
    public void RenderAscii(int startRow = 0)
    {
        Console.SetCursorPosition(0, startRow);

        // Map colors to ASCII characters for simple display
        char[] colorChars = [' ', '.', ':', ';', '+', 'x', 'X', '#', '@', '%', '&', '*', 'O', '0', '=', '█'];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int color = _screen[x, y];
                Console.ForegroundColor = ColorMap[color];
                Console.Write(colorChars[color]);
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

/// <summary>
/// Apple II color constants for readability
/// </summary>
public static class Colors
{
    public const int Black = 0;
    public const int Magenta = 1;
    public const int DarkBlue = 2;
    public const int Purple = 3;
    public const int DarkGreen = 4;
    public const int Gray1 = 5;
    public const int MediumBlue = 6;
    public const int LightBlue = 7;
    public const int Brown = 8;
    public const int Orange = 9;
    public const int Gray2 = 10;
    public const int Pink = 11;
    public const int Green = 12;
    public const int Yellow = 13;
    public const int Aqua = 14;
    public const int White = 15;
}
