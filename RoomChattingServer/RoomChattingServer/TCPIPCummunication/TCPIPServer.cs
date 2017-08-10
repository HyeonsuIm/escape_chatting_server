using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using RoomChattingServer.TCPIPCummunication;
using System.Threading;
using System.Media;
using System.Text;

namespace RoomChattingServer
{
    class TCPIPServer
    {
        MainForm m_mainForm = null;
        TcpListener tcpListener = null;
        TcpClient clientSocket = null;
        List<TCPIPClientHandler> handlerList;
        int connectionCount;
        Thread workerThread;

        private TCPIPServer()
        {
            handlerList = new List<TCPIPClientHandler>();
            connectionCount = 0;
        }
        public TCPIPServer(MainForm mainForm, int port) : this()
        {
            m_mainForm = mainForm;
            tcpListener = new TcpListener(IPAddress.Any, port);
            workerThread = new Thread(startServer);
            workerThread.Start();
        }

        ~TCPIPServer()
        {
            workerThread.Abort();
            handlerList.Clear();
        }
        public void startServer()
        {
            try
            {
                tcpListener.Start();
            }catch(Exception)
            {
                return;
            }
            while (true)
            {
                clientSocket = tcpListener.AcceptTcpClient();

                TCPIPClientHandler handler = new TCPIPClientHandler(clientSocket, this);
                handler.OnReceived += new TCPIPClientHandler.MessageDisplayHandler(receiveText);
                handler.onNameChanged += new TCPIPClientHandler.AddNameHandler(addName);
                handler.exitChat += new TCPIPClientHandler.ExitChatHandler(exitChat);
                handlerList.Add(handler);
                handler.startThread();
                connectionCount++;
            }
        }

        public void receiveText(string text, string name)
        {
            m_mainForm.addTextBox(text, name, System.Drawing.Color.White);
            m_mainForm.markTextBox(name);
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.door_slam);
            simpleSound.Play();

            SaveText(name, text);
        }

        private void sendErrorCode(string name, string error)
        {
            foreach (TCPIPClientHandler handler in handlerList)
            {
                if (handler.name == name)
                {
                    try
                    {
                        handler.sendMessage(error);
                    }
                    catch (Exception)
                    { }
                    m_mainForm.removeName(name);
                    handlerList.Remove(handler);
                    connectionCount--;
                    if (handler.m_tcpClient.Connected)
                        handler.commClose();
                }
            }
        }

        public void sendText(string name, string text)
        {
            bool isSended = false;
            foreach (TCPIPClientHandler handler in handlerList)
            {
                try
                {
                    if (handler.name == name)
                    {
                        handler.sendMessage("0" + text);
                        isSended = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            if( false == isSended)
            {
                receiveText("메세지 전송이 실패하였습니다.", name);
            }
        }

        public void addName(string name)
        {
            int result = m_mainForm.addName(name);
            if(result == 1)
            {
                sendErrorCode(name, "2");
            }

        }
        public void exitChat(string name)
        {
            //m_mainForm.addTextBox("상대방이 나갔습니다.", name, System.Drawing.Color.White);
            connectionCount--;
            m_mainForm.removeName(name);
            foreach(TCPIPClientHandler handler in handlerList)
            {
                if (handler.name == name)
                {
                    handler.commClose();
                    handlerList.Remove(handler);
                    break;
                }
            }
        }

        public List<string> getMessageListFromName(string name)
        {
            return m_mainForm.getMessageList(name);
        }

        public void saveText(string name, string text)
        {
            StreamWriter sw = new StreamWriter(name + ".txt", false, Encoding.GetEncoding("euc-kr"));
            sw.WriteLine(text);
            sw.Close();
        }
    }
}