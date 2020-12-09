using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProconFuckingSolution {

	public class DataRequest {
		private string baseUrl;
		private string token;
		private string matchId;

		public DataRequest(string baseUrl, string token, string matchId) {
			this.baseUrl = baseUrl;
			this.token = token;
			this.matchId = matchId;
		}

		public string GetRequest(string methodUrl) {
			string url = string.Format("{0}/{1}", baseUrl.TrimEnd('/'), methodUrl.TrimStart('/'));
			var client = new RestClient(url);
			client.Timeout = -1;
			var request = new RestRequest(Method.GET);
			request.AddHeader("Authorization", token);
			IRestResponse response = client.Execute(request);
			//Console.WriteLine(response.Content);
			return response.Content;
		}

		public void PostRequest(string methodUrl, string json) {
			string url = string.Format("{0}/{1}", baseUrl.TrimEnd('/'), methodUrl.TrimStart('/'));
			//Console.WriteLine(url);
			var client = new RestClient(url);
			client.Timeout = -1;
			var request = new RestRequest(Method.POST);
			request.AddHeader("Authorization", token);
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			//Console.WriteLine("du ma ket qua:   " + response.Content);
		}

		public string GetMatchID() => matchId;
	}
}