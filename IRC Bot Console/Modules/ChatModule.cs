﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC_Bot_Console
{
    class ChatModule
    {
        public static void parseChat(string nick, string chat)
        {
            if (!config.shut_up)
            {
                switch (chat)
                {
                    case "XD":
                        Function.SendServerMessage(msgType.Information, "什麼事情這麼好笑XD");
                        break;
                    case "w":
                        Function.SendServerMessage(msgType.Information, "wwwwwwwwwwwwwwwwwwwww");
                        break;
                }
            }
        }
    }
}
