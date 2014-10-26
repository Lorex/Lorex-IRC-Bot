using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;

namespace IRC_Bot_Console
{
    class CmdModule
    {
        public void say(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                Function.SendServerMessage(msgType.Error,"參數不足，語法 @say <message>");
            }
            else
            {
                Function.SendServerMessage(msgType.Information,"\u000304" + Nick + " \u000302表示 \u000304" + cmd[1]);

            }
        }
        public void time()
        {
            Function.SendServerMessage(msgType.Information,"現在時間 " + DateTime.Now);
        }
        public void help() 
        {
            Function.SendServerMessage(msgType.Information,"指令列表 ");
            Function.SendServerMessage(msgType.Information,"==================== ");
            Function.SendServerMessage(msgType.Information,"@time - 顯示當前時間 ");
            Function.SendServerMessage(msgType.Information,"@help - 顯示指令列表 ");
            Function.SendServerMessage(msgType.Information,"@exit - 關閉 Bot [管理指令]");
            Function.SendServerMessage(msgType.Information,"@say - 說話");
            Function.SendServerMessage(msgType.Information,"@me - 在對話前加入自己的名字");
            Function.SendServerMessage(msgType.Information,"@uptime - 顯示 Bot 上線時間");
            Function.SendServerMessage(msgType.Information, "@version - 顯示 Bot 版本資訊");
            Function.SendServerMessage(msgType.Information, "@rand - 產生亂數");
            Function.SendServerMessage(msgType.Information, "@g - 快速搜尋");
            Function.SendServerMessage(msgType.Information, "@shutup - 覺得 Bot 太吵的時候可以叫他閉嘴XD");
            Function.SendServerMessage(msgType.Information, "@rules - 管理對話應答規則");
        }
        public void me(string Nick, string[] cmd)
        {
            if (cmd.Length < 2)
            {
                Function.SendServerMessage(msgType.Error,"參數不足，語法 @me <message>");
            }
            else
            {
                Function.SendServerMessage(msgType.Information, "\u000304" + Nick + " " + cmd[1]);
            }
        }
        public void uptime()
        {
            Function.SendServerMessage(msgType.Information,"Bot 上線時間 " + (((TimeSpan)(DateTime.Now-IrcBot.START_TIME)).Days) + " 日 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Hours) + " 時 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Minutes) + " 分 "
                + (((TimeSpan)(DateTime.Now - IrcBot.START_TIME)).Seconds) + " 秒 "
                );
        }
        public void rand(int r1,int r2)
        {
            Random Counter = new Random(Guid.NewGuid().GetHashCode());
            Function.SendServerMessage(msgType.Information,"亂數： " + Counter.Next(r1, r2));
        }
        public  void version()
        {
            Function.SendServerMessage(msgType.Information, "Lorex IRC Bot v." + Assembly.GetEntryAssembly().GetName().Version.ToString());

        }
        public void g(string keyword)
        {
            if (keyword == "Lorex")
            {
                Function.SendServerMessage(msgType.Information, "就帥哥一個R，哪裡還需要 Google...ˊ_>ˋ");
            }
            else
            {
                Function.SendServerMessage(msgType.Information, "https://www.google.com.tw/#q=" + keyword);
                //Function.SendServerMessage(msgType.Information, "這麼簡單的事情直接叫 oktw 來做就好了，叫我幹嘛？ˊ_>ˋ");
                //IrcBot.writer.WriteLine("PRIVMSG " + config.CHANNEL + " :.g " + keyword);
                //IrcBot.writer.Flush();
                //Thread.Sleep(1000);
                //Function.SendServerMessage(msgType.Information, "恩，好乖 <(￣︶￣)> ");

            }
        }
        public void shutup()
        {
            Function.SendServerMessage(msgType.Information, "好啦好啦好啦我閉嘴就是w");
            config.shut_up = true;
        }

        public class rules
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodeList;
            XmlNode node;

            public rules()
            {
                xmlDoc.Load("Modules/ChatRules.xml");
                xmlNodeList = xmlDoc.SelectNodes("rules/chat");
                node = xmlDoc.SelectSingleNode("rules");
            }

            private void list(string type)
            {
                switch(type)
                {
                    case "rules":
                        int i = 0;
                        foreach (XmlNode node in xmlNodeList)
                        {
                            Function.SendServerMessage(msgType.Information, "RULES[" + i + "]: " + node.Attributes["regex"].Value.ToString() + " => " +  node.Attributes["respond"].Value.ToString());
                            i++;
                        }
                        break;
                    case "const":
                        Function.SendServerMessage(msgType.Information, "CONST [0]: $SENDER => Message Sender");
                        Function.SendServerMessage(msgType.Information, "CONST [1]: $MSG => Message");
                        Function.SendServerMessage(msgType.Information, "CONST [2]: $SP => Space(\" \")");
                        break;
                    default:
                        Function.SendServerMessage(msgType.Error, "參數錯誤，語法 @rules list <rules|const>");
                        break;
                }
                
            }
            private bool rulesVerify(string regex)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Modules/Commands.xml");

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("command/cmd");
                foreach (XmlNode node in xmlNodeList)
                {
                    if (Regex.IsMatch(regex, node.Attributes["line"].Value.ToString()))
                        return false;
                }
                return false;
            }
            private void add(string regex, string respond)
            {

                if (rulesVerify(regex) == false)
                {
                    Function.SendServerMessage(msgType.Error, "不被允許的判斷式");
                    return;
                }
                XmlElement main = xmlDoc.CreateElement("chat");
                main.SetAttribute("regex", regex);
                main.SetAttribute("respond", respond);
                node.AppendChild(main);
                xmlDoc.Save("Modules/ChatRules.xml");
                Function.SendServerMessage(msgType.Information, "對話規則新增： " + regex + " => " + respond);
            }
            private void delete(string regex)
            {
                bool nodeFound = false;
                foreach (XmlNode childnode in xmlNodeList)
                {
                    if (childnode.Attributes["regex"].Value.ToString() == regex)
                    {
                        node.RemoveChild(childnode);
                        xmlDoc.Save("Modules/ChatRules.xml");
                        nodeFound = true;
                        
                    }
                }
                if (nodeFound)
                    Function.SendServerMessage(msgType.Information, "已刪除對話規則： " + regex);
                else
                    Function.SendServerMessage(msgType.Error, "找不到對話規則： " + regex);

            }
            private void modify(string regex, string respond)
            {
                bool nodeFound = false;
                foreach (XmlNode childnode in xmlNodeList)
                {
                    if (childnode.Attributes["regex"].Value.ToString() == regex)
                    {
                        XmlElement element = (XmlElement)childnode;
                        element.SetAttribute("respond", respond);   
                        xmlDoc.Save("Modules/ChatRules.xml");
                        nodeFound = true;
                    }
                }
                if (nodeFound)
                    Function.SendServerMessage(msgType.Information, "已修改對話規則： " + regex + " => " + respond);
                else
                    Function.SendServerMessage(msgType.Error, "找不到對話規則： " + regex);
            }
            public void parse(string[] cmd)
            {
                switch(cmd[1])
                {
                    case "list":
                        if (cmd.Length < 3)
                        {
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @rules list <rules|const>");
                            break;
                        }
                        else
                        {
                            list(cmd[2]);
                        }
                        break;
                    case "add":
                        if (cmd.Length < 4)
                        {
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @rules add <regex> <respond>");
                            break;
                        }
                        else
                        {
                            add(cmd[2], cmd[3]);
                            break;
                        }
                    case "delete":
                        if (cmd.Length < 3)
                        {
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @rules delete <regex>");
                            break;
                        }
                        else
                        {
                            delete(cmd[2]);
                            break;
                        }

                    case "modify":
                        if (cmd.Length < 4)
                        {
                            Function.SendServerMessage(msgType.Error, "參數不足，語法 @rules modify <regex> <new_respond>");
                            break;
                        }
                        else
                        {
                            modify(cmd[2], cmd[3]);
                            break;
                        }
                    default:
                        Function.SendServerMessage(msgType.Error, "無效的參數，語法 @rules <add|delete|modify|list>");
                        break;
                }
            }
        }
        public void debug()
        {
            config.debug = !config.debug;
            Function.SendServerMessage(msgType.Information, "Debug 模式已切換");
        }
    }
}
