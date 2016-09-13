using System;
using System.Linq;

namespace TwitchChatBot {
	class SpecialAnswer:Answer {
		public const int NONE		        = 30;
		public const int WHEREISMOM			= 31;
		public const int WHEREISDAD			= 32;
		public const int RANDOMIZE_CHAR		= 33;
		public const int RANDOMIZE_WORD		= 34;
		private int special;

		public SpecialAnswer(Boolean admin, Boolean ignorePrePostCom, int special)
			:base(AnswerPicker.SPECIAL, "", Answer.NONE_HAVE_CALLER, admin, ignorePrePostCom, "") {
			this.special = special;
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
	}
}
