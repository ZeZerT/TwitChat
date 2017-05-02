using System;

//Example pleb : "@badges=;color=#2E8B57;display-name=WWWWWWWWWWWWMEMEWWWWWWWWW;emotes=114836:10-17;id=e70c3131-ea5c-4c11-a0c8-8d31766909c7;mod=0;room-id=22484632;sent-ts=1474782460521;subscriber=0;tmi-sent-ts=1474782460572;turbo=0;user-id=131915612;user-type= :wwwwwwwwwwwwmemewwwwwwwww!wwwwwwwwwwwwmemewwwwwwwww@wwwwwwwwwwwwmemewwwwwwwww.tmi.twitch.tv PRIVMSG #forsenlol :gachiGASm Jebaited"
//Example sub  : "@badges=subscriber/1;color=#1E90FF;display-name=EpicMango7;emotes=93064:71-77;id=106f6c90-1a68-42ff-993b-fbf24e13c981;mod=0;room-id=22484632;subscriber=1;tmi-sent-ts=1474782431493;turbo=0;user-id=76040250;user-type= :epicmango7!epicmango7@epicmango7.tmi.twitch.tv PRIVMSG #forsenlol :put on cancer lottery now so i can have a better chance of winning pls forsenE"
//Example mod  : "@badges=moderator/1,subscriber/1;color=#12AFED;display-name=Snusbot;emotes=;id=dbcb04da-6e7a-4e09-b367-ae8155fb2257;mod=1;room-id=22484632;subscriber=1;tmi-sent-ts=1474783602489;turbo=0;user-id=62541963;user-type=mod :snusbot!snusbot@snusbot.tmi.twitch.tv PRIVMSG #forsenlol :Looking for a Pleb Free Zone? Join other subscribers on Discord by typing !discord in chat and following the instructions."

namespace TwitchChatBotV3 {
	class IRCMessage {
		// Types
		public const int PRIVMSG	= 01;
		public const int PING		= 02;
		public const int JOIN       = 03;
		public const int PART       = 04;
		protected string text;			public String Text		{ get { return text; }		set { this.text=value;		} }
		protected string caller;		public String Caller	{ get { return caller; }	set { this.caller=value;	} }
		protected string message;		public String Message	{ get { return message; }	set { this.message=value;	} }
		protected string channel;		public String Channel	{ get { return channel; }	set { this.channel=value;	} }
		protected int length;			public Int32 Length		{ get { return length; }	set { this.length=value;	} }
		protected int type;				public Int32 Type		{ get { return type; }		set { this.type=value;		} }

		public IRCMessage(string text) {
            string interestingPart = text.Remove(0, text.IndexOf('@', text.IndexOf('@') + 1)+1);
            // Example pleb : interestingMessage = "wwwwwwwwwwwwmemewwwwwwwww.tmi.twitch.tv PRIVMSG #forsenlol :gachiGASm Jebaited";
            if(text.Contains("PRIVMSG")){
				Text = text;
				Caller = getCallerFromText(interestingPart);
				Message = getMessageFromText(interestingPart);
				Channel = getChannelFromText(interestingPart);
				length = message.Length;
				Type = PRIVMSG;
			} else if(text.StartsWith("PING")) {
				Type = PING;
			} else if(text.StartsWith("JOIN")) {
                Type = PING;
            } else if(text.StartsWith("PART")) {
                Type = PING;
            }
        }
		public IRCMessage(string message, string caller, string channel) {
			Text = ":"+caller+"!"+caller+"@"+caller+".tmi.twitch.tv PRIVMSG #"+channel+" :"+message;
			Caller = caller;
			Message = message;
			Channel = channel;
			length=message.Length;
		}

		public string ToStringformated() {
			return Message;
		}

		public override string ToString() {
			return Text;
		}

		public static string getMessageFromText(string text) {
            //return text?.Substring(text.IndexOf(" :")+2, text.Length - text.IndexOf(" :")-2);
            return text?.Substring(text.IndexOf(":")+1, text.Length - text.IndexOf(":")-1);
        }

		public static string getCallerFromText(string text) {
            //return text?.Substring(1, text.IndexOf("!") - 1);
            return text?.Substring(0, text.IndexOf(".tmi.twitch.tv"));
        }

		public static string getChannelFromText(string text) {
            //return text?.Substring(text.IndexOf("#")+1, text.IndexOf(" :", 2)-text.IndexOf("#"));
            return text?.Substring(text.IndexOf("#")+1, text.IndexOf(":")-1 - text.IndexOf("#"));
        }
        
        public static string getViewerFromText(string text) {
            return text?.Substring(text.IndexOf("!")+1, text.IndexOf("@")-1 - text.IndexOf("!"));
        }
    }
}
