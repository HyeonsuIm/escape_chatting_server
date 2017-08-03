using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace RoomChattingServer.TCPIPCummunication
{
    class TCPIPClientHandler
    {
        public TcpClient m_tcpClient;
        TCPIPServer m_server;
        public string name;
        public delegate void MessageDisplayHandler(string text, string name);
        public event MessageDisplayHandler OnReceived;

        public delegate void AddNameHandler(string name);
        public event AddNameHandler onNameChanged;

        public delegate void ExitChatHandler(string name);
        public event ExitChatHandler exitChat;
        Thread workThread = null;

        private TCPIPClientHandler() { }
        public TCPIPClientHandler(TcpClient socket, TCPIPServer server)
        {
            m_tcpClient = socket;
            m_tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            m_server = server;
        }

        public void startThread()
        {
            workThread = new Thread(doCommunication);
            workThread.Start();
        }
        ~TCPIPClientHandler()
        {
            if(workThread != null)
            {
                if (workThread.IsAlive)
                    workThread.Abort();
            }
        }

        private void doCommunication()
        {
            NetworkStream stream = null;
            try
            {
                byte[] buffer = new byte[1024];
                string msg = string.Empty;
                int bytes = 0;
                stream = m_tcpClient.GetStream();
                bytes = stream.Read(buffer, 0, buffer.Length);
                
                msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                name = msg.TrimEnd('0');
                onNameChanged(msg);

                List<string> messageList = m_server.getMessageListFromName(name);
                if (messageList.Count != 0)
                {
                    Thread.Sleep(100);
                    foreach (string message in messageList)
                    {
                        try
                        {
                            sendMessage("0" + message);
                        }catch(Exception)
                        { }
                        Thread.Sleep(100);
                    }
                }
                messageList.Clear();

                while (true)
                {
                    stream = m_tcpClient.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0)
                    {
                        exitChat(name);
                        break;
                    }
                    msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    if (msg[0] == '1')
                    {
                        if (OnReceived != null)
                            OnReceived(msg.Substring(1), name);
                    }

                    stream.Flush();
                }
            }
            catch (Exception)
            {

            }
            exitChat(name);
         }
        
        public void waitClose(int sec)
        {
            for(int i=0;i<sec * 10;i++)
            {
                if (!m_tcpClient.Connected)
                    break;
                Thread.Sleep(100);
            }
        }

        public void sendMessage(string message)
        {
            NetworkStream stream = null;
            stream = m_tcpClient.GetStream();
            byte[] sbuffer = Encoding.Unicode.GetBytes(message);
            stream.Write(sbuffer, 0, sbuffer.Length);
            stream.Flush();
        }
        public void commClose()
        {
            m_tcpClient.Close();
            if (workThread != null)
            {
                if (workThread.IsAlive)
                    workThread.Abort();
            }
            
        }
     }
}
