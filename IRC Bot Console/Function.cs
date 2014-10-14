using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class Function
    {
        public static void ParseCommand(string Nick, string Command)
        {
            CmdModule cmdClass = new CmdModule();
            string[] cmd = Command.Split(new Char[] { ' ' });
            IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000314來自 " + Nick + " 的命令已解析。");
            IrcBot.writer.Flush();

            switch (cmd[0])
            {
                case "help":
                    cmdClass.help();
                    break;
                case "time":
                    cmdClass.time();
                    break;
                case "say":
                    cmdClass.say(Nick, cmd);
                    break;
                case "me":
                    cmdClass.me(Nick, cmd);
                    break;
                case "uptime":
                    cmdClass.uptime();
                    break;
                case "rand":
                    if (cmd.Length < 3)
                    {
                        IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000304錯誤：參數不足，語法 @rand <最小值> <最大值>");
                        IrcBot.writer.Flush();
                        break;
                    }
                    else
                    {
                        try
                        {
                            int r1 = 0, r2 = 0;
                            r1 = Convert.ToInt32(cmd[1]);
                            r2 = Convert.ToInt32(cmd[2]);
                            int buff1 = Math.Min(r1, r2), buff2 = Math.Max(r1, r2);
                            cmdClass.rand(buff1, buff2);
                        }
                        catch (Exception ex)
                        {
                            Log(msgType.ERROR, ex.ToString());
                            IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000304錯誤：無效的參數");
                            IrcBot.writer.Flush();
                            break;
                        }
                    }
                    break;
                default:
                    IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u0002\u000304錯誤：命令無法解析");
                    IrcBot.writer.Flush();
                    break;
            }
        }
        public static void PMCommand(string Nick, string Command)
        {
            MngCmdModule mgcmdClass = new MngCmdModule();
            string[] cmd = Command.Split(new Char[] { ' ' });
            IrcBot.writer.WriteLine("PRIVMSG " + Nick + " :\u000314來自 " + Nick + " 的管理者命令已解析。");
            IrcBot.writer.Flush();

            switch (cmd[0])
            {
                case "exit":
                    if (cmd.Length < 2)
                    {
                        IrcBot.writer.WriteLine("PRIVMSG " + Nick + " :\u000304錯誤：請輸入密碼");
                        IrcBot.writer.Flush();
                        break;
                    }
                    else
                    {
                        mgcmdClass.exit(Nick, cmd[1]);
                    }
                    break;
                default:
                    IrcBot.writer.WriteLine("PRIVMSG " + Nick + " :\u0002\u000304錯誤：命令無法解析");
                    IrcBot.writer.Flush();
                    break;
            }
        }
        public static void Log(msgType type, string msg)
        {

            msg = msg.Insert(0, " -> ");
            switch (type)
            {
                case msgType.COMMAND:
                    msg = msg.Insert(0, "[COMMAND]");
                    break;
                case msgType.ERROR:
                    msg = msg.Insert(0, "[ERROR]");
                    break;
                case msgType.MESSAGE:
                    msg = msg.Insert(0, "[MESSAGE]");
                    break;
                case msgType.MGRCOMMAND:
                    msg = msg.Insert(0, "[MGRCOMMAND]");
                    break;
                case msgType.CHAT:
                    msg = msg.Insert(0, "[CHAT]");
                    break;
            }
            msg = msg.Insert(0, "[" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "] ");

            Console.WriteLine(msg);

        }
    }
}
