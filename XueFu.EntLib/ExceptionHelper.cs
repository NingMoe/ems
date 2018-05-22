using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Configuration;

namespace XueFu.EntLib
{
    public enum MessageType
    {
        WebType = 1,
        WinType = 2
    }

    public class ExceptionHelper
    {
        public static void ProcessException(Exception ex)
        {
            ProcessException(ex, true);
        }

        public static void ProcessException(Exception ex, bool log)
        {
            if (log)
            {
                RecordLog(ex);
            }
            ShowMessage(ex.Message);
        }

        private static void RecordLog(Exception ex)
        {
            new TxtLog(ServerHelper.MapPath(@"\Log\")).Write(ex.ToString());
        }

        private static void ShowMessage(string message)
        {
            IMessage message2 = null;
            switch (((MessageType)Enum.Parse(typeof(MessageType), ConfigurationManager.AppSettings["MessageType"])))
            {
                case MessageType.WebType:
                    message2 = new WebMessage();
                    break;

                case MessageType.WinType:
                    message2 = new WinMessage();
                    break;
            }
            if (message2 != null)
            {
                message2.Show(message);
            }
        }
    }    

    public interface IMessage
    {
        void Show(string message);
    }

    public class WebMessage : IMessage
    {
        public void Show(string message)
        {
            ScriptHelper.Alert(message);
        }
    }

    public class WinMessage : IMessage
    {
        public void Show(string message)
        {
        }
    }
}
