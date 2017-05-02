using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace TwitchChatBotV3 {
	class SpecialAnswer : IRCMessage {
		new private int type;	public Int32 Type1			{ get { return type; }			set { this.type=value;			} }
		
		// Types

		public const int NONE                       = 30;
		public const int WHEREISMOM                 = 31;
		public const int WHEREISDAD                 = 32;
		public const int RANDOMIZE_CHAR             = 33;
		public const int RANDOMIZE_WORD             = 34;
		public const int CODEX                      = 35;
		public const int KILL                       = 36;
		public const int SPANK                      = 37;
		public const int DEATH                      = 38;
		public const int CASCADE                    = 39;
		public const int CHAIN                      = 40;
		public const int CANCER                     = 41;
		public const int CHROMOROME                 = 42;
		public const int GACHI                      = 43;
		public const int AFK                        = 44;
		public const int WEEB                       = 45;
		public const int GYM                        = 46;
        public const int TRIHARD                    = 47;
        public const int KKONA                      = 48;

        public SpecialAnswer(int type) : base(null, TwitchClient.botName, TwitchClient.channel) {
			Type = type;
		}

		public void getAnswer(string text) {
			switch(type) {
				case WHEREISMOM:
					Message = Whereismom();
					break;
				case WHEREISDAD:
					Message = Whereisdad();
					break;
				case RANDOMIZE_CHAR:
					Message = ShuffleChar(text);
					break;
				case RANDOMIZE_WORD:
					Message = ShuffleWord(text);
					break;
				case CODEX:
					Message = readCodex(text);
					break;
				case KILL:
					Message = kill(text);
					break;
				case SPANK:
					Message = spank(text);
					break;
				case DEATH:
					Message = death(text);
					break;
				case CANCER:
					Message = cancer(text);
					break;
				case CHROMOROME:
					Message = chromosome(text);
					break;
				case GACHI:
					Message = gachi();
					break;
				case WEEB:
					Message = weeb(text);
					break;
				case AFK:
					Message = answeringMachine(text);
					break;
				case GYM:
					Message = gym(text);
					break;
                case TRIHARD:
                    TextAdmin = TextPleb = trihard(text);
                    break;
                case KKONA:
                    TextAdmin = TextPleb = kkona(text);
                    break;
                default:
					Message = null;
					break;
			}
		}

		public Boolean canResend() {
			return true;
		}


		private string Whereismom() {
			return new string[] {
					"I don't know where is eloise_ailv, sorry.",
					"Who ? forsenE",
					"!whereismom forsenPuke2",
					"She is screaming in another stream for whatever reason forsenLewd"
				}[new Random().Next(0, 4)];
		}

		private string Whereisdad() {
			return new string[] {
					"Nani killed him BibleThump",
					"Dad went to get a pack of cigarettes, he'll soon be back... FeelsBadMan",
					"He and Nani are making a pleb right now forsenE",
					"He is now the ghost of the volume control. RIP SMSkull",
					"!whereisdad forsenPuke",
					"He's live FailFish If you don't see him, refresh."
				}[new Random().Next(0, 6)];
		}

		private string ShuffleChar(string unshuffled) {
			Random x = new Random();
			unshuffled = unshuffled.Substring(unshuffled.IndexOf("?")+1, unshuffled.Length - unshuffled.IndexOf("?")-1);
			return new string(unshuffled.OrderBy(r => x.Next()).ToArray());
		}

		private string ShuffleWord(string unshuffled) {
			Random x = new Random();
			unshuffled = unshuffled.Substring(unshuffled.IndexOf("?")+1, unshuffled.Length - unshuffled.IndexOf("?")-1);
			return String.Join(" ", unshuffled.Split(' ').OrderBy(r => x.Next()).ToArray());
		}

		private string readCodex(string startsWith = null) {
			Dictionary<int, string> answers = new Dictionary<int, string>();
			int indice = 0;
			try {
				// "using" allows to dispose automatically of the StreamReader when it ends.
				using(StreamReader sr = new StreamReader("Codex.txt")) {
					if(startsWith.Contains("(") && startsWith.Contains(")")) startsWith = startsWith.ToUpper().Substring(startsWith.IndexOf("(")+1, startsWith.Length - startsWith.IndexOf(")"));
					if(startsWith.Length==1 && startsWith.Any(x => char.IsLetter(x))) {
						while(sr.Peek() >= 0) {
							String line = sr.ReadLine();
							if(line.StartsWith(startsWith)) answers.Add(indice++, line);
						}
					} else {
						while(sr.Peek() >= 0) {
							answers.Add(indice++, sr.ReadLine());
						}
					}
				}
				int y = new Random().Next(0, indice);
				//Console.Write("indice:"+y+"\ttext:"+answers[y]);
				return answers[y];
			} catch(Exception e) {
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
				return "I had a problem, but my creator ZeZerT isn't responsible at all for he is mighty and skilled MrDestructoid";
			}
		}

		static private string spank(string text) {
			string message = IRCMessage.getMessageFromText(text);
			string caller = IRCMessage.getCallerFromText(text);
			if(message.StartsWith("spank ") && message.Contains("please") && message.EndsWith("?"))
				return caller + " 👋 (_(_) " + message.Replace("spank ", "").Replace("please", "").Replace("?", "");
			return null;
		}

		static private string kill(string text) {
			string message = IRCMessage.getMessageFromText(text);
			string caller = IRCMessage.getCallerFromText(text);
			if(message.StartsWith("kill ") && message.Contains("please") && message.EndsWith("?"))
				return caller + " sends his regards MrDestructoid forsenKnife " + message.Replace("kill ", "").Replace("please", "").Replace("?", "");
			return null;
		}

		static private string death(string text) {
			string message = IRCMessage.getMessageFromText(text);
			string caller = IRCMessage.getCallerFromText(text);
			if(message.ToLower().StartsWith("when will i die please") && message.EndsWith("?")) {
				Random x = new Random();
				int riggedText = x.Next(0, 6);
				string[] time = {" seconds", " minutes", " hours", " days", " months", " years"};
				return caller + ", According to science, you should be dead in " + x.Next(1, (int)Math.Pow(10, 6-riggedText)+1) + time[riggedText] + ". MrDestructoid";
			}
			return null;
		}

		private string cancer(string text) {
			string message = IRCMessage.getMessageFromText(text);
			if(message.StartsWith("cancer lottery") && message.EndsWith("?")) {
				Dictionary<int, string> answers = new Dictionary<int, string>();
				int indice = 0;
				string winner = null;
				Random x = new Random();

				using(StreamReader sr = new StreamReader("Cancer.txt")) {
					while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
				}


				//using(StreamWriter outputFile = new StreamWriter(File.Open("Cancerous.txt", FileMode.Create))) {
				using(WebClient wc = new WebClient()) {
					dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+TwitchClient.channel+"/chatters"));
					string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
					winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
					//outputFile.Write(jsonRaw.chatters.viewers.ToString());
				}
				//}
				return winner + ", You now have a "+ answers[x.Next(0, indice)]+"! "+ congrats();
				//return "EpicMango7, You now have a "+ answers[x.Next(0, indice)]+"! "+ congrats();
			}
			return null;
		}

		private string chromosome(string text) {
			string message = IRCMessage.getMessageFromText(text);
			if(message.StartsWith("chromosome lottery") && message.EndsWith("?")) {
				string winner = null;
				Random x = new Random();

				using(WebClient wc = new WebClient()) {
					dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+TwitchClient.channel+"/chatters"));
					string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
					winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
				}

				int extra = x.Next(-10, 10);
				string mol = extra<0 ? " less" : " more";
				return winner + ", You now have " + Math.Abs(extra) + mol +" chromosomes than average :) :)";
			}
			return null;
		}

		private string weeb(string text) {
			string message = IRCMessage.getMessageFromText(text);
			if(message.StartsWith("weeb lottery") && message.EndsWith("?")) {
				string winner = null;
				Random x = new Random();

				using(WebClient wc = new WebClient()) {
					dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+TwitchClient.channel+"/chatters"));
					string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
					winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
				}

				string action = new string[] {
						" suicide in front of everyone",
						" spamm your lolis forsenPuke"
					}[x.Next(0, 2)];


				return winner + ", You now have been designated by unanimity as the official weeb in this chat. You are therefore pleased to "+ action+" :)";
			}
			return null;
		}


		private string gym(string text) {
			string message = IRCMessage.getMessageFromText(text);
			if(message.StartsWith("who is the boss of this gym") && message.EndsWith("?")) {
				string winner = null;
				Random x = new Random();

				using(WebClient wc = new WebClient()) {
					dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+TwitchClient.channel+"/chatters"));
					string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
					winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
				}

				return winner.ToUpper() + " IS THE BOSS OF THIS GYM" + gachiEmote();
			}
			return null;
		}

		private string gachi() {
			string caller = IRCMessage.getCallerFromText(text);
			return new string[] {
					caller + ", http://imgur.com/Lo35Wpj "+ gachiEmote(),    caller + ", http://imgur.com/a/ORH3Y "+ gachiEmote(),    caller + ", http://imgur.com/q46GV6a "+ gachiEmote(),    caller + ", http://imgur.com/lvpQ8qK "+ gachiEmote(),
					caller + ", http://imgur.com/mKMxWtL "+ gachiEmote(),    caller + ", http://imgur.com/qoomcwL "+ gachiEmote(),    caller + ", http://imgur.com/E1bP7wc "+ gachiEmote(),    caller + ", http://imgur.com/hblBERk "+ gachiEmote(),
					caller + ", http://imgur.com/UpIW00T "+ gachiEmote(),    caller + ", http://imgur.com/3YGbyAv "+ gachiEmote(),    caller + ", http://imgur.com/ArNXBtq "+ gachiEmote(),    caller + ", http://imgur.com/NKfhvKi "+ gachiEmote(),
					caller + ", http://imgur.com/8225haP "+ gachiEmote(),    caller + ", http://imgur.com/pSIZB6I "+ gachiEmote(),    caller + ", http://imgur.com/vI7wvg6 "+ gachiEmote(),    caller + ", http://imgur.com/2bwezXf "+ gachiEmote(),
					caller + ", http://imgur.com/okJYCg1 "+ gachiEmote(),    caller + ", http://imgur.com/9sR3sYT "+ gachiEmote(),    caller + ", http://imgur.com/xZlCLK9 "+ gachiEmote(),    caller + ", http://imgur.com/ycykB4h "+ gachiEmote(),
					caller + ", http://imgur.com/WqXaM9n "+ gachiEmote(),    caller + ", http://imgur.com/RUrLa6P "+ gachiEmote(),    caller + ", http://imgur.com/BSNzC5t "+ gachiEmote(),    caller + ", http://imgur.com/cF6Ie2P "+ gachiEmote(),
					caller + ", http://imgur.com/KYji1Cr "+ gachiEmote(),    caller + ", http://imgur.com/VtXq3J4 "+ gachiEmote(),    caller + ", http://imgur.com/aisGkEQ "+ gachiEmote(),    caller + ", http://imgur.com/nM4J12i "+ gachiEmote(),
					caller + ", http://imgur.com/KqF3nXs "+ gachiEmote(),    caller + ", http://imgur.com/5xCwV4s "+ gachiEmote()
				}[new Random().Next(0, 30)];
		}
		private string gachiEmote() {
			return new string[] {
					" gachiBASS ",
					" gachiGASM ",
					" gachiGold ",
					" Jebaited "
				}[new Random().Next(0, 4)];
		}

		private string congrats() {
			return new string[] {
					"Congratulations !",
					"Awesome ! :o",
					"I'm proud of you :)",
					"That's what I like in you !",
					"You'll die alone you little shit forsenE",
					"Damn that's no luck.. LUL"
				}[new Random().Next(0, 6)];
		}
        
        private string trihard(string text) {
            string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
            if(message.StartsWith("trihard lottery") && message.EndsWith("?")) {
                string winner = null;
                Random x = new Random();

                using(WebClient wc = new WebClient()) {
                    dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+channel+"/chatters"));
                    string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
                    winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
                }

                return winner + ", I stole your bike TriHard";
            }
            return null;
        }

        private string kkona(string text) {
            string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
            if(message.StartsWith("kkona lottery") && message.EndsWith("?")) {
                string winner = null;
                Random x = new Random();

                using(WebClient wc = new WebClient()) {
                    dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+channel+"/chatters"));
                    string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());
                    winner = viewerArray[x.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
                }

                return winner + ", I voted for you as the next president \\ KKona /";
            }
            return null;
        }


        private string answeringMachine(string text) {
			string message = IRCMessage.getMessageFromText(text);
			string caller = IRCMessage.getCallerFromText(text);
			//Console.WriteLine();
			//Console.WriteLine("message:"+message.ToLower().Replace("'", "").Replace(",", ""));
			//Console.WriteLine("imafk:"+Program.imafk);
			//Console.WriteLine(message.ToLower().Replace("'", "").Replace(",", "").StartsWith("@mrzezertoid im afk"));
			if(!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(caller))
				if(message.ToLower().Replace("'", "").Replace(",", "").StartsWith("@mrzezertoid im afk")) {
					using(StreamWriter outputFile = new StreamWriter(File.Open("Afks.txt", FileMode.Append))) {
						outputFile.WriteLine(caller);
					}
					return caller+" is now afk. If you wish to talk to him, please say his nickname in the message and I'll relay it. MrDestructoid";
				}
			return null;
		}

		public string heAfkBro(string text) {
			string message = IRCMessage.getMessageFromText(text);
			string caller = IRCMessage.getCallerFromText(text);
			if(!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(caller)) {
				List<string> afks = new List<string>();
				string returnme = "";
				string removeMe = "";

				using(StreamReader sr = new StreamReader("Afks.txt")) {
					while(sr.Peek() >= 0) afks.Add(sr.ReadLine());
				}

				foreach(string afk in afks) {
					if(caller.Equals(afk)) {
						removeMe = afk;
					} else if(message.Contains(afk)) {
						returnme = "/w "+afk+" "+caller+": "+message;
					}
				}

				afks.Remove(removeMe);

				using(StreamWriter outputFile = new StreamWriter(File.Open("Afks.txt", FileMode.Create))) {
					foreach(string afk in afks) outputFile.WriteLine(afk);
				}

				return returnme;
			}
			return null;
		}
	}
}
