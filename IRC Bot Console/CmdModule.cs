using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class CmdModule
    {
        string CHANNEL = config.CHANNEL;
        public void say(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000304錯誤：參數不足，語法 @say <message>");
                IrcBot.writer.Flush();
            }
            else
            {
                IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000304" + Nick + " \u000302表示 \u000304" + cmd[1]);
                IrcBot.writer.Flush();

            }
        }
        public void time()
        {
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302現在時間 " + DateTime.Now);
            IrcBot.writer.Flush();
        }
        public void help() 
        {
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302指令列表 ");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302==================== ");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@time - 顯示當前時間 ");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@help - 顯示指令列表 ");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@exit - 關閉 Bot [管理指令]");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@say - 說話");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@me - 在對話前加入自己的名字");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@uptime - 顯示 Bot 上線時間");
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302@rand - 產生亂數");

            IrcBot.writer.Flush();
        }
        public void me(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000304錯誤：參數不足，語法 @me <message>");
                IrcBot.writer.Flush();
            }
            else
            {
                IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000304" + Nick + " \u000304" + cmd[1]);
                IrcBot.writer.Flush();
            }
        }
        public void uptime()
        {
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302Bot 上線時間 " + (((TimeSpan)(DateTime.Now-IrcBot.START_TIME)).Days) + " 日 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Hours) + " 時 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Minutes) + " 分 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Seconds) + " 秒 "
                );
            IrcBot.writer.Flush();
        }
        public void rand(int r1,int r2)
        {
            Random Counter = new Random(Guid.NewGuid().GetHashCode());
            IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000302亂數： " + Counter.Next(r1, r2));
            IrcBot.writer.Flush();
        }
    }
}
