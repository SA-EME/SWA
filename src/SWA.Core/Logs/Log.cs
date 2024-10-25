﻿using System.Text;
using System;
using System.Net.Sockets;

namespace SWA.Core.Logs
{

    public class Log
    {

        public int Version
        {
            get { return 1; }
        }

        public string Name { get; set; }
        public string Hostname { get; set; }
        public string Appname { get; set; }

        public LogSeverity Severity { get; set; }
        public string Source { get; set; }
        public int EventID { get; set; }
        public string Message { get; set; }

        public DateTimeOffset TimeGenerated { get; set; }

        public Log(string Name, string Hostname, string Appname, LogSeverity Severity, int EventID, string Message, DateTimeOffset TimeGenerated)
        {
            this.Name = Name;
            this.Hostname = Hostname;
            this.Appname = Appname;
            this.Severity = Severity;
            this.EventID = EventID;
            this.Message = Message;
            this.TimeGenerated = TimeGenerated;
        }


        private byte[] ToFormatRSyslog()
        {
            StringBuilder fl = new StringBuilder();
            fl.Append("<").Append((int)this.Severity).Append(">");
            fl.Append(Version);
            fl.Append(" ").Append(TimeGenerated.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"));
            // fl.Append(" ").Append(TimeGenerated.ToString("o")); OLD FORMAT
            fl.Append(" ").Append(Hostname);
            fl.Append(" ").Append(Appname);
            fl.Append(" - - - ").Append(Message);

            SWALog.Write("DEBUG", "Log formatted to RSyslog before encoding : " + fl.ToString());

            return Encoding.UTF8.GetBytes(fl.ToString());
        }

        public void Format()
        {
            this.Message = this.Message.Replace("\t", " ");
            this.Message = this.Message.Replace("\r", " ");
        }

        public void Send()
        {
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                udpClient.Connect(SWALog.SWAConfig.Server, 514);
                udpClient.DontFragment = false;


                Byte[] sendBytes = this.ToFormatRSyslog();

                udpClient.Send(sendBytes, sendBytes.Length);
                udpClient.Close();
                SWALog.Write("DEBUG", "Log sent to the serveur : " + SWALog.SWAConfig.Server);
            }
            catch (Exception ex)
            {
                SWALog.Write("ERROR", "Error, can't send log to the serveur" + ex.StackTrace);
                udpClient.Close();
            }
        }


        public override String ToString()
        {
            return $"{TimeGenerated} {Hostname} {Appname} {Severity} {EventID} {Message}";
        }

    }

}
