using System;
using System.Globalization;
using System.Threading;

namespace TwitchChatBot {
	class Program {
		//static IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "ZeZerT", "oauth:");
		static string channel = "forsenlol", preCom = "", postCom = "?", admin = "zezert";
		static IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "MrZezertoid", "oauth:");
		//static string channel = "forsenlol";
		static int commandsUsed = 0, relevant = 0;
		static string lastCommand = "";

		static void Main(string[] args) {
			irc.joinRoom(channel);
			specialAnswer("ZeZerT", "abc", "abc");
			while(true) heard(irc.readMessage());
		}

		static private void heard(string text) {
			try {
				if(!String.IsNullOrEmpty(text)) {
					string speaker = "unknown";

					string message = text.Substring(text.IndexOf(" :") + 2, text.Length - text.IndexOf(" :") - 2);
					//message = message.ToLower();

					try {
						if(text.Contains("!")) speaker = text.Substring(1, text.IndexOf("!") - 1);
					} catch(Exception e) {
						Console.WriteLine("Caught an error : " + e);
						Console.WriteLine("Due to : " + text);
					}

					Console.SetCursorPosition(0, Console.CursorTop); //move cursor
					Console.Write("Irrelevant messages : " + incrementRelevant() + "\t Relevant messages : " + commandsUsed);
					choice(message, speaker);
				}
			} catch(Exception e) {
				Console.WriteLine("Caught an error : " + e);
				Console.WriteLine("Due to : " + text);
			}
		}

		static private Boolean answer(string caller, string message, string command, string answer) {
			if(check(message, command)) return specialAnswer(caller, message, answer);
			return false;
		}
		static private Boolean specialAnswer(string caller, string message, string answer) {
			irc.sendChatMessage(answer);
			incrementUsed();

			DateTime dateTime = DateTime.Now;

			Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("\n[" + DateTime.Now.ToString("hh:mm:ss:fff") + "] " + caller + ": ");
			Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(message);
			Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.WriteLine("Answered " + answer);
			Console.ForegroundColor = ConsoleColor.Gray;
			try {
				Thread.Sleep(2000);
			} catch(Exception e) {
				Console.WriteLine("Caught an error : " + e);
				Console.WriteLine("Due to : Thread.Sleep(2000);");
			}
			return true;
		}

		static private Boolean check(string message, string command) {
			return message.StartsWith(preCom + command + postCom) || message.StartsWith(preCom + command + " " + postCom);
		}

		static private void choice(string message, string caller) {
			Boolean dic = false; // Do I Care
			if(false) {
				if(caller.Equals(admin))    // For the admin
					dic =  answer(caller, message, "slave", "MrDestructoid Senpai MrDestructoid noticed MrDestructoid me MrDestructoid")
						|| answer(caller, message, "fuckyou", "Oh yes sir ! gachiGASM")
						|| answer(caller, message, "givepoints", "!givepoints zezert all MrDestructoid");
				else                        // For the plebs
					dic =  answer(caller, message, "slave", caller + ", you are not my master MrDestructoid")
						|| answer(caller, message, "fuckyou", caller + ", No, fuck YOU leatherman ! gachiGASM")
						|| answer(caller, message, "givepoints", caller + ", Your ID doesn't match. Please go fuck yourself <3 MrDestructoid forsenWhip");

				// Standard commands						
				//KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan KKoceanMan ᴛᴀᴋᴇ ᴍᴇ ʙy ᴛʜᴇ ʜᴀɴᴅ KKoceanMan KKoceanMan KKoceanMan ʟᴇᴀᴅ ᴍᴇ ᴛᴏ ᴛʜᴇ ʟᴀɴᴅ ᴛʜᴀᴛ yᴏᴜ ᴜɴᴅᴇʀSᴛᴀɴᴅ KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan

				// AnswerPicker ap = new AnswerPicker();
				// ap.addAnswer(STARTS_WITH, "kappa", "Kappa");
				// ap.pickAnswer(caller, "kappa");
				if(!dic)
					dic =  answer(caller, message, "dududu", "forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls")
						|| answer(caller, message, "weeb", "Shut up, weeb ! forsenSheffy forsenWhip forsenLewd Now bend over, I'll show you the world. ")
						|| answer(caller, message, "weebs", "Weebs DansGame forsenPuke ")
						|| answer(caller, message, "bean", "I WAS EATING THIS BEAN forsenSWA")
						|| answer(caller, message, "beans", "I WAS EATING THOSE BEANS forsenSWA")
						|| answer(caller, message, "kappa", "Kappa")
						|| answer(caller, message, "lying", "@" + caller + " I'M NOT FUCKING LYING forsenSWA YOU FUCKING CUNT forsenSWA")
						|| answer(caller, message, "forsene", "This slave is a proud forsenE sub forsenDDK forsenWhip MrDestructoid")
						|| answer(caller, message, "blinak", "@Blinak, Hey look at my bot MrDestructoid")
						|| answer(caller, message, "viewbot", "@" + caller + ", I am here to serve. MrDestructoid")
						|| answer(caller, message, "gay", "@" + caller + ", I am now gay for you. KappaPride 🌹")
						|| answer(caller, message, "", "🤔 Hum yeah I'm not sure, I think we need more samples. 🤔")
						|| answer(caller, message, "old forsen", "ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ FeelsBadMan ☂ ヽ｀ヽ｀、ヽヽ｀ヽ｀、｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ '")
						|| answer(caller, message, "can on head", "AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤ 4Head")
						|| answer(caller, message, "brain power", "/me forsenPls O-oooooooooo AAAAE-A-A-I-A-U- JO-oooooooooooo AAE-O-A-A-U-U-A- E-eee-ee-eee AAAAE-A-E-I-E-A- JO-ooo-oo-oo-oo EEEEO-A-AAA-AAAA forsenPls")
						|| answer(caller, message, "where is dad", whereisdad())
						|| answer(caller, message, "cooldown", "Engine is now cooling down. MrDestructoid")
						|| answer(caller, message, "gazatu", "He's my favorite bot owner nyanPls after Blinak of course nyanPls and ZeZerT MrDestructoid")
						|| answer(caller, message, "zezert", "ZeZerT sends his regards MrDestructoid forsenKnife")
						//|| answer(caller, message, "flea", "⎝ FeelsBadMan Flea you hoe ")
						|| answer(caller, message, "finger", " ☟ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤ TakeNRG GivePLZ")
						|| answer(caller, message, "chromosome", "Oh no, I dropped my MONSTER chromosome ⎬⎨ that I use it for my  MAGNUM forsenE")
						|| answer(caller, message, "hands up", " ⎝⎞ TriHard ⎛⎠ I DINDU MUFFIN OFCR")
						|| answer(caller, message, "911", "ANELE █ █ KAPOW")
						|| answer(caller, message, "where is mom", whereismom())
						|| answer(caller, message, "spooders", "🕷🕷 ᕕ WutFace ᕗ NOT 🕷 THIS WAY  DansGame 🕸 OH NO I'M STUCK, TELL BILLY I LO..")
						|| answer(caller, message, "ants", "🐜 ANTS 🐜 WutFace 🐜🐜🐜 RUN FOR 🐜 YOUR LIFE 🐜🐜 ᕕ SwiftRage ᕗ")
						|| answer(caller, message, "holiday", "☀️ 🏖 🌴 MrDestructoid  Feels good to be on holiday")
						|| answer(caller, message, "entertained", "ARE YOU NOT ENTERTAINED ? SwiftRage 🗡")
						|| answer(caller, message, "oceanman", "KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan KKoceanMan ᴛᴀᴋᴇ ᴍᴇ ʙy ᴛʜᴇ ʜᴀɴᴅ KKoceanMan KKoceanMan KKoceanMan ʟᴇᴀᴅ ᴍᴇ ᴛᴏ ᴛʜᴇ ʟᴀɴᴅ ᴛʜᴀᴛ yᴏᴜ ᴜɴᴅᴇʀSᴛᴀɴᴅ KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan")
						|| answer(caller, message, "kids", "I have candies 🍭 PedoBear 🚚")
						|| answer(caller, message, "eu", "Europe was discovered in 1849 when Chuck Norris rode his horse over the Atlantic Ocean. Upon discovering land there, he named the discovery Eastern USA, which today is abbreviated as EU.")
						|| answer(caller, message, "leak", "M̶͕̯͈̲͍̩̤͖̣̼̭̘͈̻ͦͪ͐̏͒͛̈́ͫ̽ͭ̀ͯ͟͟ I can fix this 🔧 forsenE")
						|| answer(caller, message, "party", "💃🏃💃🚶💃🏃🏃🚶💃🏃🏃🚶🚶💃💃🚶🏃💃🏃🏃🚶💃🏃🚶💃🏃🚶💃🏃🏃🚶🏃🏃")
						|| answer(caller, message, "suicide", "This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage ");
				//
				if(!dic)
					dic = message.StartsWith("!enation") ? specialAnswer(caller, message, "/me DO NATION 👌💰")
						: message.StartsWith("!genji") ? specialAnswer(caller, message, "There is no way I add that weeb overwatch emote forsenSWA")
						: message.Contains("mrzezertoid") && message.EndsWith("?") ? specialAnswer(caller, message, "@" + caller + ", Idk kev forsenE")
						: message.Contains("i'm not fucking lying !") ? specialAnswer(caller, message, "@" + caller + ", YES YOU ARE BabyRage")
						: message.Contains("im not fucking lying !") ? specialAnswer(caller, message, "@" + caller + ", YES YOU ARE BabyRage")
						: message.EndsWith("?") && (eval(message, 15)) ? specialAnswer(caller, message, "🤔")
						: message.StartsWith("kill ") && message.Contains("please") && message.EndsWith("?") && message.Length>12 ? specialAnswer(caller, message, killplease(caller, message)) : false;
			}
			answer(caller, message, "pingme", "pinged @ZeZerT");
		}

		static private string killplease(string caller, string message) {
			return caller + " sends his regards MrDestructoid forsenKnife " + message.Replace("kill ", "").Replace("please", "").Replace("?", "");
		}

		static private string whereismom() {
			return new string[] {
				"She is right here GivePLZ https://www.twitch.tv/eloise_ailv TakeNRG",
				"I'm busy with her right now, come back in 5mins. forsenTriggered forsenWhip forsenLewd",
				"Who ? forsenE",
				"!whereismom forsenPuke2",
				"She is screaming in another stream for whatever reason forsenLewd"
			}[new Random().Next(0, 5)];
		}

		static private string whereisdad() {
			return new string[] {
				"Nani killed him BibleThump",
				"Dad went to get a pack of cigarettes, he'll soon be back... FeelsBadMan",
				"He and Nani are making a pleb right now forsenE",
				"He is now the ghost of the volume control. RIP SMSkull",
				"!whereisdad forsenPuke",
				"!whereisbilly gachiGASM",
				"He's live FailFish If you don't see him, refresh."
			}[new Random().Next(0, 7)];
		}

		static private Boolean eval(string evalme, int expectedValue) {
			// Removing all caps, all spaces and the ? from the string to evaluate.
			evalme = evalme.ToLower().Replace(" ", "").Replace("?", "");

			// Checking there is no alphabetical characters in the requested string.
			for(decimal dec = 97; dec <= 122; dec++) {
				if(evalme.IndexOf((char) dec) != -1) return false;
			}

			// Removing all spaces, because who cares about those.
			char itwas;
			Boolean moreThanOne = true;
			int signePositionP = evalme.IndexOf("+"),
				signePositionM = evalme.IndexOf("-"),
				signePositionD = evalme.IndexOf("/"),
				signePositionX = evalme.IndexOf("*"),
				signPosition = -1;

			if(signePositionP != -1) { signPosition = signePositionP; moreThanOne = !moreThanOne; itwas = '+'; } else if(signePositionM != -1) { signPosition = signePositionM; moreThanOne = !moreThanOne; itwas = '-'; } else if(signePositionD != -1) { signPosition = signePositionD; moreThanOne = !moreThanOne; itwas = '/'; } else if(signePositionX != -1) { signPosition = signePositionX; moreThanOne = !moreThanOne; itwas = '*'; } else return false;

			// Detects if more than one operator was found. If so, we skip.
			if(moreThanOne || signPosition == -1) return false;

			// Getting the interesting string from the one to evaluate. 
			string lefts = evalme.Substring(0, signPosition);
			string rights = evalme.Substring(signPosition+1, evalme.Length - signPosition-1);

			try { // Left part
				float lefti = float.Parse(lefts, CultureInfo.InvariantCulture);
				try { // Right part
					float righti = float.Parse(rights, CultureInfo.InvariantCulture);
					float result = 0;

					// Actual calculations
					switch(itwas) {
						case '+': result = lefti + righti; break;
						case '-': result = lefti - righti; break;
						case '/': result = lefti / righti; break;
						case '*': result = lefti * righti; break;
						default: result = -1; break;
					}

					if(result == expectedValue) return true;
					else return false;

				} catch(FormatException e) { // right try
					Console.WriteLine("Caught an error : " + e);
					Console.WriteLine("Due to : " + rights);
					return false;
				}
			} catch(FormatException e) { // left try
				Console.WriteLine("Caught an error : " + e);
				Console.WriteLine("Due to : " + lefts);
				return false;
			}
		}

		static private int incrementUsed() {
			return ++commandsUsed;
		}

		static private int incrementRelevant() {
			return ++relevant;
		}

		static private string setLastComUsed(string com) {
			return lastCommand = com;
		}
		static private string getLastComUsed() {
			return lastCommand;
		}
	}
}
