using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;


namespace IRC_Bot_Console
{
    class ChatModule
    {
        public static void parseChat(string nick, string chat)
        {
            if (!config.shut_up)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Modules/ChatRules.xml");

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("rules/chat");
                foreach (XmlNode node in xmlNodeList)
                {

                    if (Regex.IsMatch(chat, node.Attributes["regex"].Value.ToString()))
                    {
                        Function.Log(consoleType.Message, "Chat Match: " + node.Attributes["regex"].Value.ToString() + " <=> " + node.Attributes["respond"].Value.ToString());
                        Function.SendServerMessage(msgType.Information, node.Attributes["respond"].Value.ToString());
                    }
                }
            }
            /*
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
            }*/
        }
    }
}
