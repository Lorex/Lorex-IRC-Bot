using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class config
    {
        public const string SERVER = "irc.freenode.net";
        public const int PORT = 6667;
        public const string USER = "Lorex_Bot Lorex_Bot Lorex_Bot :Lorex_IRC";
        public const string NICK = "Lorex_Bot";

        public const string CHANNEL = "#oktw";
        public static bool shut_up = false; 
    }
    public enum consoleType
    {
        Message,
        Error,
        Command,
        ManagerCommand,
        Chat
    }
    public enum msgType
    {
        Notify,
        Information,
        Error
    }
}
