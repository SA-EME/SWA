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

        public string LogName { get; set; }
        public string Hostname { get; set; }
        public string Appname { get; set; }
        public string Name { get; set; }
        public LogSeverity Severity { get; set; }
        public string Source { get; set; }
        public int EventID { get; set; }
        public string Message { get; set; }

        public DateTimeOffset TimeGenerated { get; set; }

        public Log(string LogName, string Hostname, string Appname, LogSeverity Severity, int EventID, string Message, DateTimeOffset TimeGenerated)
        {
            this.LogName = LogName;
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
            fl.Append("<").Append("6").Append(">");
            fl.Append(Version);
            fl.Append(" ").Append(TimeGenerated.ToString("o"));
            fl.Append(" ").Append(Hostname);
            fl.Append(" ").Append(Appname);
            fl.Append(" - - - ").Append(Message);

            return Encoding.ASCII.GetBytes(fl.ToString());
        }

        public void Send()
        {
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                udpClient.Connect(SWAService.SERVER_IP, 514);
                udpClient.DontFragment = false;


                Byte[] sendBytes = this.ToFormatRSyslog();
                udpClient.Send(sendBytes, sendBytes.Length);
                udpClient.Close();
            }
            catch (Exception)
            {
                udpClient.Close();
            }
        }

    }


}
