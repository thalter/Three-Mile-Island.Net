using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreeMileIsland
{
    class Program
    {
        // Game variables
        static int SCNT, DCNT, CNT, OTMP, TEMP;
        static int TMP0, TMP1, TMP2, TMP3, TMP4;
        static bool VFLG, UFLG, TFLG, FFLG, EFLG;
        static int UC1, UC2, UC3, UC4, UC5, UC6, UC7, UC8;
        static int EOUT, COST, MAIN, DMND, ACTP, MAXP;
        static int AIR, FCNT, TCNT;
        
        // Arrays
        static int[] VC = new int[20];
        static int[] VA = new int[20];
        static int[] VP = new int[20];
        static int[] PI = new int[26];
        static int[] UC = new int[25];
        static int[] PU = new int[25];
        static int[] TC = new int[5];
        static int[] TU = new int[5];
        static int[] SC = new int[4];
        static int[] FI = new int[4];
        static int[] SL = new int[4];
        static int[] GC = new int[12];
        static int[] BU = new int[12];
        static int[] BO = new int[12];
        static int[] SP = new int[4];
        static int[] FR = new int[19];
        static int[] CR = new int[19];
        static int[] CRC = new int[19];
        static int[] CCR = new int[3];
        static int[] FCOL = new int[5];
        static int[] CX = new int[19];
        static int[] CY = new int[19];
        
        static string CMMD = "";
        static string LET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string LN = "------------------------------";
        static string SP_STR = "                              ";
        
        // Game state
        static int DYN = -1;
        static int KB;
        static int LAB = 0;
        static int CMMD_INDEX = 0;
        static int CCNT = 0;
        static int CHAR = 1;
        static bool SYM = true;
        static int CP, CP0;
        static bool LEAK = false;
        static int LK0;
        static bool FRDMG = false;
        static int RPOS = 9;
        static int DAY = 1;
        static int SDN = 0, ENN = 0;
        static bool GMAIN = false;
        static int NDG = 0;
        
        // Constants
        static int VF0, VF1, VA0, VA1, VR0, VR1;
        static int UF0, UF1, UA0, UA1, UR0, UR1;
        static int TF0, TF1, TA0, TA1, TR0, TR1;
        static int SR0, SR1, DC0, DC1, DM0, DM1;
        static int FF0, VM0, VM1, UM0, UM1, TM0, TM1;
        static int GC0, GC1, GM0, GM1;
        static int LOSS1, LOSS2, RAD1, RD1, RD5;
        static int TMPMD;
        
        static Random rand = new Random();
        
        static void Main(string[] args)
        {
            Initialize();
            ShowMainMenu();
            GameLoop();
        }
        
        static void Initialize()
        {
            // Initialize constants
            VF0 = 250; VF1 = 100;
            VA0 = 5; VA1 = 5;
            VR0 = 25; VR1 = 25;
            UF0 = 500; UF1 = 100;
            UA0 = 5; UA1 = 5;
            UR0 = 50; UR1 = 25;
            TF0 = 2760; TF1 = 120;
            TA0 = 10; TA1 = 10;
            TR0 = 100; TR1 = 100;
            SR0 = 25; SR1 = 25;
            DC0 = 50; DC1 = 15;
            DM0 = 75; DM1 = 25;
            FF0 = 120; FCNT = FF0;
            VM0 = 1; VM1 = 1;
            UM0 = 2; UM1 = 2;
            TM0 = 5; TM1 = 5;
            GC0 = 750; GC1 = 50;
            GM0 = 10; GM1 = 10;
            
            MAIN = 0; COST = 200;
            MAXP = 0; ACTP = 0;
            EOUT = 0; DMND = 100;
            LOSS1 = -200; LOSS2 = -500;
            CP0 = 50; CP = CP0;
            LK0 = 100;
            RAD1 = 50;
            RD5 = 25; RD1 = 75;
            TMPMD = 2500;
            TMP0 = 0;
            TMP1 = 400;
            TMP2 = 500;
            TMP3 = 600;
            TMP4 = 750;
            TEMP = 0;
            CNT = 0; SCNT = 0;
            DCNT = DC0 + RND(DC1) * (RND(3) - 1);
            DAY = 1;
            
            // Initialize arrays
            InitializeArrays();
            InitializePositions();
            
            Console.Clear();
            Console.WriteLine("WELCOME TO ...");
            Console.WriteLine();
            Console.WriteLine("    THREE MILE ISLAND NUCLEAR FACILITY");
            Console.WriteLine();
            Console.WriteLine("THE REACTOR IS IN A COLD SHUT-DOWN");
            Console.WriteLine();
            Console.WriteLine("STATE WITH ALL SYSTEMS IN-ACTIVE.");
            Console.WriteLine();
            Console.WriteLine("THE DATE IS DECEMBER 30,1978, AND THE");
            Console.WriteLine();
            Console.WriteLine("REACTOR HAS BEEN CERTIFIED OPERATIONAL.");
            Console.WriteLine();
            Console.WriteLine("INITIALIZE ACCORDING TO OFFICIAL MUSE");
            Console.WriteLine();
            Console.WriteLine("GUIDELINES (Y OR N) ?");
            
            var key = Console.ReadKey(true);
            if (key.KeyChar == 'Y' || key.KeyChar == 'y')
            {
                // Auto-initialize system
                AutoInitialize();
            }
        }
        
        static void InitializeArrays()
        {
            // Initialize valves
            VP[1] = 0; VP[2] = 1;
            VP[3] = 2; VP[4] = 11;
            VP[5] = 5; VP[6] = 6;
            VP[7] = 7; VP[8] = 12;
            VP[9] = 14; VP[10] = 15;
            VP[11] = 21; VP[12] = 22;
            VP[13] = 24; VP[14] = 24;
            VP[15] = 24; VP[16] = 19;
            VP[17] = 19; VP[18] = 19;
            VP[19] = 23;
            
            // Initialize filter colors
            FCOL[1] = 10; FCOL[2] = 7;
            FCOL[3] = 6; FCOL[4] = 2;
            
            // Initialize pipes
            for (int p = 1; p <= 25; p++)
                PI[p] = 10;
                
            // Initialize pumps
            for (int u = 1; u <= 24; u++)
            {
                UC[u] = RND(UF1) + UF0;
                PU[u] = 1;
            }
            
            // Initialize valves
            for (int v = 1; v <= 19; v++)
            {
                VC[v] = RND(VF1) + VF0;
                VA[v] = 1;
            }
            
            // Initialize buildings
            for (int b = 1; b <= 11; b++)
                BU[b] = 0;
                
            // Initialize filters
            for (int f = 1; f <= 3; f++)
            {
                FI[f] = 10;
                SL[f] = 0;
                SC[f] = RND(SR1) + SR0;
            }
            
            // Initialize turbines
            for (int t = 1; t <= 4; t++)
            {
                TC[t] = RND(TF1) + TF0;
                TU[t] = 10;
            }
            
            // Initialize control rods
            for (int r = 1; r <= 18; r++)
            {
                CR[r] = 0;
                CRC[r] = 12;
                FR[r] = 10;
            }
            
            // Initialize gauges
            for (int g = 1; g <= 11; g++)
                GC[g] = GC0 + RND(GC1);
                
            CCR[1] = 0; CCR[2] = 0;
            BU[3] = 6;
            BU[2] = 12;
            GC[11] = GC0;
        }
        
        static void InitializePositions()
        {
            // Control rod X positions
            CX[1] = 10; CX[2] = 12; CX[3] = 11;
            CX[4] = 10; CX[5] = 12; CX[6] = 11;
            CX[7] = 10; CX[8] = 12; CX[9] = 11;
            CX[10] = 14; CX[11] = 13; CX[12] = 15;
            CX[13] = 14; CX[14] = 13; CX[15] = 15;
            CX[16] = 14; CX[17] = 13; CX[18] = 15;
            
            // Control rod Y positions
            CY[1] = 19; CY[2] = 19; CY[3] = 20;
            CY[4] = 21; CY[5] = 21; CY[6] = 22;
            CY[7] = 23; CY[8] = 23; CY[9] = 24;
            CY[10] = 19; CY[11] = 20; CY[12] = 20;
            CY[13] = 21; CY[14] = 22; CY[15] = 22;
            CY[16] = 23; CY[17] = 24; CY[18] = 24;
        }
        
        static void AutoInitialize()
        {
            // Simulates the auto-initialization sequence
            Console.WriteLine("\nInitializing reactor systems...");
            Thread.Sleep(1000);
        }
        
        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("            THREE MILE ISLAND");
            Console.WriteLine();
            Console.WriteLine("                MAIN MENU");
            Console.WriteLine();
            Console.WriteLine("     0  CONTAINMENT");
            Console.WriteLine();
            Console.WriteLine("     1  TURBINE, FILTER, CONDENSER");
            Console.WriteLine();
            Console.WriteLine("     2  REACTOR CORE");
            Console.WriteLine();
            Console.WriteLine("     3  PUMP HOUSE");
            Console.WriteLine();
            Console.WriteLine("     4  MAINTENANCE SCHEDULE");
            Console.WriteLine();
            Console.WriteLine("     5  COST ANALYSIS");
            Console.WriteLine();
            Console.WriteLine("     6  OPERATIONAL STATUS");
            Console.WriteLine();
            Console.WriteLine("     7  SAVE / RESET STATE");
            Console.WriteLine();
        }
        
        static void GameLoop()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    ShowMainMenu();
                    continue;
                }
                
                if (char.IsDigit(key.KeyChar))
                {
                    int choice = key.KeyChar - '0';
                    if (choice >= 0 && choice <= 7)
                    {
                        DYN = choice;
                        HandleMenuChoice(choice);
                    }
                }
                
                // Update simulation
                UpdateSimulation();
            }
        }
        
        static void HandleMenuChoice(int choice)
        {
            Console.Clear();
            switch (choice)
            {
                case 0:
                    ShowContainment();
                    break;
                case 1:
                    ShowTurbineFilterCondenser();
                    break;
                case 2:
                    ShowReactorCore();
                    break;
                case 3:
                    ShowPumpHouse();
                    break;
                case 4:
                    ShowMaintenanceSchedule();
                    break;
                case 5:
                    ShowCostAnalysis();
                    break;
                case 6:
                    ShowOperationalStatus();
                    break;
                case 7:
                    SaveResetState();
                    break;
            }
        }
        
        static void ShowContainment()
        {
            Console.WriteLine("CONTAINMENT VIEW");
            Console.WriteLine();
            Console.WriteLine("Core Temperature:    " + TEMP + " DEG");
            Console.WriteLine("Containment Pressure: " + BU[1] + "00 PSI");
            Console.WriteLine("Containment Water:    " + BU[7] + ",000 GAL");
            Console.WriteLine("PCS Pressure:         " + (BU[2] * 100 + 1200) + " PSI");
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowTurbineFilterCondenser()
        {
            Console.WriteLine("TURBINE, FILTER, CONDENSER VIEW");
            Console.WriteLine();
            Console.WriteLine("Turbines Operating:  " + TCNT + " of 4");
            Console.WriteLine("Filters Active:      " + FC + " of 3");
            Console.WriteLine("Condenser Status:    " + (BU[6] > 0 ? "ACTIVE" : "OFF"));
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowReactorCore()
        {
            Console.WriteLine("REACTOR CORE VIEW");
            Console.WriteLine();
            Console.WriteLine("Core Temperature:     " + TEMP + " DEG");
            Console.WriteLine("Control Rods:         " + TMP0 + " DEG");
            Console.WriteLine("Pumps Required:       " + CNT);
            Console.WriteLine();
            Console.WriteLine("Use +/- to adjust control rods");
            Console.WriteLine("Press ESC to return to main menu");
            
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) break;
                
                if (key.KeyChar == '+' || key.KeyChar == '=')
                {
                    AdjustControlRods(1);
                }
                else if (key.KeyChar == '-' || key.KeyChar == '_')
                {
                    AdjustControlRods(-1);
                }
                
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowPumpHouse()
        {
            Console.WriteLine("PUMP HOUSE VIEW");
            Console.WriteLine();
            Console.WriteLine("Pump House Water:    " + BU[10] + ",000 GAL");
            Console.WriteLine("Radiation Level:     " + (BU[11] / 100.0).ToString("F2") + " MREMS/HR");
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowMaintenanceSchedule()
        {
            Console.WriteLine("MAINTENANCE SCHEDULE");
            Console.WriteLine();
            Console.WriteLine("PUMPS:");
            for (int u = 1; u <= 6; u++)
            {
                Console.WriteLine($"  Pump {LET[u-1]}: " + GetComponentStatus(PU[u], UC[u], UF0 + UF1));
            }
            Console.WriteLine();
            Console.WriteLine("VALVES:");
            for (int v = 1; v <= 6; v++)
            {
                Console.WriteLine($"  Valve {LET[v-1]}: " + GetComponentStatus(VA[v], VC[v], VF0 + VF1));
            }
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowCostAnalysis()
        {
            Console.Clear();
            Console.WriteLine("               COST ANALYSIS");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("OPERATING COST:   $ " + COST + ",000");
            Console.WriteLine();
            Console.WriteLine("MAINTENANCE COST: $ " + MAIN + (MAIN > 0 ? ",000" : ""));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("ELECTRIC DEMAND: " + DMND + " MEGAWATTS");
            Console.WriteLine();
            Console.WriteLine("ELECTRIC OUTPUT: " + EOUT + " MEGAWATTS");
            Console.WriteLine();
            Console.WriteLine("(ELECTRIC DEMAND CHANGES AT " + FormatTime(SCNT + DCNT) + " )");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("PROJECTED PROFIT: $ " + MAXP + (MAXP > 0 ? ",000" : ""));
            Console.WriteLine();
            Console.WriteLine("ACTUAL PROFIT:    " + (ACTP < 0 ? "< " : "") + "$ " + Math.Abs(ACTP) + 
                            (ACTP != 0 ? ",000" : "") + (ACTP < 0 ? " > <LOSS>" : ""));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void ShowOperationalStatus()
        {
            Console.Clear();
            Console.WriteLine("            OPERATIONAL STATUS");
            Console.WriteLine();
            Console.WriteLine("CORE TEMP:      " + (GC[1] > 0 || RND(2) == 0 ? TEMP + " DEG" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("CTRL RODS:      " + (GC[2] > 0 || RND(2) == 0 ? TMP0 + " DEG" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("PCS PRES:       " + (GC[3] > 0 || RND(2) == 0 ? (BU[2] * 100 + 1200) + " PSI" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("PMPS REQ'D:     " + (GC[4] > 0 || RND(2) == 0 ? CNT.ToString() : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("CNTMT PRES:     " + (GC[5] > 0 || RND(2) == 0 ? BU[1] + "00 PSI" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("CNTMT WTR:      " + (GC[6] > 0 || RND(2) == 0 ? BU[7] + ",000 GAL" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("PMP HSE WTR:    " + (GC[7] > 0 || RND(2) == 0 ? BU[10] + ",000 GAL" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("PMP HSE RAD:    " + (GC[8] > 0 || RND(2) == 0 ? (BU[11] / 100.0).ToString("F2") + " MREMS/HR" : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("FLUSH TIME:     " + (GC[9] > 0 || RND(2) == 0 ? FormatFlushTime() : RND(100).ToString()));
            Console.WriteLine();
            Console.WriteLine("PRSZER WTR:     " + (GC[10] > 0 || RND(2) == 0 ? BU[3] + ",000 GAL" : RND(100).ToString()));
            Console.WriteLine();
            
            // Display warnings
            if (CP == 32767) Console.WriteLine("                              SEALED");
            if (TEMP > TMP3) Console.WriteLine("                              TEMP");
            if (FRDMG) Console.WriteLine("                              FR DAMAGE");
            if (TMP0 == 0) Console.WriteLine("                              SCRAM");
            if (UC1 > 0) Console.WriteLine("                              ECCS" + (UC3 > 0 ? "     ESCS" : ""));
            if (BU[11] > 0) Console.WriteLine("                              RADLEAK");
            if (FC == 0) Console.WriteLine("                              FLTR" + (AIR > 0 ? "     AIR" : ""));
            if (BU[6] > 0) Console.WriteLine("                              CNDSER");
            if (BU[4] > 0) Console.WriteLine("                              STMER");
            if (LEAK) Console.WriteLine("                              PCSLEAK");
            if (EOUT > 0 && EOUT < DMND) Console.WriteLine("                              BROWNOUT");
            if (EOUT == 0) Console.WriteLine("                              BLACKOUT");
            
            Console.WriteLine();
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                UpdateSimulation();
                Thread.Sleep(100);
            }
        }
        
        static void SaveResetState()
        {
            Console.WriteLine("SAVE / RESET STATE");
            Console.WriteLine();
            Console.WriteLine("This feature is not yet implemented.");
            Console.WriteLine("Press ESC to return to main menu");
            
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }
        }
        
        static void AdjustControlRods(int direction)
        {
            int r = RPOS;
            if (direction > 0)
            {
                TMP0 += (CR[r] < 33) ? 1 : 0;
                int r1 = (r - 1) / 9 + 1;
                CCR[r1] += (CR[r] < 33) ? 1 : 0;
                CR[r] += (CR[r] < 33) ? 1 : 0;
                if (CRC[r] == 12) CRC[r] = 9;
                if (CR[r] > 14) CRC[r] = 1;
            }
            else
            {
                TMP0 -= (CR[r] > 0) ? 1 : 0;
                int r1 = (r - 1) / 9 + 1;
                CCR[r1] -= (CR[r] > 0) ? 1 : 0;
                CR[r] -= (CR[r] > 0) ? 1 : 0;
                if (CR[r] < 15) CRC[r] = 9;
                if (CR[r] < 4) CRC[r] = 12;
            }
        }
        
        static void UpdateSimulation()
        {
            // Update system counters
            SCNT++;
            CNT = (TU[1] == 13 ? 1 : 0) + (TU[2] == 13 ? 1 : 0) + 
                  (TU[3] == 13 ? 1 : 0) + (TU[4] == 13 ? 1 : 0);
            
            // Update temperature
            OTMP = TEMP;
            if (TEMP > 0)
            {
                TEMP = TEMP + ((BU[2] < CNT) ? CNT : 0) + 
                       ((PI[3] == 10 || BU[4] == 14) ? CNT : 0);
                TEMP = TEMP + Math.Sign(CNT - ((PI[11] != 10 ? UC1 : 0) + 
                       (PI[8] != 10 ? UC2 : 0))) * CNT;
                TEMP = TEMP - ((BU[4] < 14 && BU[6] < 12 && PI[3] != 10) ? 1 : 0);
            }
            
            // Check for day rollover
            if (SCNT >= 1440)
            {
                SCNT = 0;
                DAY++;
                COST += RND(10) + 1;
                MAIN = 0;
            }
            
            // Check for demand update
            if (DCNT == 0)
            {
                DCNT = DC0 + RND(DC1) * (RND(3) - 1);
                int c = DM0 + RND(DM1);
                DMND += (SCNT < 720) ? c : -c / 2;
                if (DMND > 1000) DMND = 1000;
                if (DMND < 100) DMND = 100;
            }
            else
            {
                DCNT--;
            }
            
            // Update building units (copy from BU to BO for comparison)
            Array.Copy(BU, BO, BU.Length);
            
            // Check critical conditions
            CheckCriticalConditions();
        }
        
        static void CheckCriticalConditions()
        {
            // Check for excessive losses
            if (ACTP < LOSS2)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("          OFFICIAL NOTIFICATION");
                Console.WriteLine();
                Console.WriteLine("FROM: PUBLIC UTILITIES COMMISSION");
                Console.WriteLine();
                Console.WriteLine("TO  : MANAGER, THREE MILE ISLAND");
                Console.WriteLine();
                Console.WriteLine("RE  : EXCESSIVE LOSSES FROM OPERATIONS");
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
                Console.ReadKey();
                Environment.Exit(0);
            }
            
            // Check for meltdown
            if (TEMP > TMPMD)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("               EMERGENCY NOTICE");
                Console.WriteLine();
                Console.WriteLine("               MELT-DOWN");
                Console.WriteLine();
                Console.WriteLine("The reactor core has exceeded maximum temperature!");
                Console.WriteLine("Catastrophic failure imminent!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        
        static string GetComponentStatus(int status, int timer, int maxTimer)
        {
            if (timer >= maxTimer)
                return "FAILED";
            else if (status == 1)
                return "OFF     " + FormatTime(SCNT + timer);
            else if (status == 12)
                return "ON      " + FormatTime(SCNT + timer / CNT);
            else
                return "REPAIR  " + FormatTime(SCNT + timer);
        }
        
        static string FormatTime(int minutes)
        {
            int day = DAY;
            int c = minutes;
            
            while (c >= 1440)
            {
                day++;
                c -= 1440;
            }
            
            int hours = c / 60;
            int mins = c % 60;
            
            return $"{day}/{hours:D2}:{mins:D2}";
        }
        
        static string FormatFlushTime()
        {
            int c = FF0 - FCNT;
            if (c >= 0)
            {
                int hours = c / 60;
                int mins = c % 60;
                return $"{hours:D2}:{mins:D2}";
            }
            return "EXPIRED";
        }
        
        static int RND(int max)
        {
            return rand.Next(max);
        }
    }
}
