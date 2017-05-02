
namespace TwitchChatBotV3 {
    class TwitchClient {
        // One for each channel the bot is in
        public static string channel = "zezert", preCom = "", postCom = "?", admin = "zezert", botName = "MrZezertoid", botCredentials = "oauth:j0zyvyvajdg1rqrfl8b3qntyfxdhym";
        static IrcClient irc = new IrcClient("irc.twitch.tv", 6667, botName, botCredentials);
    }
}