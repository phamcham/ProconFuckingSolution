using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution.RevicedDataObject {

	public class Agent {

		[JsonProperty("x")]
		public int X { get; set; }

		[JsonProperty("y")]
		public int Y { get; set; }

		[JsonProperty("agentID")]
		public int AgentID { get; set; }
	}

	public class Team {

		[JsonProperty("agents")]
		public List<Agent> Agents { get; set; }

		[JsonProperty("teamID")]
		public int TeamID { get; set; }

		[JsonProperty("areaPoint")]
		public int AreaPoint { get; set; }

		[JsonProperty("tilePoint")]
		public int TilePoint { get; set; }
	}

	public class Treasure {

		[JsonProperty("x")]
		public int X { get; set; }

		[JsonProperty("y")]
		public int Y { get; set; }

		[JsonProperty("point")]
		public int Point { get; set; }

		[JsonProperty("status")]
		public int Status { get; set; }
	}

	public class Obstacle {

		[JsonProperty("x")]
		public int X { get; set; }

		[JsonProperty("y")]
		public int Y { get; set; }
	}

	public class OnGame {

		[JsonProperty("turn")]
		public int Turn { get; set; }

		[JsonProperty("teams")]
		public List<Team> Teams { get; set; }

		[JsonProperty("tiled")]
		public List<List<int>> Tiled { get; set; }

		[JsonProperty("width")]
		public string Width { get; set; }

		[JsonProperty("height")]
		public string Height { get; set; }

		[JsonProperty("points")]
		public List<List<int>> Points { get; set; }

		[JsonProperty("actions")]
		public List<object> Actions { get; set; }

		[JsonProperty("treasure")]
		public List<Treasure> Treasure { get; set; }

		[JsonProperty("obstacles")]
		public List<Obstacle> Obstacles { get; set; }

		[JsonProperty("startedAtUnixTime")]
		public long StartedAtUnixTime { get; set; }
	}
}