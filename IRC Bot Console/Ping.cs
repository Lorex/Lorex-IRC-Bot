using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IRC_Bot_Console
{
    class Ping
    {
        static string PING = "PING :";
        private Thread pingSender;
        
        //建構子
        public Ping()
        {
            pingSender = new Thread (new ThreadStart(this.Run));
        }
        public void Start()
        {
            pingSender.Start();
        }
        public void Run()
        {
            while (true)
            {
                IrcBot.writer.WriteLine(PING + IrcBot.SERVER);
                IrcBot.writer.Flush();
                Thread.Sleep(15000);
            }
        }
    }
}
