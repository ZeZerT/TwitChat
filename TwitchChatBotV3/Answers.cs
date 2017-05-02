using System;

namespace TwitchChatBotV3 {
	class Answer : IRCMessage {
		new private int type;
		private int withCaller;
		private Boolean admin;
		private Boolean force;
		private Boolean ignorePrePostCom;

		private DateTime usedOn;
		private Boolean exists = false;

		// Types
		public const int NONE_HAVE_CALLER           = 20;
		public const int PLEB_STARTS_WITH_CALLER    = 21;
		public const int PLEB_ENDS_WITH_CALLER      = 22;
		public const int ADMIN_STARTS_WITH_CALLER   = 23;
		public const int ADMIN_ENDS_WITH_CALLER     = 24;
		public const int BOTH_STARTS_WITH_CALLER    = 25;
		public const int BOTH_ENDS_WITH_CALLER      = 26;

		public Answer(int type, string message, int withCaller, Boolean admin, Boolean ignorePrePostCom) : base(message, TwitchClient.botName, TwitchClient.channel){
			exists = true;

			Type = type;
			Admin = admin;
			Message = message;
			WithCaller = withCaller;
			IgnorePrePostCom = ignorePrePostCom;
		}

		static public Boolean Exists(Answer checkme) {
			return checkme.exists;
		}

		public void getAnswer(string caller, Boolean admin) {
			string start_call = "@" + caller;
			string end_call = caller + " .";
			UsedOn = DateTime.Now;

			if(!String.IsNullOrEmpty(Message))
				switch(withCaller) {
					case NONE_HAVE_CALLER:
						Message = admin ? Message : Message;
						break;
					case PLEB_STARTS_WITH_CALLER:
						Message = admin ? Message : start_call + Message;
						break;
					case PLEB_ENDS_WITH_CALLER:
						Message = admin ? Message : Message + end_call;
						break;
					case ADMIN_STARTS_WITH_CALLER:
						Message = admin ? start_call + Message : Message;
						break;
					case ADMIN_ENDS_WITH_CALLER:
						Message = admin ? Message + end_call : Message;
						break;
					case BOTH_STARTS_WITH_CALLER:
						Message = admin ? start_call + Message : start_call + Message;
						break;
					case BOTH_ENDS_WITH_CALLER:
						Message = admin ? Message + end_call : Message + end_call;
						break;
					default:
						Message = null;
						break;
				}
		}
		
		public void forceResend(Boolean forceMeMaybe) {
			force = forceMeMaybe;
		}

		public Boolean canResend() {
			Boolean returnMe = force || DateTime.Now.Subtract(UsedOn).TotalSeconds > 30;
			force = false;
			return returnMe;
		}

		public override string ToString() {
			return "type="+Type+" Message="+Message+" withCaller="+WithCaller+" admin="+Admin+" ignorePrePostCom="+IgnorePrePostCom;
		}
		
		public Int32 WithCaller {
			get { return withCaller; }
			set { this.withCaller=value; }
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