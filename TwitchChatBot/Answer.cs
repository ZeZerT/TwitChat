using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace TwitchChatBot {
	class Answer {
		private int type;
		private int count;
		private int special;
		private int withCaller;
		private string textAdmin;
		private string textPleb;
		private Boolean admin;
		private Boolean force;
		private Boolean ignorePrePostCom;

		private List<string> last5;
		private DateTime usedOn;
		static private Boolean exists = false;

		public const int NONE_HAVE_CALLER           = 20;
		public const int PLEB_STARTS_WITH_CALLER    = 21;
		public const int PLEB_ENDS_WITH_CALLER      = 22;
		public const int ADMIN_STARTS_WITH_CALLER   = 23;
		public const int ADMIN_ENDS_WITH_CALLER     = 24;
		public const int BOTH_STARTS_WITH_CALLER    = 25;
		public const int BOTH_ENDS_WITH_CALLER      = 26;

		public const int NONE                       = 30;
		public const int WHEREISMOM                 = 31;
		public const int WHEREISDAD                 = 32;
		public const int RANDOMIZE_CHAR             = 33;
		public const int RANDOMIZE_WORD             = 34;
		public const int CODEX                      = 35;
		public const int KILL						= 36;
		public const int SPANK                      = 37;
		public const int DEATH                      = 38;
		public const int CASCADE                    = 39;
		public const int CHAIN                      = 40;

		public Answer() {
			exists = false;
		}

		public Answer(int type, string text, int withCaller, Boolean admin, Boolean ignorePrePostCom, string textAdmin, int special) {
			last5 = new List<string>();
			exists = true;

			Count = 0;
			Type = type;
			Admin = admin;
			Special = special;
			TextPleb = text;
			WithCaller = withCaller;
			IgnorePrePostCom = ignorePrePostCom;
			TextAdmin = Admin ? textAdmin : TextPleb;
		}

		static public Boolean Exists(Answer checkme) {
			return exists;
		}

		public string getAnswer(string caller, Boolean admin) {
			last5.Add(caller);
			UsedOn = DateTime.Now;
			Count++;

			string start_call = "@" + caller;
			string end_call = caller + " .";

			AnswerPicker.debug("caller, admin, TextAdmin, TextPleb, special, Type", caller, admin, TextAdmin, TextPleb, special, Type);
			if(!String.IsNullOrEmpty(TextAdmin+textPleb))
			switch(withCaller) {
				case NONE_HAVE_CALLER:
					return admin ? TextAdmin : TextPleb;
				case PLEB_STARTS_WITH_CALLER:
					return admin ? TextAdmin : start_call + TextPleb;
				case PLEB_ENDS_WITH_CALLER:
					return admin ? TextAdmin : TextPleb + end_call;
				case ADMIN_STARTS_WITH_CALLER:
					return admin ? start_call + TextAdmin : TextPleb;
				case ADMIN_ENDS_WITH_CALLER:
					return admin ? TextAdmin + end_call : TextPleb;
				case BOTH_STARTS_WITH_CALLER:
					return admin ? start_call + TextAdmin : start_call + TextPleb;
				case BOTH_ENDS_WITH_CALLER:
					return admin ? TextAdmin + end_call : TextPleb + end_call;
				default: return null;
			}
			return null;
		}

		public void fillAnswers(string message, string caller, Boolean admin) {
			switch(special) {
				case WHEREISMOM:
					TextAdmin = TextPleb = Whereismom();
					break;
				case WHEREISDAD:
					TextAdmin = TextPleb = Whereisdad();
					break;
				case RANDOMIZE_CHAR:
					TextAdmin = TextPleb = ShuffleChar(message);
					break;
				case RANDOMIZE_WORD:
					TextAdmin = TextPleb = ShuffleWord(message);
					break;
				case CODEX:
					TextAdmin = TextPleb = readCodex(message);
					break;
				case KILL:
					TextAdmin = TextPleb = kill(message);
					break;
				case SPANK:
					TextAdmin = TextPleb = spank(message);
					break;
				case DEATH:
					TextAdmin = TextPleb = death(message);
					break;
				default:
					TextAdmin = TextPleb = null;
					break;
			}
		}

		public string getLastCaller() {
			List<string> names = last5.GetRange(Count -1, 1);
			return String.Join(", ", names.ToArray());
		}

		public string getLastCallers(int number) {
			if(Count -1 -number <0) number = Count -1;
			List<string> names = last5.GetRange(Count -1 -number, number +1);
			return String.Join(", ", names.ToArray());
		}

		public void forceResend(Boolean forceMeMaybe) {
			force = forceMeMaybe;
		}
		public Boolean canResend() {
			if(Special == NONE) {
				Boolean returnMe = force || DateTime.Now.Subtract(UsedOn).TotalSeconds > 30;
				force = false;
				return returnMe;
			}
			return true;
		}

		public Boolean isSpecial() {
			return Special != NONE;
		}

		private string Whereismom() {
			return new string[] {
				"She is right here GivePLZ https://www.twitch.com/eloise_ailv TakeNRG",
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
				Console.Write("indice:"+y+"\ttext:"+answers[y]);
				return answers[y];
			} catch(Exception e) {
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
				return "I had a problem, but my creator ZeZerT isn't responsible at all for he is mighty and skilled MrDestructoid";
			}
		}

		static private string spank(string message) {
			if(message.StartsWith("spank ") && message.Contains("please") && message.EndsWith("?"))
				return " 👋 (_(_) " + message.Replace("spank ", "").Replace("please", "").Replace("?", "");
			return null;
		}
		
		static private string kill(string message) {
			if(message.StartsWith("kill ") && message.Contains("please") && message.EndsWith("?"))
				return " sends his regards MrDestructoid forsenKnife " + message.Replace("kill ", "").Replace("please", "").Replace("?", "");
			return null;
		}

		static private string death(string message) {
			if(message.ToLower().StartsWith("when will i die ")&& message.Contains("please") && message.EndsWith("?")) {
				Random x = new Random();
				int riggedText = x.Next(0, 6);
				string[] time = {" seconds", " minutes", " hours", " days", " months", " years"};
				return ", According to science, you should be dead in " + x.Next(1, (int)Math.Pow(10, 6-riggedText)+1) + time[riggedText] + ". MrDestructoid";
			}
			return null;
		}

		public Int32 Type {
			get { return type; }
			set { this.type=value; }
		}

		public Int32 WithCaller {
			get { return withCaller; }
			set { this.withCaller=value; }
		}

		public Int32 Special {
			get { return special; }
			set { this.special=value; }
		}

		public Int32 Count {
			get { return count; }
			set { this.count=value; }
		}

		public String TextPleb {
			get { return textPleb; }
			set { this.textPleb=value; }
		}

		public String TextAdmin {
			get { return textAdmin; }
			set { this.textAdmin=value; }
		}

		public Boolean IgnorePrePostCom {
			get { return ignorePrePostCom; }
			set { this.ignorePrePostCom=value; }
		}

		public Boolean Admin {
			get { return admin; }
			set { this.admin=value; }
		}

		protected DateTime UsedOn {
			get { return usedOn; }
			set { this.usedOn=value; }
		}
	}

}