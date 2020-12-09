using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProconFuckingSolution {

	internal partial class Program {

		private static void Main(string[] args) {
			GAME game = new GAME();

			Console.WriteLine("Press any to start...");
			Console.ReadKey();

			game.StartGame();
			game.UpdateGame();

			while (true) {
				Console.ReadKey();
				try {
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					game.UpdateGame();
					game.RunSolution1();
					stopwatch.Stop();

					long currTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
					long timeLeft = Math.Max(GAME.beforeGame.TurnMillis - ((currTime - GAME.onGame.StartedAtUnixTime) % (GAME.beforeGame.TurnMillis + GAME.beforeGame.IntervalMillis)), 0) / 1000 + GAME.beforeGame.IntervalMillis / 1000 + 1;

					timeLeft = 1000 * timeLeft - stopwatch.ElapsedMilliseconds;
					//Console.WriteLine("Waiting for: " + timeLeft);
					//Thread.Sleep(int.Parse(timeLeft.ToString()) + 300);
				}
				catch {
					Console.WriteLine("Toang roi bu em ak");
				}
			}
		}
	}
}