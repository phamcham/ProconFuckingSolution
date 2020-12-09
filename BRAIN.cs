using ProconFuckingSolution.MyClass;
using ProconFuckingSolution.MyDataStructure;
using ProconFuckingSolution.RevicedDataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution {

	public class BRAIN {
		private Map2D<bool> isObs;
		private Map2D<int> scores;
		private Map2D<int> owns;
		private List<MyAgent> myAgents;
		private List<MyAgent> enemyAgents;

		public int size;

		public BRAIN() {
		}

		public void InitBRAIN(BeforeGame beforeGame, OnGame onGame) {
			size = int.Parse(onGame.Height);
			isObs = new Map2D<bool>(size);
			scores = new Map2D<int>(size);
			owns = new Map2D<int>(size);
			myAgents = new List<MyAgent>();
			enemyAgents = new List<MyAgent>();

			foreach (var obs in onGame.Obstacles) {
				isObs[obs.Y, obs.X] = true;
			}
			foreach (var tre in onGame.Treasure) {
				if (tre.Status == 0)
					scores[tre.Y, tre.X] = tre.Point;
			}
			for (int i = 1; i <= size; i++) {
				for (int j = 1; j <= size; j++) {
					scores[i, j] += onGame.Points[i - 1][j - 1];
					owns[i, j] = onGame.Tiled[i - 1][j - 1];
				}
			}
			foreach (var tea in onGame.Teams) {
				if (tea.TeamID == beforeGame.TeamID) {
					foreach (var age in tea.Agents) {
						myAgents.Add(new MyAgent(age.Y, age.X, age.AgentID));
					}
				}
				else {
					foreach (var age in tea.Agents) {
						enemyAgents.Add(new MyAgent(age.Y, age.X, age.AgentID));
					}
				}
			}
		}

		public List<MyAgent> GetMyAgents() => myAgents;

		public Map2D<int> GetMapScore() => scores;

		public Map2D<bool> GetMapObs() => isObs;

		public Map2D<int> GetMapOwn() => owns;

		public bool IsMyAgent(int id) {
			foreach (var age in myAgents) {
				if (id == age.id) return true;
			}
			return false;
		}

		public bool HasMyAgent(int x, int y) {
			foreach (var age in myAgents) {
				if (x == age.x && y == age.y) return true;
			}
			return false;
		}

		internal bool HasEnemyAgent(int x, int y) {
			foreach (var age in enemyAgents) {
				if (x == age.x && y == age.y) return true;
			}
			return false;
		}
	}
}