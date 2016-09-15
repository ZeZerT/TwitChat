using System;

namespace TwitchChatBot{
	public static class Program {
		static string channel = "forsenlol", preCom = "", postCom = "?", admin = "zezert";
		static IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "MrZezertoid", "oauth:j0zyvyvajdg1rqrfl8b3qntyfxdhym");

		static void Main(string[] args) {
			AnswerPicker pickMeChat = new AnswerPicker(preCom, postCom, admin);
			string lastUse = DateTime.Now.ToString("hh:mm:ss:fff");
			string lastRead = DateTime.Now.ToString("hh:mm:ss:fff");
			string ifNotNullSend = "";
			Boolean flagContinue = true;

			irc.joinRoom(channel);
			pickMeChat.helloMessage(irc, "That turns me on gachiGASM");

			// Admin / pleb differenciated commands
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "slave", "you are not my master MrDestructoid",
								 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "MrDestructoid Senpai MrDestructoid noticed MrDestructoid me MrDestructoid");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "fuck you", "No, fuck YOU leatherman ! gachiGASM",
							 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "Oh yes sir ! gachiGASM");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "givepoints", "Your ID doesn't match. Please go fuck yourself <3 MrDestructoid forsenWhip",
							 Answer.PLEB_STARTS_WITH_CALLER, AnswerPicker.WITH_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "!givepoints ZeZerT all MrDestructoid");

			// Regular commands			
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "pleb", "This is a suicide test. If you're a pleb, kill yourself forsenC forsenGun");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "kappa", "This is a Kappa test. If Kappa Kappa is Kappa , then Kappa yourself");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "kids", "I have candies 🍭 PedoBear 🚚");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "dududu", "forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls forsenDisco forsenPls");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "weeb", "Keep moving, it's funnier for me forsenSheffy forsenGun nyanPls");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "beans", "I WAS EATING THOSE BEANS forsenSWA");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "lying", " I'M NOT FUCKING LYING forsenSWA YOU FUCKING CUNT forsenSWA", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "forsene", "This slave is a proud forsenE sub forsenDDK forsenWhip MrDestructoid");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "blinak", "@Blinak, Hey look at my bot MrDestructoid");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "viewbot", "I am here to serve. MrDestructoid", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "gay", "I am now gay for you. KappaPride 🌹", Answer.BOTH_STARTS_WITH_CALLER);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "old forsen", "ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ FeelsBadMan ☂ ヽ｀ヽ｀、ヽヽ｀ヽ｀、｀ヽ｀、ヽヽ｀ヽ｀、ヽヽ｀ヽ、ヽヽ｀ヽ '");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "can on head", "AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ AMPTropPunch ㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤ 4Head");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "brain power", "/me forsenPls O-oooooooooo AAAAE-A-A-I-A-U- JO-oooooooooooo AAE-O-A-A-U-U-A- E-eee-ee-eee AAAAE-A-E-I-E-A- JO-ooo-oo-oo-oo EEEEO-A-AAA-AAAA forsenPls");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "cooldown", "Engine is now cooling down. MrDestructoid");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "gazatu", "He's my favorite bot owner nyanPls after Blinak of course nyanPls and ZeZerT MrDestructoid");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "zezert", "ZeZerT sends his regards MrDestructoid forsenKnife");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "finger my ass", "(‿ˠ‿) ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ 👆");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "chromosome", "Oh no, I dropped my MONSTER chromosome ⎬⎨ that I use it for my MAGNUM autism forsenE");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "hands up", "forsenSheffy forsenGun ⎝⎞ TriHard ⎛⎠ I DINDU MUFFIN OFCR");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "911", "ANELE ✈ █ ✈ █ KAPOW");       
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "ants", "🐜 ANTS 🐜 WutFace 🐜🐜🐜 RUN FOR 🐜 YOUR LIFE 🐜🐜 ᕕ SwiftRage ᕗ");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "holiday", "☀️ 🏖 🌴 MrDestructoid  Feels good to be on holiday");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "oceanman", "KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan KKoceanMan ᴛᴀᴋᴇ ᴍᴇ ʙy ᴛʜᴇ ʜᴀɴᴅ KKoceanMan KKoceanMan KKoceanMan ʟᴇᴀᴅ ᴍᴇ ᴛᴏ ᴛʜᴇ ʟᴀɴᴅ ᴛʜᴀᴛ yᴏᴜ ᴜɴᴅᴇʀSᴛᴀɴᴅ KKoceanMan ᴏᴄᴇᴀɴᴍᴀɴ KKoceanMan KKoceanMan");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "eu", "Europe was discovered in 1849 when Chuck Norris rode his horse over the Atlantic Ocean. Upon discovering land there, he named the discovery Eastern USA, which today is abbreviated as EU.");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "leak", "I can fix this 🔧 forsenE");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "bean", "I WAS EATING THIS BEAN forsenSWA");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "party", "💃🏃💃🚶💃🏃🏃🚶💃🏃🏃🚶🚶💃💃🚶🏃💃🏃🏃🚶💃🏃🚶💃🏃🚶💃🏃🏃🚶🏃🏃");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "suicide", "This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage This is how real bot suicide SwiftRage ");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "racism on twitch", "Guess which skin color is banned on twitch. Don't know yet? Here is a tip : Red, Yellow, Green, Blue, TriHard , White");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "forsenGASM", "We are legion OhGod We do not forgive OhGod We do not forget OhGod We are gachinimous OhGod Expect us OhGod");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "megalul", "DEAD MEME LUL https://www.google.com/trends/explore?date=all&q=megalul");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "dungeon", "[ KKaper KKaper ] Look what I have in my dungeon forsenDDK");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "entertained", "ARE YOU NOT ENTERTAINED ? SwiftRage 🗡");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "memer of the day", "http://i.imgur.com/dbYwyIS.png PogChamp");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "billy", "Some religions call him \"God\", for others it's \"Allah\", for me it's just \"Billy\".");
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "savjz", "I save my jizz for Savjz forsenLewd");
			
			// Irregular commands
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "!offlinechat", "/me JAVLA FITTA KUK HELVETE", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "!enation", "/me DO NATION 👌💰", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "!genji", "There is no way I add that weeb overwatch emote forsenSWA", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.addAnswer(false, AnswerPicker.CONTAINS_ENDS, "mrzezertoid--?", "Idk kev forsenE", Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.addAnswer(false, AnswerPicker.CONTAINS, "im not fucking lying !", "YES YOU ARE BabyRage", Answer.BOTH_STARTS_WITH_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM);
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_ENDS, "?--?", "🤔 Hum yeah I'm not sure, I think we need more samples. 🤔");

			// Special commands
			pickMeChat.addAnswer(true, AnswerPicker.SPECIAL, "randomize those words", "", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.RANDOMIZE_WORD);
			pickMeChat.addAnswer(true, AnswerPicker.SPECIAL, "randomize this", "", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.RANDOMIZE_CHAR);
			pickMeChat.addAnswer(true, AnswerPicker.SPECIAL, "where is dad", "", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.WHEREISDAD);
			pickMeChat.addAnswer(true, AnswerPicker.SPECIAL, "where is mom", "", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, false, "", Answer.WHEREISMOM);
			//Console.WriteLine("Triggers are : " + pickMeChat.getAnswers());
	
			// End the bot
			pickMeChat.addAnswer(false, AnswerPicker.STARTS_WITH, "Death is the only answer.", "Duty prevails.", Answer.NONE_HAVE_CALLER, AnswerPicker.WITHOUT_PRE_POST_COM, AnswerPicker.WITH_ADMIN_VERSION, "Only in death does duty end.");
			
			string checkString = "";
			int skipThem = 9;

			if(pickMeChat.canWeGo()) {
				while(flagContinue) {
					checkString = irc.readMessage();
					//Console.WriteLine("\""+checkString+"\"");
					if(checkString.Equals("PING: tmi.twitch.tv") || checkString.Equals("PING :tmi.twitch.tv")) {
						irc.refresh(false);
						skipThem = 4;
					} else {
						if(skipThem >0) skipThem--;
						else {
							ifNotNullSend = pickMeChat.pickAnswer(checkString);
							lastRead = DateTime.Now.ToString("hh:mm:ss:fff");

							if(ifNotNullSend != null) {
								lastUse = lastRead;
								irc.sendChatMessage(ifNotNullSend);
							}
							if(ifNotNullSend == "Only in death does duty end.") {
								flagContinue = false;
								Console.ForegroundColor = ConsoleColor.DarkRed;
								Console.SetCursorPosition(0, Console.CursorTop); //move cursor
								Console.WriteLine("\n\n\n\n\n\n\n\n\nLast read : {0}\t Messages read : {1}\t Messages answered : {1}\tLast answer : {3}", lastRead, pickMeChat.MessageRead, pickMeChat.MessageSent, lastUse);
								Console.WriteLine("It was my pleasure.");

							} else {
								//Console.WriteLine(ifNotNullSend);
								Console.SetCursorPosition(0, Console.CursorTop); //move cursor
								Console.Write("Last read : {0}\t Messages read : {1}\t Messages answered : {1}\tLast answer : {3}", lastRead, pickMeChat.MessageRead, pickMeChat.MessageSent, lastUse);
							}

						}
					}
				}
			}
			// Keep the console visible
			Console.ReadLine();
		}
	}
}
