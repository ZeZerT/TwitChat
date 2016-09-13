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
        }
		
		public void joinRoom(string channel) {
            this.channel = channel;
            outputStream.WriteLine("JOIN #" + this.channel);
            Console.WriteLine("JOIN #" + this.channel);
            outputStream.Flush();
        }

        public void sendIrcMessage(string message) {
			try {
				outputStream.WriteLine(message);
				outputStream.Flush();
			} catch(IOException e) {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Caught an error : " + e);
				Console.WriteLine("Trying to reconnect..");
				joinRoom(channel);
			}
		}

        public void sendChatMessage(string message) {
			sendIrcMessage(":" + username + "!" + username + "@" + username + "tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
		}

        public string readMessage() {
            return inputStream.ReadLine();
		}
    }
}
