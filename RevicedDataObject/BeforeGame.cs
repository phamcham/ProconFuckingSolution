using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution.RevicedDataObject {

	public class BeforeGame {

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("intervalMillis")]
		public int IntervalMillis { get; set; }

		[JsonProperty("matchTo")]
		public string MatchTo { get; set; }

		[JsonProperty("teamID")]
		public int TeamID { get; set; }

		[JsonProperty("turnMillis")]
		public int TurnMillis { get; set; }

		[JsonProperty("turns")]
		public int Turns { get; set; }
	}
}