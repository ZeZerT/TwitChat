using System;
using System.Collections.Generic;

namespace TwitchChatBot {
	class AnswerPicker {
		// Dictionnary containing the trigger and the corresponding answer
		private Dictionary<string, Answer> answers;

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
		
		private string preCom;
		private string postCom;
		private string admin;
		private Boolean noErrorWhileAdding;
		private Boolean saidHello;

		private DateTime lastUse;

		public AnswerPicker(string precom, string postcom, string admin) {
			this.preCom  = precom;
			this.postCom = postcom;
			this.admin   = admin;
			this.answers = new Dictionary<string, Answer>();
			this.lastUse = DateTime.Now;
			this.saidHello = false;
			this.noErrorWhileAdding = true;
		}

		public void addAnswer(Boolean usesMessage, int type, string trigger, string textPleb, int withCaller = Answer.NONE_HAVE_CALLER, Boolean ignorePrePostCom =WITH_PRE_POST_COM, Boolean admin =WITHOUT_ADMIN_VERSION, string textAdmin ="", int special = SpecialAnswer.NONE) {
			Boolean keyFound = false;

			// Searching for the key (the trigger), hoping we don't find it.
			foreach(string key in answers.Keys) {
				if(key == trigger) keyFound = true;
			}

			// If it wasn't found, we can add the trigger and it's answer
			if(!keyFound) {
				// If the answer "uses the message", it's a special answer. Else, it's a normal one.
				if(usesMessage) answers.Add(trigger, new SpecialAnswer(admin, ignorePrePostCom, special));
				else answers.Add(trigger, new Answer(type, textPleb, withCaller, admin, ignorePrePostCom, textAdmin));
				//Console.WriteLine("Added : {0}, {1}, {2}, {3}, {4}, {5}, {6}", type, trigger, text, withCaller, ignorePrePostCom, admin, textAdmin);
			}else {
				// An error occured, we log it and block the answer retrieval. Adding to the answers may be blocked if we check all but the first addAnswer call by the blocker.
				Console.WriteLine("NOT Added : {0}, {1}, {2}, {3}, {4}, {5}, {6}", type, trigger, textPleb, withCaller, ignorePrePostCom, admin, textAdmin);
				noErrorWhileAdding = false;
			}
		}

		public Boolean canWeGo() {
			return noErrorWhileAdding;
		}

		public string pickAnswer(string text) {
			// The text is the complete message, including headers as who sent the message, to which channel, and of course the message itself.
			if(text != null) {
				// The message is then retrieved, as well as the caller and an enventual trigger that would have been detected. 
				string message = text.Substring(text.IndexOf(" :") + 2, text.Length - text.IndexOf(" :") - 2);
				string caller = getCaller(text);
				string trigger = getTrigger(message);

				// Debug
				//Console.WriteLine("message : \"{0}\"" + "trigger : \"{1}\"" + "caller : \"{2}\"" + " null or empty? {3}" , message, trigger, caller, (caller != null && trigger != null));

				if(caller != null && trigger != null) {
					// Do the current message match any of our answers ?
					if(Answer.Exists(answers[trigger])) {
						// Identify if we have a corresponding answer, if so, determine if it's a normal or a special one (normal : predictable string, special : unpredictable string) 

						// We found one of our triggers in the message, but does it really match out expections ? (is it relevant)
						Boolean relevant = false;

						// If the specified answer is a special one
						if(answers[trigger].GetType() == typeof(SpecialAnswer)) {
							SpecialAnswer response = (SpecialAnswer) answers[trigger];
							//Check if we are interested in this message
							relevant = response.IgnorePrePostCom 
								? (message.StartsWith(trigger) || message.StartsWith(trigger))
								: (message.StartsWith(preCom + trigger + postCom) || message.StartsWith(preCom + trigger + " " + postCom));
							response.fillAnswers(message, caller, (caller==admin));
							return tryToSend(relevant, response, caller);

							// Or if it's a normal one (answers[trigger].GetType() == typeof(Answer))
						} else {
							Answer response = answers[trigger];
							// Array containing the different parts of a trigger, if there may be several.
							string[] triggers = null;
							// Self-explanatory , we check if we are interested in this message
							switch(response.Type) {
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
									triggers = trigger.Split("--".ToCharArray());
									relevant = (message.StartsWith(triggers[0]) && message.Contains(triggers[1]));
									break;
								case CONTAINS_ENDS:
									triggers = trigger.Split("--".ToCharArray());
									relevant = (message.Contains(triggers[0]) && message.EndsWith(triggers[1]));
									break;
								case STARTS_ENDS:
									triggers = trigger.Split("--".ToCharArray());
									relevant = (message.StartsWith(triggers[0]) && message.EndsWith(triggers[1]));
									break;
								case STARTS_CONTAINS_ENDS:
									triggers = trigger.Split("--".ToCharArray());
									relevant = (message.StartsWith(triggers[0]) &&message.Contains(triggers[1]) && message.EndsWith(triggers[2]));
									break;
								default: return null;
							}

							return tryToSend(relevant, response, caller);
						}
					}
					return null;
				}
				return null;
			}
			return null;
		}

		private string tryToSend(Boolean relevant, Answer response, string caller) {
			// If so, then we check if we can send a message (30s cooldown for same messages, 1.2s for different ones (values given by Twitch chat rules))
			if(relevant && response.canResend() && DateTime.Now.Subtract(lastUse).TotalSeconds > 2) {
				// Set the "last used" variable to the current moment, print the answer and return it.
				lastUse = DateTime.Now;
				string answer = response.getAnswer(caller, (caller==admin));

				Console.ForegroundColor = ConsoleColor.White; Console.Write("\n[" + DateTime.Now.ToString("hh:mm:ss:fff") + "] " + caller + " > ");
				Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.WriteLine("Answered " + answer);
				Console.ForegroundColor = ConsoleColor.Gray;

				return answer;
			}
			return null;
		}

		// Self-explanatory
		private string getTrigger(string message) {
			foreach(string key in answers.Keys) {
				if(message.Contains(key))	return key;
			}
			return null;
		}

		// Self-explanatory
		private string getCaller(string message) {
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

		// As polite as polite can be, one who say hi must say goodbye.
		public void goodbyeMessage(IrcClient irc, string message) {
			if(saidHello) {
				irc.sendChatMessage(message);
				saidHello = !saidHello;
			}
		}

		// Self-explanatory, but never used.
		public Boolean removeAnswer(string trigger) {
			return answers.Remove(trigger);
		}

		// Self-explanatory, but never used.
		public void removeAnswers() {
			answers.Clear();
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
