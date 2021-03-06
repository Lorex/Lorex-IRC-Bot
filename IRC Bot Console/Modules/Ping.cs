﻿using System;
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
            pingSender.IsBackground = true;
            pingSender.Name = "PingSender";
            while (true)
            {
                if (IrcBot.connection == true)
                {
                    IrcBot.writer.WriteLine(PING + config.SERVER);
                    Thread.Sleep(15000);
                }
                else
                {
                    pingSender.Abort();
                    
                }
            }
        }
    }
}
