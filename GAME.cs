using Newtonsoft.Json;
using ProconFuckingSolution.MyClass;
using ProconFuckingSolution.MyDataStructure;
using ProconFuckingSolution.RevicedDataObject;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProconFuckingSolution {

	internal class GAME {

		// ************ he thong ************ //
		public static DataRequest request;

		public static BRAIN brain = new BRAIN();

		public static BeforeGame beforeGame;
		public static OnGame onGame;

		// ************ game processing ************* //

		public void StartGame() {
			string baseUrl = @"http://112.137.129.202:8004/";
			string token = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJkYXRhIjoibGVtb250cmVlIiwiaWF0IjoxNjA3NDk5NjI2LCJleHAiOjE2MDc1ODYwMjZ9.VlDH5T_n6KP2v1IBwi0QFvf9P0YQ6x3KhCz5j_M45VY";
			string matchId = "179";
			request = new DataRequest(baseUrl, token, matchId);
		}

		public void UpdateGame() {
			try {
				string jsonReviced = request.GetRequest(@"/matches");
				beforeGame = JsonConvert.DeserializeObject<List<BeforeGame>>(jsonReviced)[0];

				jsonReviced = request.GetRequest(@"/matches/" + request.GetMatchID());

				onGame = JsonConvert.DeserializeObject<OnGame>(jsonReviced);

				brain.InitBRAIN(beforeGame, onGame);
			}
			catch (Exception) {
				Console.WriteLine("Tu meny riquet");
			}
		}

		// ************** algorithms *********** //
		public void RunSolution1() {
			Solution1 solution1 = new Solution1(brain, beforeGame);
			solution1.TrySolution1();
		}
	}
}