using System;
using System.Linq;
using System.Collections.Generic;

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

		public const int NONE						= 30;
		public const int WHEREISMOM					= 31;
		public const int WHEREISDAD					= 32;
		public const int RANDOMIZE_CHAR				= 33;
		public const int RANDOMIZE_WORD				= 34;

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

			string start_call = "@" + caller + ", ";
			string end_call = ", " + caller + " .";

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
		}

		public void fillAnswers(string message, string caller, Boolean admin) {
			switch(special) {
				case WHEREISMOM:
					TextAdmin = Whereismom();
					TextPleb = Whereismom();
					break;
				case WHEREISDAD:
					TextAdmin = Whereisdad();
					TextPleb = Whereisdad();
					break;
				case RANDOMIZE_CHAR:
					TextAdmin = ShuffleChar(message);
					TextPleb = ShuffleChar(message);
					break;
				case RANDOMIZE_WORD:
					TextAdmin = ShuffleWord(message);
					TextPleb = ShuffleWord(message);
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

		public void forceResend() {
			force = true;
		}
		public Boolean canResend() {
			Boolean returnMe = force || DateTime.Now.Subtract(UsedOn).TotalSeconds > 30;
			force = false;
			return returnMe;
		}

		public Boolean isSpecial() {
			return Special != NONE;
		}

		private string Whereismom() {
			return new string[] {
				"She is right here GivePLZ https://www.twitch.tv/eloise_ailv TakeNRG",
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
