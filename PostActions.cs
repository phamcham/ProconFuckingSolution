using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution {

	public class Action {

		[JsonProperty("agentID")]
		public int AgentID { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("dx")]
		public int Dx { get; set; }

		[JsonProperty("dy")]
		public int Dy { get; set; }

		public Action(int agentID, int dx, int dy, string type) {
			AgentID = agentID;
			Dx = dx;
			Dy = dy;
			Type = type;
		}
	}

	public class PostActions {

		[JsonProperty("actions")]
		public List<Action> Actions { get; set; }

		public PostActions() {
			Actions = new List<Action>();
		}
	}
}