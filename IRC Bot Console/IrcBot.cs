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
        public static bool connection = true;
        static public void Main(string[] args)
        {
            NetworkStream stream;
            TcpClient irc;
            string input;
            StreamReader reader;;

            try
            {
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

                Function.SendServerMessage(msgType.Notify, "Bot 連線成功");
                Function.Log(consoleType.Message, "Server on.");
                connection = true;
                while (connection == true)
                {
                    while (connection && ((input = reader.ReadLine()) != null))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
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
                                string[] splitHeader = (splitInput[0].Split(new char[] { '!' }));
                                string[] splitMsg = input.Split(new char[] { ':' });
                                string SayNick = splitHeader[0].TrimStart(':');
                                string SayWord = splitInput[3].TrimStart(':');
                                string SayTarget = splitInput[2];

                                for (int i = 4; i < splitInput.Length; i++)
                                    SayWord = SayWord + " " + splitInput[i]; 


                                if (SayWord.StartsWith("@"))
                                {
                                    if (SayTarget == config.NICK)
                                    {
                                        Function.Log(consoleType.ManagerCommand, SayNick + " : " + SayWord);
                                        Function.PMCommand(SayNick, SayWord.TrimStart('@'));
                                        break;
                                    }
                                    else
                                    {
                                        Function.Log(consoleType.Command, SayNick + " : " + SayWord);
                                        Function.ParseCommand(SayNick, SayWord.TrimStart('@'));
                                        break;
                                    }
                                }
                                else
                                {
                                    Function.Log(consoleType.Chat, SayNick + " : " + SayWord);
                                    ChatModule.parseChat(SayNick, SayWord);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    connection = false;
                    stream.Close();
                    irc.Client.Close();
                    Function.Log(consoleType.Message, "Disconnected.");
                    Console.Read();
                }
            }
            catch (Exception ex)
            {
                Function.Log(consoleType.Error, ex.ToString());
                Thread.Sleep(5000);
                string[] argv = { };
                Main(argv);
            }
        }
    }
}
