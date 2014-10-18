using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace IRC_Bot_Console
{
    class CmdModule
    {
        public void say(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                Function.SendServerMessage(msgType.Error,"參數不足，語法 @say <message>");
            }
            else
            {
                Function.SendServerMessage(msgType.Information,"\u000304" + Nick + " \u000302表示 \u000304" + cmd[1]);

            }
        }
        public void time()
        {
            Function.SendServerMessage(msgType.Information,"現在時間 " + DateTime.Now);
        }
        public void help() 
        {
            Function.SendServerMessage(msgType.Information,"指令列表 ");
            Function.SendServerMessage(msgType.Information,"==================== ");
            Function.SendServerMessage(msgType.Information,"@time - 顯示當前時間 ");
            Function.SendServerMessage(msgType.Information,"@help - 顯示指令列表 ");
            Function.SendServerMessage(msgType.Information,"@exit - 關閉 Bot [管理指令]");
            Function.SendServerMessage(msgType.Information,"@say - 說話");
            Function.SendServerMessage(msgType.Information,"@me - 在對話前加入自己的名字");
            Function.SendServerMessage(msgType.Information,"@uptime - 顯示 Bot 上線時間");
            Function.SendServerMessage(msgType.Information, "@version - 顯示 Bot 版本資訊");
            Function.SendServerMessage(msgType.Information, "@rand - 產生亂數");
            Function.SendServerMessage(msgType.Information, "@g - 快速搜尋");
            Function.SendServerMessage(msgType.Information, "@shutup - 覺得 Bot 太吵的時候可以叫他閉嘴XD");
            
        }
        public void me(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                Function.SendServerMessage(msgType.Error,"參數不足，語法 @me <message>");
            }
            else
            {
                Function.SendServerMessage(msgType.Information, "\u000304" + Nick + " " + cmd[1]);
            }
        }
        public void uptime()
        {
            Function.SendServerMessage(msgType.Information,"Bot 上線時間 " + (((TimeSpan)(DateTime.Now-IrcBot.START_TIME)).Days) + " 日 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Hours) + " 時 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Minutes) + " 分 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Seconds) + " 秒 "
                );
        }
        public void rand(int r1,int r2)
        {
            Random Counter = new Random(Guid.NewGuid().GetHashCode());
            Function.SendServerMessage(msgType.Information,"亂數： " + Counter.Next(r1, r2));
        }
        public  void version()
        {
            Function.SendServerMessage(msgType.Information, "Lorex IRC Bot v." + Assembly.GetEntryAssembly().GetName().Version.ToString());

        }
        public void g(string keyword)
        {
            if (keyword == "Lorex")
            {
                Function.SendServerMessage(msgType.Information, "就帥哥一個R，哪裡還需要 Google...ˊ_>ˋ");
            }
            else
            {
                //Function.SendServerMessage(msgType.Information, "https://www.google.com.tw/#q=" + keyword);
                Function.SendServerMessage(msgType.Information, "這麼簡單的事情直接叫 oktw 來做就好了，叫我幹嘛？ˊ_>ˋ");
                IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :.g " + keyword);
                IrcBot.writer.Flush();
                Thread.Sleep(1000);
                Function.SendServerMessage(msgType.Information, "恩，好乖 <(￣︶￣)> ");

            }
        }
        public void shutup()
        {
            Function.SendServerMessage(msgType.Information, "好啦好啦好啦我閉嘴就是w");
            config.shut_up = true;
        }
    }
}
