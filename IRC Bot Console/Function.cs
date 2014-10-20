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
            if (!config.shut_up)
            {
                //Function.SendServerMessage(msgType.Notify, "來自 " + Nick + " 的命令已解析。");

                CmdModule.rules rules = new CmdModule.rules();
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
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @rand <最小值> <最大值>");
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
                                Log(consoleType.Error, ex.ToString());
                                Function.SendServerMessage(msgType.Error, "無效的參數");
                                break;
                            }
                        }
                        break;
                    case "version":
                        cmdClass.version();
                        break;
                    case "g":
                        if (cmd.Length < 2)
                        {
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @g <keyword>");
                            break;
                        }
                        else
                        {
                            cmdClass.g(cmd[1]);
                            break;
                        }
                    case "shutup":
                        cmdClass.shutup();
                        break;
                    case "rules":
                        if (cmd.Length < 2)
                        {
                            Function.SendServerMessage(msgType.Error, "無效的參數，語法 @rules <add|delete|modify|list>");
                        }
                        else
                        {
                            rules.parse(cmd);
                        }
                        break;
                    
                    default:
                        Function.SendServerMessage(msgType.Error, "命令無法解析");
                        break;
                }
            }
            else if (cmd[0] == "shutup")
            {
                Function.SendServerMessage(msgType.Information, "YA!!! 又可以說話惹 wwwwwwwwww");
                config.shut_up = false;
            }
        }
        public static void PMCommand(string nick, string command)
        {
            MngCmdModule mgcmdClass = new MngCmdModule();
            string[] cmd = command.Split(new Char[] { ' ' });
            Function.SendServerMessage(msgType.Notify,"來自 " + nick + " 的管理者命令已解析。",nick);

            switch (cmd[0])
            {
                case "exit":
                    if (cmd.Length < 2)
                    {
                        Function.SendServerMessage(msgType.Error,"請輸入密碼",nick);
                        break;
                    }
                    else
                    {
                        mgcmdClass.exit(nick, cmd[1]);
                    }
                    break;
                default:
                    Function.SendServerMessage(msgType.Error,"錯誤：命令無法解析",nick);
                    break;
            }
        }
        public static void Log(consoleType type, string msg)
        {

            msg = msg.Insert(0, " -> ");
            switch (type)
            {
                case consoleType.Command:
                    msg = msg.Insert(0, "[COMMAND]");
                    break;
                case consoleType.Error:
                    msg = msg.Insert(0, "[ERROR]");
                    break;
                case consoleType.Message:
                    msg = msg.Insert(0, "[MESSAGE]");
                    break;
                case consoleType.ManagerCommand:
                    msg = msg.Insert(0, "[MGRCOMMAND]");
                    break;
                case consoleType.Chat:
                    msg = msg.Insert(0, "[CHAT]");
                    break;
            }
            msg = msg.Insert(0, "[" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "] ");

            Console.WriteLine(msg);

        }
        public static void SendServerMessage(msgType type, string msg, string nick = config.CHANNEL)
        {
            switch (type){
                case msgType.Notify:
                    IrcBot.writer.WriteLine("PRIVMSG " + nick + " :\u000314" + msg);
                    break;
                case msgType.Error:
                    IrcBot.writer.WriteLine("PRIVMSG " + nick + " :\u000304錯誤：" + msg);
                    break;
                case msgType.Information:
                    IrcBot.writer.WriteLine("PRIVMSG " + nick + " :\u000302" + msg);
                    break;
                
            }
            IrcBot.writer.Flush();
        }
    }
}
