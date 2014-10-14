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
        public static StreamWriter writer;
        public static DateTime START_TIME = DateTime.Now; //count system uptime
        public  enum  msgType {
            MESSAGE,
            ERROR,
            COMMAND,
            MGRCOMMAND,
            CHAT
        }
        static void Main(string[] args)
        {
            NetworkStream stream;
            TcpClient irc;
            string input;
            StreamReader reader;

            try{
                irc = new TcpClient(config.SERVER, config.PORT);
                stream = irc.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

                // Start Ping Sender Thread
                Ping ping = new Ping();
                ping.Start();
                writer.WriteLine("NICK " + config.NICK);
                writer.Flush();
                writer.WriteLine("USER " + config.USER);
                writer.Flush();
                writer.WriteLine("JOIN " + config.CHANNEL);
                writer.Flush();
                writer.WriteLine("PRIVMSG " + config.CHANNEL + " :Bot 連線成功。");
                writer.Flush();

                Log(msgType.MESSAGE,"Server on.");
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
                                    if (SayTarget == config.NICK)
                                    {
                                        Log(msgType.MGRCOMMAND, SayNick + " : " + SayWord);
                                        PMCommand(SayNick, SayWord.TrimStart('@'));

                                    }
                                    else
                                    {
                                        Log(msgType.COMMAND, SayNick + " : " + SayWord);
                                        ParseCommand(SayNick, SayWord.TrimStart('@'));

                                    }
                                }
                                else
                                    Log(msgType.CHAT, SayNick + " : " + SayWord);
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
            catch (Exception ex)
            {
                Log(msgType.ERROR, ex.ToString());
                Thread.Sleep(5000);
                string[] argv = { };
                Main(argv);
            }
        }
        static void ParseCommand(string Nick, string Command)
        {
            CmdModule cmdClass = new CmdModule();
            string[] cmd = Command.Split(new Char[] { ' ' });
            writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000314來自 " + Nick + " 的命令已解析。");
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
                            Log(msgType.ERROR,ex.ToString());
                            writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u000304錯誤：無效的參數");
                            writer.Flush();
                            break;
                        }
                    }
                    break;
                default:
                    writer.WriteLine("PRIVMSG " + config.CHANNEL + " :\u0002\u000304錯誤：命令無法解析");
                    writer.Flush();
                    break;
            }
        }
        static void PMCommand(string Nick, string Command)
        {
            MngCmdModule mgcmdClass = new MngCmdModule();
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
        public static void Log(msgType type, string msg)
        {

            msg = msg.Insert(0, " -> ");
            switch(type)
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
            }
            msg = msg.Insert(0, "[" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "] ");

            Console.WriteLine(msg);
            
        }
    }
}
