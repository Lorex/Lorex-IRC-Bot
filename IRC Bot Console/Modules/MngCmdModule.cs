using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class MngCmdModule
    {
        public void exit(string Nick, string password)
        {
            if ((Nick == "Lorex") && (password == "exit"))
            {
                Function.SendServerMessage(msgType.Information, "\u000304警告：正在結束 Bot （管理者： " + Nick + "）");

                IrcBot.writer.WriteLine("PART " + config.CHANNEL);
                IrcBot.writer.Flush();
                IrcBot.writer.WriteLine("QUIT");
                IrcBot.writer.Flush();
                IrcBot.connection = false;
            }
            else
            {
                Function.SendServerMessage(msgType.Information, "\u000302沒權限你是在關屁關ww  (#",Nick);
            }
        }
    }
    
}
