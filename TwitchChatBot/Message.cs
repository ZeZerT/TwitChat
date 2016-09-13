using System;

namespace TwitchChatBot {
	class Message {
		private string text;
		private string caller;
		private DateTime date;
		private int length;

		public String Text {
			get { return text; }
			set { this.text=value; }
		}
		public String Caller {
			get { return caller; }
			set { this.caller=value; }
		}
		public DateTime Date {
			get { return date; }
			set { this.date=value; }
		}
		public Int32 Length {
			get { return length; }
			set { this.length=value; }
		}

		public Message(string text, string caller, DateTime date, int length) {
			Text = text;
			Caller = caller;
			Date = date;
			Length = length;
		}
	}
}
