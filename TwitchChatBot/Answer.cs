using System;
using System.Collections.Generic;

namespace TwitchChatBot {
	class Answer {
		private int type;
		private int count;
		private int withCaller;
		private string textAdmin;
		private string textPleb;
		private Boolean ignorePrePostCom;
		private Boolean admin;

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

		public Answer() {
			exists = false;
		}
		
		public Answer(int type, string text, int withCaller, Boolean admin, Boolean ignorePrePostCom, string textAdmin) {
			last5 = new List<string>();
			exists = true;

			Count = 0;
			Type = type;
			Admin = admin;
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

		public string getLastCaller() {
			List<string> names = last5.GetRange(Count -1, 1);
			return String.Join(", ", names.ToArray());
		}

		public string getLastCallers(int number) {
			if(Count -1 -number <0) number = Count -1;
			List<string> names = last5.GetRange(Count -1 -number, number +1);
			return String.Join(", ", names.ToArray());
		}

		public Boolean canResend() {
			return DateTime.Now.Subtract(UsedOn).TotalSeconds > 30;
		}

		public Int32 Type {
			get { return type; }
			set { this.type=value; }
		}

		public Int32 WithCaller {
			get { return withCaller; }
			set { this.withCaller=value; }
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
