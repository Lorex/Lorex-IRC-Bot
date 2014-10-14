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

                Function.Log(msgType.MESSAGE,"Server on.");
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
                                        Function.Log(msgType.MGRCOMMAND, SayNick + " : " + SayWord);
                                        Function.PMCommand(SayNick, SayWord.TrimStart('@'));
                                        break;
                                    }
                                    else
                                    {
                                        Function.Log(msgType.COMMAND, SayNick + " : " + SayWord);
                                        Function.ParseCommand(SayNick, SayWord.TrimStart('@'));
                                        break;
                                    }
                                }
                                else
                                {
                                    Function.Log(msgType.CHAT, SayNick + " : " + SayWord);
                                    ChatModule.parseChat(SayNick, SayWord);
                                    break;
                                }
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
                Function.Log(msgType.ERROR, ex.ToString());
                Thread.Sleep(5000);
                string[] argv = { };
                Main(argv);
            }
        }

    }
}
