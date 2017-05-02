using System;
using System.IO;
using System.Net.Sockets;

namespace TwitchChatBot {
    class IrcClient {
        private string username;
        private string channel;
        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        public IrcClient(string ip, int port, string username, string password) {
            this.username = username;
            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());
			

			outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + this.username);
            outputStream.WriteLine("USER " + this.username + " 8 * : " + this.username);
			//outputStream.WriteLine("CAP REQ :twitch.tv/tags twitch.tv/membership");
		}
		
		public void JoinRoom(string channel) {
            this.channel = channel;
            outputStream.WriteLine("JOIN #" + this.channel);
            Console.WriteLine("JOIN #" + this.channel);
            outputStream.Flush();
        }

        public void SendIrcMessage(string message) {
			try {
				outputStream.WriteLine(message);
				outputStream.Flush();
			} catch(Exception e) {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Caught an error : " + e);
			}
		}

        public void SendChatMessage(string message) {
			SendIrcMessage(":" + username + "!" + username + "@" + username + "tmi.twitch.tv PRIVMSG #" + channel + " :" + message + " ");
		}

        public string ReadMessage() {
			string sendme = inputStream.ReadLine();
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + GetCallerFromText(sendme) + ": " + GetMessageFromText(sendme));
			//Console.WriteLine(sendme);
			if(sendme.Equals("PING: tmi.twitch.tv") || sendme.Equals("PING :tmi.twitch.tv")) SendIrcMessage("PONG :tmi.twitch.tv");
			return sendme;
		}

		public static string GetMessageFromText(string text) {
			//return text?.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2);
			if(text.GetType() == typeof(string)) if(text.Contains(":")) return text?.Substring(text.IndexOf(" :") + 2, text.Length - text.IndexOf(" :") - 2);
			return text;
		}

		public static string GetCallerFromText(string text) {
			if(text.GetType()==typeof(string)) if(text.Contains("!")) return text?.Substring(1, text.IndexOf("!") - 1);
			return text;
			//return text?.Substring(0, text.IndexOf(".tmi.twitch.tv"));
		}

		public string GetChannel() {
			return channel;
		}
	}
}
