using Newtonsoft.Json;
using ProconFuckingSolution.MyClass;
using ProconFuckingSolution.MyDataStructure;
using ProconFuckingSolution.RevicedDataObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution {

	public class Solution1 {
		private BRAIN brain;
		private BeforeGame beforeGame;

		public Solution1(BRAIN brain, BeforeGame beforeGame) {
			this.brain = brain;
			this.beforeGame = beforeGame;
		}

		private List<MyAgent> myAgents;

		private Map2D<int> mapScores;
		private Map2D<bool> mapObs;
		private Map2D<int> mapOwn;
		private int size;
		private int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
		private int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };
		private List<int> plan;
		private bool startConvexHull = false;
		private int deepConvexHull = 4; // cang nho cang de bao
		private int turnStartConvexHull = 5;

		private int CalcConvexHull(Map2D<int> mapOwnClone) {
			var vis = new Map2D<bool>(size);
			for (int i = 1; i <= size; i++) {
				var pois = new List<Point>() { new Point(1, i), new Point(size, i),
											new Point(i, 1), new Point(i, size) };
				foreach (var p in pois) {
					int x = p.X, y = p.Y;
					if (vis[x, y] || IsOutOfMap(x, y)
						|| mapOwnClone[x, y] == beforeGame.TeamID) continue;
					vis[x, y] = true;
					var q = new Queue<Point>();
					q.Enqueue(p);
					while (q.Count > 0) {
						var t = q.Dequeue();
						foreach (var j in new int[] { 0, 2, 4, 6 }) {
							int nx = t.X + dx[j], ny = t.Y + dy[j];
							if (vis[nx, ny] || IsOutOfMap(nx, ny)
									|| mapOwnClone[nx, ny] == beforeGame.TeamID) continue;
							vis[nx, ny] = true;
							q.Enqueue(new Point(nx, ny));
						}
					}
				}
			}
			int res = 0;
			for (int i = 1; i <= size; i++) {
				for (int j = 1; j <= size; j++) {
					if (vis[i, j] == false && mapOwnClone[i, j] != beforeGame.TeamID)
						res += Math.Abs(mapScores[i, j]);
				}
			}
			return res;
		}

		private bool IsOutOfMap(int x, int y) {
			return x <= 0 || y <= 0 || x > size || y > size;
		}

		private int TryGo(int x, int y, int deep, List<Point> vis, int curProfit) {
			if (deep < 0 || IsOutOfMap(x, y) || vis.Contains(new Point(x, y)) || mapObs[x, y]
					|| brain.HasMyAgent(x, y) || brain.HasEnemyAgent(x, y)) return curProfit;
			// he so tuy chinh bao
			if (startConvexHull && (deep == deepConvexHull || GAME.beforeGame.Turns - GAME.onGame.Turn <= deepConvexHull)) {
				var mapOwnClone = mapOwn.Clone();
				foreach (var poi in vis) {
					mapOwnClone[poi.X, poi.Y] = beforeGame.TeamID;
				}
				curProfit += CalcConvexHull(mapOwnClone);
			}
			vis.Add(new Point(x, y));
			int maxProfit = 0;
			// nếu đi vào ô của đối thủ thì mất 1 turn
			if (mapOwn[x, y] > 0 && mapOwn[x, y] != GAME.beforeGame.TeamID) deep--;
			for (int i = 0; i < 8; i++) {
				int cur = TryGo(x + dx[i], y + dy[i], deep - 1, new List<Point>(vis), curProfit + mapScores[x, y]);
				maxProfit = Math.Max(maxProfit, cur);
			}
			vis.Remove(new Point(x, y));
			return maxProfit;
		}

		public void TrySolution1() {
			if (GAME.beforeGame.Turns - GAME.onGame.Turn <= turnStartConvexHull) {
				startConvexHull = true;
				deepConvexHull = GAME.brain.size / 5;
				Console.WriteLine("Start convex hull with: deep = " + deepConvexHull);
			}

			myAgents = brain.GetMyAgents();
			mapScores = brain.GetMapScore();
			mapObs = brain.GetMapObs();
			mapOwn = brain.GetMapOwn();
			size = brain.size;

			// sua score nếu đi rồi cho nó = 0
			for (int i = 1; i <= size; i++) {
				for (int j = 0; j <= size; j++) {
					if (mapOwn[i, j] == beforeGame.TeamID) mapScores[i, j] = 0;
				}
			}

			for (int i = 1; i <= size; i++) {
				for (int j = 1; j <= size; j++) {
					Console.Write("{0,3}", mapScores[i, j]);
				}
				Console.WriteLine();
			}

			plan = new List<int>();

			foreach (var age in myAgents) {
				// DFS
				int maxProfit = -1000;
				List<int> res = new List<int>();
				for (int i = 0; i < 8; i++) {
					if (IsOutOfMap(age.x + dx[i], age.y + dy[i])) continue;
					if (mapObs[age.x + dx[i], age.y + dy[i]]) continue;
					int curProfit = -10;
					// thử độ sâu
					for (int findRange = 1; findRange <= Math.Min(6, GAME.beforeGame.Turns - GAME.onGame.Turn); findRange++)
						curProfit = Math.Max(curProfit, TryGo(age.x + dx[i], age.y + dy[i], findRange, new List<Point>(), 0));
					if (maxProfit < curProfit) {
						maxProfit = curProfit;
						res = new List<int>() { i };
					}
					else if (maxProfit == curProfit) {
						res.Add(i);
					}
				}

				int bestChoice = res[0];
				if (maxProfit <= -10) {
					// điểm nhận được <= -10 thì cũng chả có lợi lộc j, đứng cmn im
					bestChoice = -1;
				}
				else {
					int maxIfChoose = -1000;
					int totalStep = 6;
					// nếu có nhiều hướng đi mò xem hướng nào nhiều điểm nhất và có số bước ít nhất
					foreach (var j in res) {
						int step = 0;
						int curProfit = 0;
						for (int findRange = 1; findRange <= Math.Min(5, GAME.beforeGame.Turns - GAME.onGame.Turn); findRange++) {
							int tryFire = TryGo(age.x + dx[j], age.y + dy[j], findRange, new List<Point>(), 0);
							if (tryFire > curProfit) {
								curProfit = tryFire;
								step = findRange;
							}
						}
						if (maxIfChoose < curProfit) {
							maxIfChoose = curProfit;
							bestChoice = j;
							totalStep = step;
						}
						else if (maxIfChoose == curProfit && totalStep > step) {
							totalStep = step;
							bestChoice = j;
						}
					}
					Console.WriteLine(string.Format("[{0},{1}] step: {2} profit: {3}", age.x, age.y, totalStep + 1, maxProfit));

					// nếu bước tiếp theo nhận đc 0 điểm
					if (mapScores[age.x + dx[bestChoice], age.y + dy[bestChoice]] == 0) {
						// đi sang 2 bên xem thế nào
						int nx = age.x + dx[(bestChoice + 1) % 8];
						int ny = age.y + dy[(bestChoice + 1) % 8];
						if (!IsOutOfMap(nx, ny) && !mapObs[nx, ny] && !GAME.brain.HasEnemyAgent(nx, ny) && mapScores[nx, ny] > 0) {
							bestChoice = (bestChoice + 1) % 8;
							maxIfChoose = mapScores[nx, ny];
						}
						nx = age.x + dx[(bestChoice + 7) % 8];
						ny = age.y + dy[(bestChoice + 7) % 8];
						if (!IsOutOfMap(nx, ny) && !mapObs[nx, ny] && !GAME.brain.HasEnemyAgent(nx, ny) && mapScores[nx, ny] > 0) {
							bestChoice = (bestChoice + 7) % 8;
							if (mapScores[nx, ny] < maxIfChoose)
								bestChoice = (bestChoice + 1) % 8;
						}
					}
				}
				plan.Add(bestChoice);
				if (bestChoice >= 0) {
					mapObs[age.x + dx[bestChoice], age.y + dy[bestChoice]] = true;
				}
			}
			// post action
			ExcutePlan(plan);
		}

		private void ExcutePlan(List<int> plan) {
			PostActions postActions = new PostActions();
			for (int i = 0; i < myAgents.Count; i++) {
				var type = "move";
				if (mapOwn[myAgents[i].x + dx[plan[i]], myAgents[i].y + dy[plan[i]]] > 0 &&
					mapOwn[myAgents[i].x + dx[plan[i]], myAgents[i].y + dy[plan[i]]] != beforeGame.TeamID)
					type = "remove";
				if (plan[i] == -1) {
					postActions.Actions.Add(new Action(myAgents[i].id, 0, 0, "stay"));
					Console.WriteLine("[{0}, {1}] stay", myAgents[i].x, myAgents[i].y);
				}
				else {
					postActions.Actions.Add(new Action(myAgents[i].id, dy[plan[i]], dx[plan[i]], type));
					Console.WriteLine("[{0}, {1}] {2} [{3}, {4}]", myAgents[i].x, myAgents[i].y, type, myAgents[i].x + dx[plan[i]], myAgents[i].y + dy[plan[i]]);
				}
			}
			string json = JsonConvert.SerializeObject(postActions);
			GAME.request.PostRequest(@"/matches/" + GAME.request.GetMatchID() + @"/action", json);
			//Console.WriteLine(json);
		}
	}
}