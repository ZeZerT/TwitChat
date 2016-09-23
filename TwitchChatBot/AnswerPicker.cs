using System;
using System.Collections.Generic;

namespace TwitchChatBot {
	class AnswerPicker {
		// Dictionnary containing the trigger and the corresponding answer
		private Dictionary<string, Answer> answers;
		public const Boolean DEBUG = false;
		
		// Constants used for explicit use of the class
		public const int STARTS_WITH				= 10;
		public const int CONTAINS					= 11;
		public const int ENDS_WITH					= 12;
		public const int STARTS_CONTAINS			= 13;
		public const int CONTAINS_ENDS				= 14;
		public const int STARTS_ENDS				= 15;
		public const int STARTS_CONTAINS_ENDS		= 16;
		public const int SPECIAL			        = 17;

		public const Boolean WITHOUT_PRE_POST_COM    = true;
		public const Boolean WITH_PRE_POST_COM      = false;

		public const Boolean WITH_ADMIN_VERSION     = true;
		public const Boolean WITHOUT_ADMIN_VERSION  = false;

		// Knowing the total of messages sent and read. Is also used to counter the 30s cooldown if the bot sends another message.
		private static String lastMessage;
		private long messageSent;
		private long messageRead;

		private string preCom;
		private string postCom;
		private string admin;
		private Boolean noErrorWhileAdding;
		private Boolean saidHello;

		private DateTime lastUse;

		public AnswerPicker(string precom, string postcom, string admin) {
			this.answers = new Dictionary<string, Answer>();
			this.noErrorWhileAdding = true;
			this.lastUse = DateTime.Now;
			this.preCom  = precom;
			this.postCom = postcom;
			this.admin   = admin;
			this.saidHello = false;
		}

		public void addAnswer(int type, string trigger, string textPleb, int withCaller = Answer.NONE_HAVE_CALLER, Boolean ignorePrePostCom =WITH_PRE_POST_COM, Boolean admin =WITHOUT_ADMIN_VERSION, string textAdmin ="", int special = Answer.NONE) {
			Boolean keyFound = false;
			trigger = trigger.ToLower();
			// Searching for the key (the trigger), hoping we don't find it.
			foreach(string key in answers.Keys) {
				if(key == trigger) keyFound = true;
			}

			// If it wasn't found, we can add the trigger and it's answer
			if(!keyFound) {
				answers.Add(trigger, new Answer(type, textPleb, withCaller, admin, ignorePrePostCom, textAdmin, special));
				//Console.WriteLine("Added : {0}, {1}, {2}, {3}, {4}, {5}, {6}", type, trigger, text, withCaller, ignorePrePostCom, admin, textAdmin);
			}else {
				// An error occured, we log it and block the answer retrieval. Adding to the answers may be blocked if we check all but the first addAnswer call by the blocker.
				Console.WriteLine("NOT Added : {0}, {1}, {2}, {3}, {4}, {5}, {6}", type, trigger, textPleb, withCaller, ignorePrePostCom, admin, textAdmin);
				noErrorWhileAdding = false;
			}
			debug("usesMessage, type, trigger, textPleb, withCaller, ignorePrePostCom, admin, textAdmin, special, keyFound", type, trigger, textPleb, withCaller, ignorePrePostCom, admin, textAdmin, special, keyFound);
		}

		public Boolean canWeGo() {
			return noErrorWhileAdding;
		}

		public string pickAnswer(string text) {
			// The text is the complete message, including headers as who sent the message, to which channel, and of course the message itself.
			if(text != null) {
				// The message is then retrieved, as well as the caller and an enventual trigger that would have been detected. 
				
				string message = text.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2).ToLower();
				string trigger = getTrigger(message);
				string caller = getCaller(text);
				incrementRead();

				if(caller != null && trigger != null) {
					// Do the current message match any of our answers ?
					if(Answer.Exists(answers[trigger])) {
						// Identify if we have a corresponding answer, if so, determine if it's a normal or a special one (normal : predictable string, special : unpredictable string) 

						// We found one of our triggers in the message, but does it really match out expections ? (is it relevant)
						Boolean relevant = false;
						
						Answer response = answers[trigger];
						// Array containing the different parts of a trigger, if there may be several.
						string[] triggers = null;
						// Self-explanatory , we check if we are interested in this message
						switch(response.Type) {
							case SPECIAL:
							case STARTS_WITH:
								relevant = response.IgnorePrePostCom
									? (message.StartsWith(trigger) || message.StartsWith(trigger))
									: (message.StartsWith(preCom + trigger + postCom) || message.StartsWith(preCom + trigger + " " + postCom));
								break;
							case CONTAINS:
								relevant = message.Contains(trigger);
								break;
							case ENDS_WITH:
								relevant = message.EndsWith(trigger);
								break;
							case STARTS_CONTAINS:
								triggers = trigger.Split("¤".ToCharArray());
								relevant = (message.StartsWith(triggers[0]) && message.Contains(triggers[1]));
								break;
							case CONTAINS_ENDS:
								triggers = trigger.Split("¤".ToCharArray());
								relevant = (message.Contains(triggers[0]) && message.EndsWith(triggers[1]));
								Console.WriteLine("CONTAINS_END, relevant : " + relevant + " triggers : ");
								foreach(string printme in triggers) {
									Console.WriteLine(printme);
								}
								break;
							case STARTS_ENDS:
								triggers = trigger.Split("¤".ToCharArray());
								relevant = (message.StartsWith(triggers[0]) && message.EndsWith(triggers[1]));
								break;
							case STARTS_CONTAINS_ENDS:
								triggers = trigger.Split("¤".ToCharArray());
								relevant = (message.StartsWith(triggers[0]) &&message.Contains(triggers[1]) && message.EndsWith(triggers[2]));
								break;
							default: return null;
						}

						if(response.isSpecial()) response.fillAnswers(text, caller, (caller==admin));
						
						lastMessage = trigger;
						debug("text, message, trigger, caller, response.Type, response.isSpecial()", text, message, trigger, caller, response.Type, response.isSpecial());
						return tryToSend(relevant, response, caller, lastMessage != trigger);
					}
					return null;
				}
				return null;
			}
			return null;
		}

		private string tryToSend(Boolean relevant, Answer response, string caller, Boolean force) {
			// If so, then we check if we can send a message (30s cooldown for same messages, 1.2s for different ones (values given by Twitch chat rules))
			if(relevant && response.canResend() && DateTime.Now.Subtract(lastUse).TotalSeconds > 2) {
				// Set the "last used" variable to the current moment, print the answer and return it.
				lastUse = DateTime.Now;
				string answer = response.getAnswer(caller, (caller==admin));
				if(!String.IsNullOrEmpty(answer)){
					incrementSent();

					// Allows to send the same message before the 30 required seconds if a different message was sent inbetween.
					answers[lastMessage].forceResend(force);

					Console.ForegroundColor = ConsoleColor.White; Console.Write("\n[" + DateTime.Now.ToString("hh:mm:ss:fff") + "] " + caller + " > ");
					Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.WriteLine(answer);
					Console.ForegroundColor = ConsoleColor.Gray;
					debug("relevant, response, caller, force, answer", relevant, response, caller, force, answer);
					return answer;
				}
				return null;
			}
			return null;
		}
		//keyFound = ((message.Contains(key)) && (keyFound == null) && (answers[keyFound].Type<answers[key].Type) && (answers[keyFound].Type != answers[key].Type)) ? key : keyFound;


		// Self-explanatory
		private string getTrigger(string message) {
			message = message.ToLower();
			string keyFound = null;
			foreach(string key in answers.Keys) {
				if(message.Contains(key)) {
					if(keyFound != null) keyFound = (answers[keyFound].Type < answers[key].Type) && (answers[keyFound].Type != answers[key].Type) ? key : keyFound;
					else keyFound = key;
				}
			}
			return keyFound;
		}

		// Self-explanatory
		public static string getCaller(string message) {
			if(message.Contains("!")) return message.Substring(1, message.IndexOf("!") - 1);
			else return null;
		}

		// The first message the bot says when he's turned on. Because he is a polite and nice guy. He only says that once.
		public void helloMessage(IrcClient irc, string message) {
			if(!saidHello) {
				irc.sendChatMessage(message);
				saidHello = !saidHello;
			}
		}

		// Self-explanatory, but never used.
		public Boolean removeAnswer(string trigger) {
			return answers.Remove(trigger);
		}

		// Self-explanatory, but never used.
		private void removeAnswers() {
			answers.Clear();
		}

		// Self-explanatory, made for clarity.
		private void incrementRead() {
			MessageRead++;
		}

		// Self-explanatory, made for clarity.
		private void incrementSent() {
			MessageSent++;
		}

		// get & set
		internal static string LastMessage {
			get { return lastMessage; }
			set { lastMessage=value; }
		}

		public Int64 MessageSent {
			get { return messageSent; }
			set { this.messageSent=value; }
		}

		public Int64 MessageRead {
			get { return messageRead; }
			set { this.messageRead=value; }
		}

		public static void debug(string argNames, params object[] args) {
			if(DEBUG) {
				string[] argName = argNames.Split(",".ToCharArray());
				string showMe = "\nDEBUG -- ";
				for(int i=0; i<argName.Length; i++)	showMe += argName[i]+ "=" + args[i];
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(showMe);
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}
	}
}


//Functions's junkyard. There is no use for them at the moment, but maybe one day... Only Admin knows.

/*
public string getAnswers(){
	string returnMe = "";
	foreach(string key in answers.Keys) {
		returnMe += " \n " + key;
	}
	return returnMe;
} 

public Boolean changeAnswer(string trigger, string text, int type, Boolean admin = false) {
	Boolean keyFound = false;
	foreach(string key in answers.Keys) {
		if(key == trigger) if(answers["trigger"].Admin == admin) keyFound = true;
	}

	if(keyFound) {
		answers[trigger] = new Answer(type, text, admin);
		return true;
	}
	return false;
}*/
