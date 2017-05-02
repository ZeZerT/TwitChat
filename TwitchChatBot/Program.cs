using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace TwitchChatBot {
	class Program {
		//public static string channel = "amazhs", preCom = "", postCom = "?", admin = "zezert";
		public static string channel = "forsenlol", preCom = "", postCom = "?", admin = "zezert";
		//public static string channel = "zezert", preCom = "", postCom = "?", admin = "zezert";
		static IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "MrZezertoid", "oauth:j0zyvyvajdg1rqrfl8b3qntyfxdhym");
		public static List<string> emoteList;

		static public string imafk= "@MrZezertoid im afk";
		static void Main(string[] args) {
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			AnswerPicker pickMeChat = new AnswerPicker(preCom, postCom, admin);
			string lastUse = DateTime.Now.ToString("hh:mm:ss:fff");
			string lastRead = DateTime.Now.ToString("hh:mm:ss:fff");
			irc.JoinRoom(channel);
			
			string ifNotNullSend = "";
			Boolean flagContinue = true;
			//pickMeChat.helloMessage(irc, "That turns me on gachiGASM");

			//Emotes

			emoteList = new List<string>();
			
			using(WebClient wc = new WebClient()) {
				// Twitch
				string emoteGrabberUrl = "https://api.twitch.tv/kraken/chat/emoticon_images?client_id=kd214h6wl799tlq6pndubz0onxry08&emotesets=";
				int[] emotesets = new int[] {0/*global*/, 4236/*forsen*/, 21958/*powerrangers*/};

				for(int i = 0; i < emotesets.Length; i++) {
					IEnumerable<JToken> emoteCodeTwitch = JObject.Parse(wc.DownloadString(emoteGrabberUrl+emotesets[i])).SelectTokens("$..code");
					foreach(JToken item in emoteCodeTwitch) emoteList.Add(item.ToString());
				}

				// BTTV
				string bttvGlobalUrl = "https://api.betterttv.net/2/emotes";
				string bttvSubUrl = "https://api.betterttv.net/2/channels/"+channel;
				IEnumerable<JToken> emoteGlobalBTTV = JObject.Parse(wc.DownloadString(bttvGlobalUrl)).SelectTokens("$..code");
				IEnumerable<JToken> emoteSubBTTV = JObject.Parse(wc.DownloadString(bttvSubUrl)).SelectTokens("$..code");
				foreach(JToken item in emoteGlobalBTTV) emoteList.Add(item.ToString());
				foreach(JToken item in emoteSubBTTV) emoteList.Add(item.ToString());

				// FFZ
				bool takeThisOne = true;
				string ffzGlobalUrl = "http://api.frankerfacez.com/v1/set/global";
				string ffzSubUrl = "http://api.frankerfacez.com/v1/room/"+channel;
				IEnumerable<JToken> emoteGlobalFFZ = JObject.Parse(wc.DownloadString(ffzGlobalUrl)).SelectTokens("$..name"); // couper les pairs
				IEnumerable<JToken> emoteSubFFZ = JObject.Parse(wc.DownloadString(ffzSubUrl)).SelectTokens("$..name"); // couper les pairs
				foreach(JToken item in emoteGlobalFFZ) {
					if(takeThisOne) emoteList.Add(item.ToString());
					takeThisOne = !takeThisOne;
				}
				foreach(JToken item in emoteSubFFZ) {
					if(takeThisOne) emoteList.Add(item.ToString());
					takeThisOne = !takeThisOne;
				}

				// custom
				emoteList.Add("<3");	emoteList.Add(":(");	emoteList.Add(":)");	emoteList.Add(":/");	emoteList.Add(";p");
				emoteList.Add(":D");	emoteList.Add(":o");	emoteList.Add(":O");	emoteList.Add(":P");	emoteList.Add(";P");
				emoteList.Add(":p");	emoteList.Add(":z");	emoteList.Add(";)");	emoteList.Add(">(");
			}//emoteList.ForEach(Console.WriteLine);
			
			// Admin / pleb differenciated commands
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "slave", ", you are not my master MrDestructoid",
								 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "MrDestructoid Senpai MrDestructoid noticed MrDestructoid me MrDestructoid");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "fuck you", ", No, fuck YOU leatherman ! gachiGASM",
							 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "Oh yes sir ! gachiGASM");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "givepoints", ", Your ID doesn't match. Please go fuck yourself <3 MrDestructoid forsenWhip",
							 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "!givepoints ZeZerT all MrDestructoid");
			//pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!join", "",Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "!join");


			// Regular commands			
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "pleb", "This is a suicide test. If you're a pleb, kill yourself forsenC forsenGun");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "test", "This is a Kappa test. If Kappa is Kappa , then Kappa yourself");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "kids", "I have candies 🍭 PedoBear 🚚");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "dududu", "forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "weeb", "Keep moving, it's funnier for me forsenSheffy forsenGun nyanPls");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "beans", "I WAS EATING THOSE BEANS forsenSWA");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "lying", " I'M NOT FUCKING LYING forsenSWA YOU FUCKING CUNT forsenSWA", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "sub", "This slave is a proud forsenE sub forsenDDK forsenWhip MrDestructoid");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "viewbot", ", I am here to serve. MrDestructoid", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "gay", ", I am now gay for you. KappaPride 🌹", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "old forsen", "ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ FeelsBadMan ☂ ヽ｀ヽ｀、ヽヽ｀ヽ｀、｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ '");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "brain power", "/me forsenPls O-oooooooooo AAAAE-A-A-I-A-U- JO-oooooooooooo AAE-O-A-A-U-U-A- E-eee-ee-eee AAAAE-A-E-I-E-A- JO-ooo-oo-oo-oo EEEEO-A-AAA-AAAA forsenPls");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "cooldown", "Engine is now cooling down. MrDestructoid");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "gazatu", "He's my favorite bot owner nyanPls after Blinak of course nyanPls and ZeZerT MrDestructoid");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "zezert..", "🤔 What if I had two accounts 🤔 And I sub both to this retard 🤔");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "finger my ass", "👉(_(_) FCreep");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "chromosome", "Oh no, I dropped my MONSTER chromosome ⎬⎨ that I use it for my MAGNUM autism forsenE");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "hands up", "forsenSheffy forsenGun ⎝⎞ TriHard ⎛⎠ I DINDU MUFFIN OFCR");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "911", "ANELE ✈ █ ✈ █ KAPOW");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "ants", "🐜 ANTS 🐜 WutFace 🐜🐜🐜 RUN FOR 🐜 YOUR LIFE 🐜🐜 ᕕ SwiftRage ᕗ");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "holiday", "☀️ 🏖 🌴 MrDestructoid  Feels good to be on holiday");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "oceanman", "KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan KKoceanMan ᴛᴀᴋᴇ ᴍᴇ ʙy ᴛʜᴇ ʜᴀɴᴅ KKoceanMan KKoceanMan KKoceanMan ʟᴇᴀᴅ ᴍᴇ ᴛᴏ ᴛʜᴇ ʟᴀɴᴅ ᴛʜᴀᴛ yᴏᴜ ᴜɴᴅᴇʀSᴛᴀɴᴅ KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "eu", "Europe was discovered in 1849 when Chuck Norris rode his horse over the Atlantic Ocean. Upon discovering land, he named the it Eastern USA, which today is abbreviated as EU.");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "leak", "I can fix this 🔧 forsenE");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "bean", "I WAS EATING THIS BEAN forsenSWA");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "party", "💃🏃💃🚶💃🏃🏃🚶💃🏃🏃🚶🚶💃💃🚶🏃💃🏃🏃🚶💃🏃🚶💃🏃🚶💃🏃🏃🚶🏃🏃");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "suicide", "This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage ");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "racism on twitch", "Guess which skin color is banned on twitch. Don't know yet? Here is a tip : Red, Yellow, Green, Blue, TriHard , White");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "dungeon", "[ KKaper KKaper ] Look what I have in my dungeon forsenDDK");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "entertained", "ARE YOU NOT ENTERTAINED ? SwiftRage 🗡");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "memer of the day", "http://i.imgur.com/dbYwyIS.png PogChamp");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "billy", "Some religions call him \"God\", for others it's \"Allah\", for me it's just \"Billy\".");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "savjz", "I save my jizz for Savjz forsenLewd");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "isis", "http://i.imgur.com/8GQxpPt.png");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "lea cosplay", "http://i.imgur.com/sdLwZlW.gifv");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "8ball", "http://imgur.com/a/vtRbq :o OhMyDog ");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "police sirens", "Oh shit dawg TriHard 🚿 Lets get white yo");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "prime", "🚮");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "ass", "gachiBASS forsenFeels After 3 month of assfucking, my ass is now insensitive.");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "retarded weeb", "forsenLewd nymnCorn I TRY TO EAT POPCORN BUT I'M A RETARDED WEEB forsenLewd nymnCorn OH NO I MISSED AGAIN forsenLewd nymnCorn I'M SO HUNGRY, I BETTER DANCE nyanPls ");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "superpower", "Hey weebs, I saw a hentai where they drank bleech and it gave them superpowers, you should try 4Head");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "triggered", "Women are not sexual objects HotPokket BUT MEN ARE gachiGASM");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "watermelon", "Watermelon. Earthmelon. Firemelon. Airmelon. Long ago the four melon nations lived together in harmony. Then everything changed when the Firemelon Nation attacked.");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "anal", "Gachi opened my eyes FeelsGoodMan I used to believe the ass was a dead end, now I know it's just the begining of the road FeelsGoodMan");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "memes", "I used to like memes FeelsBadMan BUT THEN I LOVED THEM FeelsAmazingMan");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "forsenPleb", "Sometimes I dream that forsen comes in this chat with a fake account, and everyone tells him \"shut up pleb you do nothing for this stream\" LUL ") ;
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "blinak", "BLINAK: zezert, blinak's level was set to 1 (default user). pajaW");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "playlist", "https://zezert.andchill.tv PotatoPls");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "are you sure", "🤔 Hum yeah I'm not sure, I think we need more samples. 🤔");
            pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "heresy", "IN CASE OF HERESY, KILL EVERYONE IN SIGHTS SwiftRage forsenGun ");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "mrzezertoid", ", Idk kev forsenE", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "gender helper", "HotPokket Find your gender on https://ageofshitlords.com/list-of-all-tumblr-genders-so-far/ (big list with description, idgaf if the site is shit)");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "TriHard record", "TriHard record = 8608 for Forsenlol LULW");
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "daily dose", "HandsUp CMON HandsUp MAN HandsUp  https://youtu.be/K8k4NU39skc GachiPls DAILY GachiPls DOSE GachiPls  ");

			// Irregular commands
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!brainpower", "/me forsenPls O-oooooooooo AAAAE-A-A-I-A-U- JO-oooooooooooo AAE-O-A-A-U-U-A- E-eee-ee-eee AAAAE-A-E-I-E-A- JO-ooo-oo-oo-oo EEEEO-A-AAA-AAAA forsenPls", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!fancydance", "/me ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ ", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!bot", "monkaS So many bots in this chat", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!offlinechat", "/me JAVLA FITTA KUK HELVETE", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!??", "/me  ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ ❓❓❓❓❓❓❓❓ Hhhehehe  Hhhehehe ❓❓❓❓❓❓❓❓❓❓ Hhhehehe Hhhehehe ❓❓▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!forsenpls", "/me  ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls forsenPls ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!enation", "/me DO NATION 👌💰", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!genji", "There is no way I add that weeb overwatch emote forsenSWA", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!distq", "you know what? I'm so done with this channel. forsen doesnt read chat, doesnt click my links. it's overrun by bots and idiots. Fuck twitch chat and fuck this", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!zonteck", "Z noxWhat N T E C K", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "!fucktheplebs", "▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬▬ forsenC forsenGun forsenC forsenGun forsenC forsenGun forsenC forsenGun forsenC forsenGun ▬▬▬▬▬▬▬▬▬▬ஜ۩۞۩ஜ▬▬▬▬▬▬▬▬▬", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			
			// Special commands
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "spank", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.SPANK);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "kill", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.KILL);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "ping", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.PING);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "wake", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.WAKEUP);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "randomize those words", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.RANDOMIZE_WORD);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "randomize this", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.RANDOMIZE_CHAR);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "where is dad", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.WHEREISDAD);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "where is mom", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.WHEREISMOM);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "thought for the day", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.CODEX);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "when will I die please", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.DEATH);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "how will I die please", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.HOWDEATH);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "roll", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.ROLL);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "fancy", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.FANCY);
			
			// Lotteries
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "cancer lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.CANCER);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "chromosome lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.CHROMOROME);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "weeb lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.WEEB);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "gachi lottery", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.GACHI);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "who is the boss of this gym ?", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.GYM);
            pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "trihard lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.TRIHARD);
            pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "kkona lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.KKONA);
            pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "jaden lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.JADEN);
            pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "gaben lottery", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.GABEN);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "KKomrade lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.KKOMRADE);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "disgusting lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.DISGUSTING);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "forsenOG lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.OG);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "valentine lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.VALENTINE);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "random lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.RANDOM);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "my lottery", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.MYRANDOM);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "pigment lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.PIGMENT);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "trump lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.TRUMP);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "superpower lottery", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.SUPERPOWER);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "subway lottery", channel, Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.SUBWAY);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "gender lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.GENDER);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "anele lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.ANELE);
			pickMeChat.AddAnswer(AnswerPicker.SPECIAL, "wiki lottery", channel, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, channel, Answer.WIKI);

			// Spam bot
			//string emote = "BrokeBack";
			//pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, emote, emote, Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);

			// End the bot
			pickMeChat.AddAnswer(AnswerPicker.STARTS_WITH, "Death is the only answer.", "Duty prevails.", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "Only in death does duty end.");
			
			string checkString = "";
			int skipThem = 9;
			string maybeAnswerThat = "";
			Answer afkManager = new Answer();

			if(pickMeChat.CanWeGo()) {
				while(flagContinue) {
					checkString = irc.ReadMessage();
					if(!String.IsNullOrEmpty(checkString)) {
						if(skipThem >0) skipThem--;
						else {
							ifNotNullSend = pickMeChat.PickAnswer(checkString);
							lastRead = DateTime.Now.ToString("hh:mm:ss:fff");
							if(!String.IsNullOrEmpty(ifNotNullSend)) {
								lastUse = lastRead;
								irc.SendChatMessage(ifNotNullSend);
								if(ifNotNullSend.Equals("Only in death does duty end.")) {
									flagContinue = false;
									Console.ForegroundColor = ConsoleColor.DarkRed;
									Console.SetCursorPosition(0, Console.CursorTop); //move cursor
									Console.WriteLine("\n\n\n\n\n\n\n\n\nLast read : {0}\t Messages read : {1}\t Messages answered : {2}\tLast answer : {3}", lastRead, pickMeChat.MessageRead, pickMeChat.MessageSent, lastUse);
									Console.WriteLine("It was my pleasure.");
								}
							} else {
								maybeAnswerThat = afkManager.HeAfkBro(checkString);
								if(!String.IsNullOrEmpty(maybeAnswerThat)) irc.SendChatMessage(maybeAnswerThat);
								//Console.WriteLine(ifNotNullSend);
								//Console.SetCursorPosition(0, Console.CursorTop); //move cursor
								//Console.Write("Last read : {0}\t Messages read : {1}\t Messages answered : {2}\tLast answer : {3}", lastRead, pickMeChat.MessageRead, pickMeChat.MessageSent, lastUse);
							}

						}
					}
				}
			}
			// Keep the console visible at the end
			Console.ReadLine();
		}
	}
}

