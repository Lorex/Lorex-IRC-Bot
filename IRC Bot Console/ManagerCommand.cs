using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class ManagerCommand
    {
        string CHANNEL = IrcBot.CHANNEL;

        public void exit(string Nick, string password)
        {
            if ((Nick == "Lorex") && (password == "exit"))
            {
                IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000304警告：正在結束 Bot");
                IrcBot.writer.Flush();
                IrcBot.writer.Close();
                Environment.Exit(0);
            }
            else
            {
                IrcBot.writer.WriteLine("PRIVMSG " + Nick + " :\u000302沒權限你是在關屁關ww  (#");
                IrcBot.writer.Flush();
            }
        }
    }
}
