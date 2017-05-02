using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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
		private Random random = new Random();

		private List<string> last5;
		private DateTime usedOn;
		private Boolean exists = false;

		public const int NONE_HAVE_CALLER           = 20;
		public const int PLEB_STARTS_WITH_CALLER    = 21;
		public const int PLEB_ENDS_WITH_CALLER      = 22;
		public const int ADMIN_STARTS_WITH_CALLER   = 23;
		public const int ADMIN_ENDS_WITH_CALLER     = 24;
		public const int BOTH_STARTS_WITH_CALLER    = 25;
		public const int BOTH_ENDS_WITH_CALLER      = 26;

		public const int NONE                       = 300;
		public const int WHEREISMOM                 = 301;
		public const int WHEREISDAD                 = 302;
		public const int RANDOMIZE_CHAR             = 303;
		public const int RANDOMIZE_WORD             = 304;
		public const int CODEX                      = 305;
		public const int KILL                       = 306;
		public const int SPANK                      = 307;
		public const int DEATH                      = 308;
		public const int PING                       = 309;
		public const int HOWDEATH                   = 310;
		public const int WAKEUP                     = 311;
		public const int ROLL                       = 312;
		public const int FANCY                      = 313;

		public const int CANCER                     = 400;
		public const int CHROMOROME                 = 401;
		public const int WEEB                       = 402;
		public const int GACHI                      = 403;
		public const int GYM                        = 404;
		public const int TRIHARD                    = 405;
		public const int KKONA                      = 406;
		public const int JADEN                      = 407;
		public const int GABEN                      = 408;
		public const int KKOMRADE                   = 409;
		public const int DISGUSTING                 = 410;
		public const int OG                         = 411;
		public const int VALENTINE                  = 412;
		public const int RANDOM                     = 413;
		public const int PIGMENT                    = 414;
		public const int MYRANDOM                   = 415;
		public const int TRUMP	                    = 416;
		public const int SUPERPOWER                 = 417;
		public const int SUBWAY			            = 418;
		public const int GENDER                     = 419;
		public const int ANELE                      = 420;
		public const int WIKI                       = 421;



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
			return checkme.exists;
		}

		public string GetAnswer(string caller, Boolean admin) {
			last5.Add(caller);
			UsedOn = DateTime.Now;
			Count++;

			string start_call = "@" + caller;
			string end_call = caller + " .";

			AnswerPicker.Debug("caller, admin, TextAdmin, TextPleb, special, Type", caller, admin, TextAdmin, TextPleb, special, Type);
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

		public void FillAnswers(string text, string caller, Boolean admin) {
			switch(special) {
				case WHEREISMOM:
					TextAdmin = TextPleb = Whereismom();
					break;
				case WHEREISDAD:
					TextAdmin = TextPleb = Whereisdad();
					break;
				case RANDOMIZE_CHAR:
					TextAdmin = TextPleb = ShuffleChar(text);
					break;
				case RANDOMIZE_WORD:
					TextAdmin = TextPleb = ShuffleWord(text);
					break;
				case CODEX:
					TextAdmin = TextPleb = ReadCodex(text);
					break;
				case KILL:
					TextAdmin = TextPleb = Kill(text);
					break;
				case SPANK:
					TextAdmin = TextPleb = Spank(text);
					break;
				case DEATH:
					TextAdmin = TextPleb = Death(text);
					break;
				case PING:
					TextAdmin = TextPleb = Ping(text);
					break;
				case CANCER:
					TextAdmin = TextPleb = Cancer(text);
					break;
				case CHROMOROME:
					TextAdmin = TextPleb = Chromosome(text);
					break;
				case GACHI:
					TextAdmin = TextPleb = Gachi();
					break;
				case DISGUSTING:
					TextAdmin = TextPleb = Disgusting(text);
					break;
				case GYM:
					TextAdmin = TextPleb = Gym(text);
					break;
                case TRIHARD:
                    TextAdmin = TextPleb = Trihard(text);
                    break;
                case KKONA:
                    TextAdmin = TextPleb = Kkona(text);
                    break;
                case JADEN:
                    TextAdmin = TextPleb = Jaden(text);
                    break;
                case GABEN:
                    TextAdmin = TextPleb = Gaben(text);
                    break;
				case KKOMRADE:
					TextAdmin = TextPleb = Kkomrade(text);
					break;
				case WEEB:
					TextAdmin = TextPleb = Weeb(text);
					break;
				case OG:
					TextAdmin = TextPleb = Og(text);
					break;
				case VALENTINE:
					TextAdmin = TextPleb = Valentine(text);
					break;
				case RANDOM:
					TextAdmin = TextPleb = RandomLottery(text);
					break;
				case HOWDEATH:
					TextAdmin = TextPleb = Howdeath(text);
					break;
				case WAKEUP:
					TextAdmin = TextPleb = WakeUp(text);
					break;
				case PIGMENT:
					textAdmin = TextPleb = Pigment(text);
					break;
				case MYRANDOM:
					TextAdmin = TextPleb = MyRandomLottery(text);
					break;
				case TRUMP:
					TextAdmin = TextPleb = Trump(text);
					break;
				case ROLL:
					TextAdmin = TextPleb = Roll(text);
					break;
				case SUPERPOWER:
					TextAdmin = TextPleb = Superpower(text);
					break;
				case SUBWAY:
					TextAdmin = TextPleb = Subway(text);
					break;
				case GENDER:
					TextAdmin = TextPleb = Gender(text);
					break;
				case ANELE:
					TextAdmin = TextPleb = Anele(text);
					break;
				case FANCY:
					TextAdmin = TextPleb = Fancy(text);
					break;
				case WIKI:
					TextAdmin = TextPleb = Wiki(text);
					break;
				default:
					TextAdmin = TextPleb = null;
					break;
			}
		}
		
		public string GetLastCaller() {
			List<string> names = last5.GetRange(Count -1, 1);
			return String.Join(", ", names.ToArray());
		}

		public string GetLastCallers(int number) {
			if(Count -1 -number <0) number = Count -1;
			List<string> names = last5.GetRange(Count -1 -number, number +1);
			return String.Join(", ", names.ToArray());
		}

		public void ForceResend(Boolean forceMeMaybe) {
			force = forceMeMaybe;
		}
		public Boolean CanResend() {
			if(Special == NONE) {
				Boolean returnMe = force || DateTime.Now.Subtract(UsedOn).TotalSeconds > 30;
				force = false;
				return returnMe;
			}
			return true;
		}

		public Boolean IsSpecial() {
			return Special != NONE;
		}

		private string Whereismom() {
			return new string[] {
				"I don't know where is eloise_ailv, sorry.",
				"Who ? forsenE",
				"!whereismom forsenPuke2",
				"She is screaming in another stream for whatever reason forsenLewd"
			}[Random.Next(0, 4)];
		}

		private string Whereisdad() {
			return new string[] {
				"Nani killed him BibleThump",
				"Dad went to get a pack of cigarettes, he'll soon be back... FeelsBadMan",
				"He and Nani are making a pleb right now forsenE",
				"He is now the ghost of the volume control. RIP SMSkull",
				"!whereisdad forsenPuke",
				"He's live FailFish If you don't see him, refresh."
			}[Random.Next(0, 6)];
		}

		private string ShuffleChar(string unshuffled) {
			unshuffled = unshuffled.Substring(unshuffled.IndexOf("?")+1, unshuffled.Length - unshuffled.IndexOf("?")-1);
			return new string(unshuffled.OrderBy(r => Random.Next()).ToArray());
		}

		private string ShuffleWord(string unshuffled) {
			unshuffled = unshuffled.Substring(unshuffled.IndexOf("?")+1, unshuffled.Length - unshuffled.IndexOf("?")-1);
			return String.Join(" ", unshuffled.Split(' ').OrderBy(r => Random.Next()).ToArray());
		}

		private string ReadCodex(string startsWith = null) {
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
				int y = Random.Next(0, indice);
				//Console.Write("indice:"+y+"\ttext:"+answers[y]);
				return answers[y];
			} catch(Exception e) {
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
				return "I had a problem, but my creator ZeZerT isn't responsible at all for he is mighty and skilled MrDestructoid";
			}
		}

		private string Spank(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("spank ") && message.Contains("please") && message.EndsWith("?"))
				return " 👋 (_(_) " + message.Replace("spank ", "").Replace("please", "").Replace("?", "");
			return null;
		}

		private string Kill(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("kill ") && message.Contains("please") && message.EndsWith("?"))
				return " sends his regards MrDestructoid forsenKnife " + message.Replace("kill ", "").Replace("please", "").Replace("?", "");
			return null;
		}
		
		private string Ping(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("ping ") && message.Contains(" please") && message.EndsWith("?")) {
				string regexed = Regex.Match(message, @"\d+").Value;
				if(!String.IsNullOrEmpty(regexed)) {
					//return " pinged " + GetRandomWinner(int.Parse(regexed), 131);
					if(regexed.Length > 91) regexed = regexed.Substring(0, 100);
					return ", K­a­r­l­_­K­o­ns said : " + regexed + " more ping and ur banned :)";
				} else return ", Maybe you should learn how to write numbers";
			}
			return null;
		}

		private string WakeUp(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("wake up ") && message.Contains(" people please") && message.EndsWith("?")) {
				string regexed = Regex.Match(message, @"\d+").Value;
				if(!String.IsNullOrEmpty(regexed))
					return "Y'all should wake up forsenE " + GetRandomWinner(int.Parse(regexed), 131) + " ";
				else return " Maybe you should learn how to write numbers";
			}
			return null;
		}

		private string Death(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.ToLower().StartsWith("when will i die please") && message.EndsWith("?")) {
				int riggedText = Random.Next(0, 6);
				string[] time = {" seconds", " minutes", " hours", " days", " months", " years"};
				return ", According to science, you should be dead in " + Random.Next(1, (int)Math.Pow(10, 6-riggedText)+1) + time[riggedText] + ". MrDestructoid";
			}
			return null;
		}

		private string Howdeath(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.ToLower().StartsWith("how will i die please") && message.EndsWith("?")) {
				return WayToDie();
			}
			return null;
		}

		private string WayToDie() {
			return new string[] {
				", A bee will sting you very precisely and paralyze you. Meanwhile, the whole bee gang will rape your loved ones. You'll then suicide.",
				", You'll end up in a psychiatric hospital, and die of old age there. You won't have any internet access. ",
				", In exactly two days, you'll notice something weird on your skin. Believe me, you don't want to know what's next.",
				", Police will kill you during a raid, thinking you are dangerous. But it was just a prank, bro 4Head ",
				", You'll fucking suicide because you are a disgusting weeb DansGame ",
				", Some ZULUL warriors will free girafes from zoos and ride them in the streets. You'll be the second victime.",
				", oh shit bro stop asking me I'm not a fucking fortuneteller",
				", You will stab yourself in your sleep, thinking that you are a tomato. Looks like your family and you were part of a sandwich. ",
				", No spoiler but you would better be awake tonight, at around 6am. ",
				", No death for you. Only pain and misery. Forever. ",
				", Chocolate. ",
				", You have autism so it doesn't matter how you die, you'll be happy all along :D ",
				", MEGA-DILDO-MONSTER-GODZILLA-FUCKER-3000 on maximum power. It will take 2 weeks to clean the room after that.",
				", On this long night, you'll feel so alone you'll masturbate too much. Your brain will lack blood and you'll die. ",
				", You realize that you are a roleplayer, have done nothing productive for the past 3 years. So you decide to just end yourself.",
				", You cross the street without looking left and right. A truck runs you over, leaving a 1km trail of blood.",
				", You will be deported to the closest country, and will work as a sex slave. You'll die during a gang rape with 25 fat chineses.",
				", Remember that bug you stomped when you were a kid ? It's all species remembers. There will be no traces of you after that.",
				", Your mom will eat you.",
				", You will learn a funny news on TV, but unfortunatly, you'll also learn that you have heart problems.",
				", You'll get your dick sucked by a wood shredder. Dont worry, your whole body will follow.",
				", tusken will spam the chat so much during one stream that your computer will explode and kill everyone you ever love.",
				", You will find a funny lifehack on youtube called 'insurrance scam' but the car will actually kill you.",
				", I'll tell you, but first you have to drink 1 whole bottle of bleach :).",
				", monkaS I've seen bad shit but this one bro.. can't even describe it monkaS you better suicide to avoid all that pain.",
				", the website 'AdoptAMonkey.com' actually trains them to kill, and its too late for you to cancel the order.",
				", THE PROPHECY WAS TRUE, YOU ARE A FUCKING LOSER, NOW GO DIE YOU SCUM."
			}[Random.Next(0, 27)];
		}

		private string Cancer(string text, Boolean boolRandom = true) {
			Dictionary<int, string> answers = new Dictionary<int, string>();
            int indice = 0;

			using(StreamReader sr = new StreamReader("Cancer.txt")) {
				while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
			}

			string winner = boolRandom? GetRandomWinner():"";
			return winner + ", You now have a "+ answers[Random.Next(0, indice)]+"! "+ Congrats();
		}

		private string Chromosome(string text, Boolean boolRandom = true) {
			int extra = Random.Next(-10, 10);
			string mol = extra<0 ? " less" : " more";
			string winner = boolRandom? GetRandomWinner():"";
			return winner + ", You now have " + Math.Abs(extra) + mol +" chromosomes than average :) :)";
		}
		
		private string Disgusting(string text, Boolean boolRandom = true) {
            string action = new string[] {
				" sudoku in front of everyone",
				" spamm your lolis forsenPuke"
			}[Random.Next(0, 2)];

			string winner = boolRandom? GetRandomWinner():"";
			return winner + ", You now have been designated by unanimity as the official weeb in this chat. You are therefore welcome to "+ action+" :)";
		}

		private string Roll(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("roll ") && message.EndsWith("?")) {
				int matchCount = 0;
				int minVal = 0;
				int maxVal = 100;
				bool worked = false;
				var matches = Regex.Matches(message, "[0-9]+");
				foreach(var match in matches) {
					if(matchCount == 0) worked = Int32.TryParse(Convert.ToString(match), out maxVal);
					if(matchCount == 1) worked = Int32.TryParse(Convert.ToString(match), out minVal);
					matchCount++;
				}

				int rolled = (minVal < maxVal) ? Random.Next(minVal, maxVal) : Random.Next(maxVal, minVal);
				string ignored = worked ? (matchCount > 2)? " ("+(matchCount-2)+" inputs ignored)" : "": ", also fuck you and all your bits eShrug";
				return " rolled a "+rolled + ignored;
			}
			return null;
		}

		private string Gym(string text) {
			return GetRandomWinner().ToUpper() + " IS THE BOSS OF THIS GYM" + GachiEmote();
		}

		private string Subway(string text) {
			string bread = new string[] {
				"9-Grain Wheat",	"9-Grain Honey Oat",
				"Italian",			"Hearty Italian",
				"Italian Herbs",	"Flatbread"
			}[Random.Next(0, 6)];

			string flavour = new string[]{
				"Chicken & Chorizo Melt",	"Italian B.M.T.®",				"Chicken Tikka",		"Tuna",
				"Steak & Cheese",			"Ham",							"Meatball Marinara",	"Spicy Italian",
				"Turkey Breast and Ham",	"Chicken & Bacon Ranch Melt",	"Chicken Breast",		"Turkey Breast",
				"Chicken Teriyaki",			"VEGGIE DELITE®",				"SUBWAY MELT",			"Veggie Patty",
				"Chicken Pizziola",			"Big Beef Melt",				"Chipotle Chicken Melt"
			}[Random.Next(0, 19)];

			string cheese = new string[]{
				"American", "Cheddar"
			}[Random.Next(0,2)];

			string[] toppings = new string[]{ "Cucumbers", "Peppers", "Lettuce", "Red Onions", "Tomatoes", "Olives", "Jalapenos", "Pickles" };
			string[] sauce = new string[]{ "Southwest", "BBQ", "Ranch", "Sweet Chilli", "Honey Mustard", "Sweet Onion", "Lite Mayo" };

			//select toppings or sauce
			string addition = "";
			switch(Random.Next(0, 3)) {
				case 0: addition += toppings[Random.Next(0, 8)]; addition += " and "; addition += sauce[Random.Next(0, 7)]; break;
				case 1: addition += toppings[Random.Next(0, 8)]; addition += " and "; addition += toppings[Random.Next(0, 8)]; break;
				case 2: addition += sauce[Random.Next(0, 7)]; addition += " and "; addition += sauce[Random.Next(0, 7)]; break;
				default: break;
			}


			return " went to Subway™ and ordered a " +flavour+ " in " +bread+ " bread with " +cheese+ " cheese, "+ addition;
		}

		private string Trihard(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + ", I stole your "+ RandomObject();
        }

		private string Gender(string text, Boolean boolRandom = true) {
			Dictionary<int, string> answers = new Dictionary<int, string>();
			int indice = 0;
			using(StreamReader sr = new StreamReader("Gender.txt")) {
				while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
			}
			int y = Random.Next(0, indice);
			string winner = boolRandom? GetRandomWinner():"";
			return winner + " now identifies as a " + answers[y] +" (type \"gender helper ?\" for more info)";
		}

        private string Kkona(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + Kkonaquote();
        }

        private string Jaden(string text) {
            Dictionary<int, string> answers = new Dictionary<int, string>();
            int indice = 0;
            using(StreamReader sr = new StreamReader("Jaden.txt")) {
                while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
            }
            int y = Random.Next(0, indice);
            return answers[y];
        }

		private string Trump(string text) {
			Dictionary<int, string> answers = new Dictionary<int, string>();
			int indice = 0;
			using(StreamReader sr = new StreamReader("Trump.txt")) {
				while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
			}
			int y = Random.Next(0, indice);
			return answers[y];
		}

		private string Gaben(string text) {
			int nbGames = Random.Next(1, 20);
            double price = 0;

            for(int i = 0; i<nbGames; i++) price += Random.Next(5, 30)+Math.Round((Random.NextDouble()), 2);
            return " bought " + nbGames + " games on Steam for $" + price + " GabeN . He'll play none of them. GabeN ";
		}

		private string Kkomrade(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + Kkomradequote();
		}
		
		private string Weeb(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + ", In order to maintain some kind of balance, you are allowed to use 1 and only 1 weeb emote shit. OMGScoods";
		}

		private string Og(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + RandomOG();
		}

		private string Valentine(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + " was friend-zoned on valentine's day forsenFeels";
		}

		private string Pigment(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";
			return winner + " is " + PigmentPicker();
		}

		private string Anele(string text, Boolean boolRandom = true) {
			string winner = boolRandom? GetRandomWinner():"";

			string txt = (Random.Next(0,101) >10)? (" has a bomb in his " + BombPicker() + " ANELE 📢 )) ALLAHU AKBAR"):" was given 72 virgins PogChamp some of them are boys gachiGASM some of them are younglings PedoBear";
			return winner + txt;
		}

		private string Wiki(string text, Boolean boolRandom = true) {
			return GetFinalRedirect("https://en.wikipedia.org/wiki/Special:Random");;
		}

		private string BombPicker() {
			return new string[] {
				"turban",
				"anus",
				"shoes",
				"bomb vest",
				"nose",
				"mouth",
				"eyes <3 ",
				"town's weeb shelter",
				"mosque",
				"mom",
				"car"
			}[Random.Next(0, 11)];
		}

		private string PigmentPicker() {
			return new string[] {
				"🏻 - a cracker 4Head",
				"🏼 - dog eater MingLee",
				"🏽 - truck driver ANELE",
				"🏾 - chicken lover TriHard",
				"🏿 - a ninja forsenMonkey "
			}[Random.Next(0, 5)];
		}

		private string RandomLottery(string text) {
			switch((int)Random.Next(0, 12)) {
				case 0:		return Cancer(text);	
				case 1:		return Chromosome(text);
				case 2:		return Weeb(text);		
				case 3:		return Trihard(text);	
				case 4:		return Kkona(text);		
				case 5:		return Jaden(text);		
				case 6:		return Kkomrade(text);	
				case 7:		return Disgusting(text);
				case 8:		return Og(text);		
				case 9:		return Valentine(text);
				case 10:    return Pigment(text);
				case 11:	return Wiki(text);
				default:	return "fuck you all.";
			}
		}

		private string MyRandomLottery(string text) {
			switch((int)Random.Next(0, 14)) {
				case 0: return Cancer(text, false);
				case 1: return Chromosome(text, false);
				case 2: return Weeb(text, false);
				case 3: return Trihard(text, false);
				case 4: return Kkona(text, false);
				case 5: return Pigment(text, false);
				case 6: return Kkomrade(text, false);
				case 7: return Disgusting(text, false);
				case 8: return Og(text, false);
				case 9: return Valentine(text, false);
				case 10: return Pigment(text, false);
				case 11: return Superpower(text, false);
				case 12: return Gender(text, false);
				case 13: return Anele(text, false);
				default: return "fuck you all.";
			}
		}

		private string Superpower(string text, Boolean boolRandom = true) {
			//http://powerlisting.wikia.com/wiki/List_of_Supernatural_Powers_and_Abilities
			Dictionary<int, string> answers = new Dictionary<int, string>();
			int indice = 0;
			using(StreamReader sr = new StreamReader("Superpowers.txt")) {
				while(sr.Peek() >= 0) answers.Add(indice++, sr.ReadLine());
			}
			int y = Random.Next(0, indice);
			return " now has the following superpower : " + answers[y] + " PogChamp" ;
		}

		private string Fancy(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			if(message.StartsWith("fancy ") && message.Contains(" please") && message.EndsWith("?")) {
				string emote = message.Replace("fancy ", "").Replace(" please", "").Replace("?", "");
				string[] candidateEmoteList = emote.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);

				Boolean allEmotesOk = true;
				int countEmote = 0;
				List<string> emoteList = new List<string>() ;
				foreach(string candidate in candidateEmoteList) {
					string checkedString = CheckInEmoteList(candidate);
					if(checkedString != null) {
						allEmotesOk = allEmotesOk && true;
						emoteList.Add(checkedString);
						countEmote++;
						Console.WriteLine(candidate);
						Console.WriteLine(countEmote);
					} else allEmotesOk = false;
				}

				if(allEmotesOk) {
					string result = "/me ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ ";
					for(int i = 0; i < 10; i++)	result += emoteList[i%(countEmote)] + " ";
					result += " ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬";
					if(result.Length > 175) return ">175 characters MrDestructoid Nope MrDestructoid ";
					return result;
				}
				return "1 or more emote(s) not found MrDestructoid Learn to type maybe MrDestructoid";
			}
			return null;
		}
		private string CheckInEmoteList(string potentialEmote) {
			List<string> emoteList = Program.emoteList;
			
			Boolean stopForeach = false;
			string emoteFound = null;
			foreach(string elem in emoteList) {
				if(!stopForeach && potentialEmote.Equals(elem.ToLower())) {
					stopForeach = true;
					emoteFound = elem;
				}
			}

			Console.WriteLine("checkinemotelist : -"+emoteFound+"-");
			return emoteFound; // null if not found
		}

		public static string GetFinalRedirect(string url) {
			if(string.IsNullOrWhiteSpace(url))
				return url;

			int maxRedirCount = 8;  // prevent infinite loops
			string newUrl = url;
			do {
				HttpWebRequest req = null;
				HttpWebResponse resp = null;
				try {
					req = (HttpWebRequest)HttpWebRequest.Create(url);
					req.Method = "HEAD";
					req.AllowAutoRedirect = false;
					resp = (HttpWebResponse)req.GetResponse();
					switch(resp.StatusCode) {
						case HttpStatusCode.OK:
							return newUrl;
						case HttpStatusCode.Redirect:
						case HttpStatusCode.MovedPermanently:
						case HttpStatusCode.RedirectKeepVerb:
						case HttpStatusCode.RedirectMethod:
							newUrl = resp.Headers["Location"];
							if(newUrl == null)
								return url;

							if(newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1) {
								// Doesn't have a URL Schema, meaning it's a relative or absolute URL
								Uri u = new Uri(new Uri(url), newUrl);
								newUrl = u.ToString();
							}
							break;
						default:
							return newUrl;
					}
					url = newUrl;
				} catch(WebException) {
					// Return the last known good URL
					return newUrl;
				} catch(Exception) {
					return null;
				} finally {
					if(resp != null)
						resp.Close();
				}
			} while(maxRedirCount-- > 0);

			return newUrl;
		}

		private string GetRandomWinner(int howMany =1, int limit =175) {
            using(WebClient wc = new WebClient()) {
                dynamic jsonRaw = JsonConvert.DeserializeObject(wc.DownloadString("https://tmi.twitch.tv/group/user/"+Program.channel+"/chatters"));
                string[] viewerArray = jsonRaw.chatters.viewers.ToString().Split(",".ToCharArray());

				int howlimit = 15;
				if(howMany > howlimit) howMany = howlimit;

				string selected ="";
				for(int i = 0; i < howMany; i++) {
					string winner = " " + viewerArray[Random.Next(0, viewerArray.Length)].Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("[", "").Replace("]", "");
					if(!IsUnpingable(winner) && (selected + winner).Length <= limit) {
						selected += winner;
					}
				}
				return selected;
            }
        }

		private Boolean IsUnpingable(string Name) {
			Boolean result = false;
			string[] Unpingable = new string[] {"johnpogchamp", "boynextdoorkreygasm"};
			foreach(string x in Unpingable)	result |= Name.Equals(x);
			return result;
		}

		private string Gachi() {
			return new string[] {
				", http://imgur.com/Lo35Wpj "+ GachiEmote(),	", http://imgur.com/a/ORH3Y "+ GachiEmote(),	", http://imgur.com/q46GV6a "+ GachiEmote(),	", http://imgur.com/lvpQ8qK "+ GachiEmote(),
				", http://imgur.com/mKMxWtL "+ GachiEmote(),	", http://imgur.com/qoomcwL "+ GachiEmote(),	", http://imgur.com/E1bP7wc "+ GachiEmote(),	", http://imgur.com/hblBERk "+ GachiEmote(),
				", http://imgur.com/UpIW00T "+ GachiEmote(),	", http://imgur.com/3YGbyAv "+ GachiEmote(),	", http://imgur.com/ArNXBtq "+ GachiEmote(),	", http://imgur.com/NKfhvKi "+ GachiEmote(),
				", http://imgur.com/8225haP "+ GachiEmote(),	", http://imgur.com/pSIZB6I "+ GachiEmote(),	", http://imgur.com/vI7wvg6 "+ GachiEmote(),	", http://imgur.com/2bwezXf "+ GachiEmote(),
				", http://imgur.com/okJYCg1 "+ GachiEmote(),	", http://imgur.com/9sR3sYT "+ GachiEmote(),	", http://imgur.com/xZlCLK9 "+ GachiEmote(),	", http://imgur.com/ycykB4h "+ GachiEmote(),
				", http://imgur.com/WqXaM9n "+ GachiEmote(),	", http://imgur.com/RUrLa6P "+ GachiEmote(),	", http://imgur.com/BSNzC5t "+ GachiEmote(),	", http://imgur.com/cF6Ie2P "+ GachiEmote(),
				", http://imgur.com/KYji1Cr "+ GachiEmote(),	", http://imgur.com/VtXq3J4 "+ GachiEmote(),	", http://imgur.com/aisGkEQ "+ GachiEmote(),	", http://imgur.com/nM4J12i "+ GachiEmote(),
				", http://imgur.com/KqF3nXs "+ GachiEmote(),	", http://imgur.com/5xCwV4s "+ GachiEmote()
			}[Random.Next(0, 30)];
		}

		private string GachiEmote() {
			return new string[] {
				" gachiBASS ",
				" gachiGASM ",
				" gachiGold ",
				" gachiGAZUMU  ",
				" forsenGASM  ",
				" HandsUp ",
				" noxWhat ",
				" noxSorry ",
				" gachiLEE ",
				" tpHeresTommy "
			}[Random.Next(0, 7)];
        }

        private string Kkonaquote() {
            return new string[] {
                ", I voted for you as the next president \\ KKona /",
                ", I wanted yah to know I had sex with my cousin yesterday KKona ",
                ", I gave yah mah new trucker hat KKona",
                ", This is a private propriety and I'm allowed to shot you KKona forsenGun"
            }[Random.Next(0, 4)];
        }

		private string Kkomradequote() {
			return new string[] {
				", You were proven guilty of spying. The sentence is death. KKomrade 7 forsenEmote2",
				", Mother land is proud of you. KKomrade 7 forsenEmote2",
				", You are in charge of this submarine now. KKomrade 7 forsenEmote2",
				", Remember that time some people tried to invade KKomrade land during winter ? LUL"
			}[Random.Next(0, 4)];
		}

		private string RandomObject() {
			return new string[] {
				"bike TriHard / 🚲 ",
				"TV TriHard / 📺",
				"bitch TriHard ",
				"heart TriHard 💜"
			}[Random.Next(0, 4)];
		}

		private string RandomOG() {
			return new string[] {
				", you are now EpicMango7's bitch forsenOG",
				", you are now the most popular person in the club forsenOG",
				", forsenOG It's my territory, don't sell here cmonBruh ",
				" forsenOG forsenGun   🔫 cmonBruh " + GetRandomWinner()
			}[Random.Next(0, 4)];
		}

		private string Congrats() {
			return new string[] {
				"Congratulations !",
				"Awesome ! :o",
				"I'm proud of you :)",
				"That's what I like in you !",
				"You'll die alone you little shit forsenE",
				"Damn that's no luck.. LUL"
			}[Random.Next(0, 6)];
		}
		
		private string AnsweringMachine(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			string caller = AnswerPicker.GetCaller(text);
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

		public string HeAfkBro(string text) {
			string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
			string caller = AnswerPicker.GetCaller(text);
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

		public override string ToString() {
			return "type="+Type+" TextPleb="+TextPleb+" withCaller="+WithCaller+" admin="+Admin+" ignorePrePostCom="+IgnorePrePostCom+" textAdmin="+TextAdmin+" special="+Special;
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

		public Random Random {
			get => random;
			set => random = value;
		}
	}

}