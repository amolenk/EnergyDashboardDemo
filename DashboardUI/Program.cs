using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;

namespace DashboardUI
{
    class Program
    {
        static readonly Random random = new Random();

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            DrawDashboard();
            Write("CONNECTING...", 0, 0, ConsoleColor.Cyan, ConsoleColor.Red);

            var hubConnection = new HubConnection("http://localhost:9061/");
            IHubProxy dashboardHubProxy = hubConnection.CreateHubProxy("DashboardHub");

            dashboardHubProxy.On<DashboardStatus>("message", status =>
            {
                foreach (var chargePoint in status.ChargePoints)
                {
                    DrawChargePoint(chargePoint.Position, chargePoint.Status);
                }

                DrawMeterReading(16, 6, status.ProducedEnergy);
                DrawMeterReading(64, 10, status.ConsumedEnergy);
                DrawMeterReading(70, 25, status.ChargeEnergy);
            });

            await hubConnection.Start();

            Write("RUNNING!     ", 0, 0, ConsoleColor.Cyan, ConsoleColor.Black);

            var apiProxy = new ApiProxy();
            await apiProxy.RegisterTopologyAsync();

            //var simulationTasks = Enumerable.Range(1, 10)
            //    .Select(i => SimulateChargePointUsage(i, "chargePoint" + i, apiProxy))
            //    .ToList();

            //var timer = new Timer(2000);
            //timer.Elapsed += (s, e) =>
            //{
            //    Task.WhenAll(
            //        apiProxy.UpdateMeterReadingAsync("meter1", random.Next(200, 300)),
            //        apiProxy.UpdateMeterReadingAsync("meter2", random.Next(200, 300)),
            //        apiProxy.UpdateMeterReadingAsync("meter3", random.Next(200, 300))
            //        ).GetAwaiter().GetResult();
            //};
            //timer.Start();

            Console.ReadKey();
        }

        private static async Task SimulateChargePointUsage(int position, string chargePointId, ApiProxy proxy)
        {
            while (true)
            {
                await Delay(TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await proxy.StartSessionAsync(chargePointId);

                // Pick an amount to charge
                var currentCharge = random.Next(50, 100);
                var maxCharge = random.Next(300, 400);
                var chargeIncrement = random.Next(30, 100);

                for (int c = currentCharge; c < maxCharge; c += chargeIncrement)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));

                    await proxy.UpdateSessionAsync(chargePointId, c);
                }

                // Simulate 'charging ready'
                await proxy.UpdateSessionAsync(chargePointId, maxCharge);
                await proxy.UpdateSessionAsync(chargePointId, maxCharge);

                await Delay(TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await proxy.EndSessionAsync(chargePointId);
            }
        }

        private static Task Delay(TimeSpan min, TimeSpan max)
        {
            var milliseconds = random.Next((int)min.TotalMilliseconds, (int)max.TotalMilliseconds);

            return Task.Delay(milliseconds);
        }

        #region Drawing Methods

        private static void DrawDashboard()
        {
            // Sky
            for (var top = 0; top < 30; top++)
            {
                DrawHorizontal(0, top, 119, ConsoleColor.Cyan);
            }

            // Grass
            for (var top = 15; top < 30; top++)
            {
                DrawHorizontal(0, top, 75, ConsoleColor.Green);
            }

            // Lines
            DrawVertical(20, 15, 4, ConsoleColor.Gray);
            DrawHorizontal(21, 18, 25, ConsoleColor.Gray);
            DrawVertical(46, 14, 5, ConsoleColor.Gray);
            DrawHorizontal(46, 13, 4, ConsoleColor.Gray);
            DrawHorizontal(60, 13, 4, ConsoleColor.Gray);
            DrawVertical(64, 13, 14, ConsoleColor.Gray);
            DrawHorizontal(64, 27, 34, ConsoleColor.Gray);
            DrawVertical(98, 24, 4, ConsoleColor.Gray);
            DrawHorizontal(83, 23, 30, ConsoleColor.Blue);

            // Other objects
            DrawWindmill(17, 8);
            DrawBuilding(50, 6);
        }

        private static void DrawMeterReading(int left, int top, int reading)
        {
            DrawHorizontal(left, top, 10, ConsoleColor.Black);

            Write($"{reading} KW", left + 1, top, ConsoleColor.Black, ConsoleColor.White);
        }

        private static void DrawChargePoint(int position, string status)
        {
            var index = position - 1;
            var left = index % 2 == 0 ? 85 : 101;
            var top = 4 + (int)(4 * Math.Floor(index / 2M));
            var color = (status == "charging" ? ConsoleColor.Red
                : (status == "occupied" ? ConsoleColor.DarkMagenta : ConsoleColor.DarkGreen));

            DrawHorizontal(left, top, 10, color);
            DrawHorizontal(left, top + 1, 10, color);

            if (status == "charging" || status == "occupied")
            {
                Write("CAR", left + 1, top, color, ConsoleColor.White);
            }

            if (status == "charging")
            {
                Write("CHARGING", left + 1, top + 1, color, ConsoleColor.White);
            }
        }

        private static void DrawBuilding(int left, int top)
        {
            for (var i = top; i < top + 9; i++)
            {
                DrawHorizontal(left, i, 11, ConsoleColor.DarkGray);

                // Windows
                DrawWindow(left + 2, top + 1);
                DrawWindow(left + 7, top + 1);
                DrawWindow(left + 2, top + 4);
                DrawWindow(left + 7, top + 4);

                DrawHorizontal(left + 4, top + 7, 3, ConsoleColor.Yellow);
                DrawHorizontal(left + 4, top + 8, 3, ConsoleColor.Yellow);
            }
        }

        private static void DrawWindow(int left, int top)
        {
            DrawHorizontal(left, top, 2, ConsoleColor.Yellow);
            DrawHorizontal(left, top + 1, 2, ConsoleColor.Yellow);
        }

        private static void DrawWindmill(int left, int top)
        {
            Draw("#", left + 1, top, ConsoleColor.White);
            Draw("###", left + 6, top + 1, ConsoleColor.White);
            Draw("#", left + 2, top + 1, ConsoleColor.White);
            Draw("###", left + 3, top + 2, ConsoleColor.White);
            Draw("#", left + 1, top + 3, ConsoleColor.White);
            Draw("#", left + 3, top + 3, ConsoleColor.White);
            Draw("#", left, top + 4, ConsoleColor.White);
            Draw("#", left + 3, top + 4, ConsoleColor.White);
            Draw("#", left + 3, top + 5, ConsoleColor.White);
            Draw("#", left + 3, top + 6, ConsoleColor.White);
        }

        private static void DrawHorizontal(int left, int top, int length, ConsoleColor color)
        {
            Draw(" ".PadRight(length), left, top, color);
        }

        private static void DrawVertical(int left, int top, int length, ConsoleColor color)
        {
            for (var i = top; i < top + length; i++)
            {
                Draw(" ", left, i, color);
            }
        }

        private static void Draw(string value, int left, int top, ConsoleColor color)
        {
            Console.SetCursorPosition(left, top);
            Console.BackgroundColor = color;
            Console.Write(value);
        }

        private static void Write(string text, int left, int top, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.SetCursorPosition(left, top);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
        }

        #endregion
    }
}
