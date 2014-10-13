using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace IRC_Bot_Console
{
    class IrcBot
    {
        public static string SERVER = "irc.freenode.net";
        private static int PORT = 6667;
        private static string USER = "Lorex_Bot Lorex_Bot Lorex_Bot :Lorex_IRC";
        private static string NICK = "Lorex_Bot";

        public static string CHANNEL = "#ysitd";//"#oktw";
        public static StreamWriter writer;

        public static DateTime START_TIME = DateTime.Now; //count system uptime
        static void Main(string[] args)
        {
            NetworkStream stream;
            TcpClient irc;
            string input;
            StreamReader reader;

            try{
                irc = new TcpClient (SERVER,PORT);
                stream = irc.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

                // Start Ping Sender Thread
                Ping ping = new Ping();
                ping.Start();
                writer.WriteLine("NICK " + NICK);
                writer.Flush();
                writer.WriteLine("USER "+ USER);
                writer.Flush();
                writer.WriteLine("JOIN " + CHANNEL);
                writer.Flush();
                writer.WriteLine("PRIVMSG " + CHANNEL + " :Bot 連線成功。");
                writer.Flush();

                while(true)
                {
                    while((input=reader.ReadLine())!=null)
                    {
                        string[] splitInput = input.Split(new Char[] {' '});
                        if (splitInput[0] == "PING")
                        {
                            string PongReply = splitInput[1];
                            writer.WriteLine("PONG " + PongReply);
                            writer.Flush();
                            writer.WriteLine("PRIVMSG "+CHANNEL+" 我又來回應Server的Ping啦! XD");
                            writer.Flush();
                            continue;
                        }

                        switch (splitInput[1])
                        {
                         
                            case "PRIVMSG":
                                string[] splitHeader = (splitInput[0].Split(new char[] {'!'}));
                                string[] splitMsg = input.Split(new char[] {':'});
                                string SayNick = splitHeader[0].TrimStart(':');
                                string SayWord = splitInput[3].TrimStart(':');
                                string SayTarget = splitInput[2];

                                for (int i = 4; i < splitInput.Length;i++ )
                                    SayWord = SayWord + " " +splitInput[i];


                                if (SayWord.StartsWith("@"))
                                {
                                    if (SayTarget == NICK)
                                    {
                                        Console.WriteLine("[MGR COMMAND] -> " + SayNick + " : " + SayWord);
                                        PMCommand(SayNick, SayWord.TrimStart('@'));

                                    }
                                    else
                                    {
                                        Console.WriteLine("[COMMAND] -> " + SayNick + " : " + SayWord);
                                        ParseCommand(SayNick, SayWord.TrimStart('@'));

                                    }
                                }
                                else
                                    Console.WriteLine("[CHAT] -> " + SayNick + " : " + SayWord);
                                break;
                            default:
                                break;
                        }
                    }
                    writer.Close();
                    reader.Close();
                    irc.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(5000);
                string[] argv = { };
                Main(argv);
            }
        }
        static void ParseCommand(string Nick, string Command)
        {
            Command cmdClass = new Command();
            string[] cmd = Command.Split(new Char[] { ' ' });
            writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000314來自 " + Nick + " 的命令已解析。");
            writer.Flush();
            
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
                        IrcBot.writer.WriteLine("PRIVMSG " + CHANNEL + " :\u000304錯誤：參數不足，語法 @rand <最小值> <最大值>");
                        IrcBot.writer.Flush();
                        break;
                    }
                    else
                    {
                        int r1=0,r2=0;
                        r1 = Convert.ToInt32(cmd[1]);
                        r2 = Convert.ToInt32(cmd[2]);
                        int buff1 = Math.Min(r1, r2), buff2 = Math.Max(r1, r2);

                        cmdClass.rand(buff1,buff2);
                    }
                    break;
                default:
                    writer.WriteLine("PRIVMSG " + CHANNEL + " :\u0002\u000304錯誤：命令無法解析");
                    writer.Flush();
                    break;
            }
        }
        static void PMCommand(string Nick, string Command)
        {
            ManagerCommand mgcmdClass = new ManagerCommand();
            string[] cmd = Command.Split(new Char[] { ' ' });
            writer.WriteLine("PRIVMSG " + Nick + " :\u000314來自 " + Nick + " 的管理者命令已解析。");
            writer.Flush();

            switch (cmd[0])
            {
                case "exit":
                    if (cmd.Length < 2)
                    {
                        IrcBot.writer.WriteLine("PRIVMSG " + Nick + " :\u000304錯誤：請輸入密碼");
                        IrcBot.writer.Flush();
                        break;
                    }else
                    {
                        mgcmdClass.exit(Nick, cmd[1]);
                    }
                    break;
                default:
                    writer.WriteLine("PRIVMSG " + Nick + " :\u0002\u000304錯誤：命令無法解析");
                    writer.Flush();
                    break;
            }
        }
    }
}
