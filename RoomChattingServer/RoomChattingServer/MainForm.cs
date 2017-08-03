using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RoomChattingServer
{
    public partial class MainForm : Form
    {
        public const int rowCount = 4;
        TCPIPServer server;
        delegate void SetTextCallback(string text, string name, Color color);
        delegate void SetBackColorToMarked(string name);
        delegate void AddComboBoxCallback(string name);
        delegate void RemoveComboBoxCallback(string name);
        int[] isUsedTextBox = new int[10];
        List<string>[] reserveChatList = new List<string>[10];
        Color markColor = Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        Color backColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        char[] trimData = { '\r', '\n', ' ' };
        string fileName = @"roomNameList.txt";
        bool isAlt = false;
        public MainForm()
        {
            InitializeComponent();
            InitializeOtherComponent();
            syncorinizeComponentPosition();

        }

        private void InitializeOtherComponent()
        {
            for (int i = 0; i < sizeOfTextBox; i++)
            {
                reserveChatList[i] = new List<string>();

                this.textBox[i] = new System.Windows.Forms.RichTextBox();
                this.textBoxLabel[i] = new System.Windows.Forms.Label();
                this.textBox[i].BackColor = backColor;

                textBox[i].SelectionStart = textBox[i].Text.Length;
                textBox[i].ScrollToCaret();

                this.Controls.Add(this.textBox[i]);
                this.Controls.Add(this.textBoxLabel[i]);
            }
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            serverIpText.Text = host.AddressList[0].ToString();
        }

        private void MainForm_Loaded(object sender, EventArgs e)
        {
            server = new TCPIPServer(this, 10002);
            if (!isAlreadySet())
            {
                MessageBox.Show("방 이름 파일이 존재하지 않습니다.", "오류");
                Application.Exit();
            }

            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader psReader = new StreamReader(fileStream, System.Text.Encoding.Unicode);
            string[] roomNameList = new string[sizeOfTextBox];
            string roomName;
            for (int i = 0; i < sizeOfTextBox; i++) 
            {
                
                try
                {
                    roomName = psReader.ReadLine();

                    textBoxLabel[i].Text = roomName;
                    textBox[i].Name = roomName;
                    chatSelectComboBox.Items.Insert(i, roomName);
                }
                catch (Exception)
                {
                    MessageBox.Show("방이름 갯수가 부족합니다.", "오류");
                    psReader.Close();
                    fileStream.Close();
                    Application.Exit();
                }
            }
            psReader.Close();
            fileStream.Close();
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            syncorinizeComponentPosition();
        }

        private bool isAlreadySet()
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
                return true;
            else
                return false;
        }

        private void syncorinizeComponentPosition()
        {
            chatGroupBox.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 150);
            serverIpText.Size = new Size(this.ClientSize.Width - 20, 50);
            Font ipFont = serverIpText.Font;
            this.serverIpText.Font = new Font(ipFont.Name, 50, ipFont.Style, GraphicsUnit.Pixel, ipFont.GdiCharSet, ipFont.GdiVerticalFont);
            int chatGroupBoxTop = chatGroupBox.Top;
            int chatGroupBoxBottom = chatGroupBox.Bottom;
            int chatGroupBoxLeft = chatGroupBox.Left;
            int chatGroupBoxRight = chatGroupBox.Right;

            int chatBoxWidth = chatGroupBox.Size.Width / rowCount;
            int chatBoxHeight = chatGroupBox.Size.Height / 2;

            int chatBoxIntervalWidth = 10;
            int chatBoxIntervalHeight = 10;

            for (int col = 0; col < 2; col++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    
                    this.textBox[row + col * rowCount].Location = new System.Drawing.Point(chatGroupBoxLeft + chatBoxWidth * row, (int)(chatGroupBoxTop + chatBoxHeight * (col + labelRatio)) );
                    this.textBox[row + col * rowCount].Multiline = true;
                    this.textBox[row + col * rowCount].Size = new System.Drawing.Size(chatBoxWidth - chatBoxIntervalWidth, (int)(chatBoxHeight * textBoxRatio - chatBoxIntervalHeight));
                    this.textBox[row + col * rowCount].ForeColor = Color.White;
                    this.textBox[row + col * rowCount].ReadOnly = true;
                    this.textBox[row + col * rowCount].ScrollBars = RichTextBoxScrollBars.ForcedVertical;

                    this.textBoxLabel[row + col * rowCount].Location = new System.Drawing.Point(chatGroupBoxLeft + chatBoxWidth * row, (int)(chatGroupBoxTop + chatBoxHeight * col));
                    this.textBoxLabel[row + col * rowCount].Size = new System.Drawing.Size(chatBoxWidth - chatBoxIntervalWidth, (int)(chatBoxHeight * labelRatio - chatBoxIntervalHeight));
                    this.textBoxLabel[row + col * rowCount].ForeColor = Color.White;
                    this.textBoxLabel[row + col * rowCount].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    Font font = this.textBoxLabel[row + col * rowCount].Font;
                    if (chatBoxHeight < 0)
                        continue;
                    try
                    {
                        this.textBoxLabel[row + col * rowCount].Font = new Font("굴림", chatBoxHeight / 15, font.Style, GraphicsUnit.Pixel, font.GdiCharSet, font.GdiVerticalFont);
                    }catch(Exception)
                    {

                    }
                }
            }
            this.chatSendButton.Location = new System.Drawing.Point(chatGroupBoxRight - 100 - chatBoxIntervalWidth, chatGroupBoxBottom + 10);
            this.inputTextBox.Location = new System.Drawing.Point(inputTextBox.Location.X, chatGroupBoxBottom + 10);
            this.inputTextBox.Size = new System.Drawing.Size(chatGroupBox.Width - 30 - chatSendButton.Width - chatSelectComboBox.Width, this.inputTextBox.Height);
            this.chatSelectComboBox.Location = new System.Drawing.Point(chatSelectComboBox.Location.X, chatGroupBoxBottom + 10);
        }

        public void addTextBox(string text, string name, Color color)
        {
            int index = findTextBoxToIndex(name);
            if (index == -1)
                return;
            if (this.textBox[index].InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(addTextBox);
                this.Invoke(d, new object[] { text, name, color});
            }
            else
            {
                text = text.TrimEnd(trimData);
                int startIndex = getLastTextBoxIndex(index);
                this.textBox[index].AppendText("\r\n" + text);
                int endIndex = getLastTextBoxIndex(index);
                setTextColor(index, startIndex, endIndex, color);
                textBox[index].SelectionStart = textBox[index].Text.Length;
                textBox[index].ScrollToCaret();
            }
        }

        private void chatSendButton_Click(object sender, EventArgs e)
        {
            if (inputTextBox.TextLength == 0)
                return;
            sendData();
            inputTextBox.Select();
        }
        
        private void sendData()
        {
            if(this.chatSelectComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("방이 선택되지 않았습니다.", "오류");
                return;
            }
            if(this.chatSelectComboBox.Items[this.chatSelectComboBox.SelectedIndex] == null)
            {
                return;
            }
            String name = (String)this.chatSelectComboBox.Items[this.chatSelectComboBox.SelectedIndex];
            String text = this.inputTextBox.Text;

            this.inputTextBox.Clear();

            if (isUsedTextBox[this.chatSelectComboBox.SelectedIndex] >= 1)
            {
                addTextBox(text, name, Color.Yellow);
                if (isUsedTextBox[this.chatSelectComboBox.SelectedIndex] == 2)
                {
                    reserveChatList[this.chatSelectComboBox.SelectedIndex].Add(text);
                }
                else
                {
                    server.sendText(name, text);
                }
                this.textBox[this.chatSelectComboBox.SelectedIndex].BackColor = backColor;
            }
        }

        public void addComboBox(string name)
        {
            if (this.chatSelectComboBox.InvokeRequired)
            {
                AddComboBoxCallback d = new AddComboBoxCallback(addComboBox);
                this.Invoke(d, new object[] { name });
            }
            else
            {
                chatSelectComboBox.Items.Add(name);
            }
            
        }

        public int addName(string name)
        {
            int index = findTextBoxToIndex(name);
            if (index != -1)
            {
                isUsedTextBox[index] = 1;
                //addTextBox("입장하였습니다.", name, Color.White);
                return 0;
            }
            else{
                return 1;
            }
        }

        public void removeName(string name)
        {
            if (this.chatSelectComboBox.InvokeRequired)
            {
                RemoveComboBoxCallback d = new RemoveComboBoxCallback(removeName);
                this.Invoke(d, new object[] { name });
            }
            else
            {
                int index = findTextBoxToIndex(name);
                if (index == -1)
                    return;
            }
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                if(keyData >= (Keys.Control | Keys.F1) && keyData <= (Keys.Control | Keys.F8))
                {
                    int index = (int)keyData - (int)(Keys.Control | Keys.F1);
                    textBox[index].Clear();
                    textBox[index].BackColor = backColor;
                }
                if ((int)keyData >= 112 && (int)keyData < 112 + sizeOfTextBox)
                {
                    int index = (int)(keyData) - 112;
                    if (isUsedTextBox[index] >= 1)
                    {
                        if (isAlt)
                        {
                            textBox[index].Clear();
                        }
                        else
                        {
                            selectTextBox(index);
                            inputTextBox.Select();
                            chatSelectComboBox.SelectedIndex = index;
                        }
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("접속되지 않은 방입니다.", "오류");
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private void selectTextBox(int index)
        {
            try
            {
                this.textBox[index].BackColor = backColor;
            }
            catch (Exception)
            {

            }
        }

        public void markTextBox(string name)
        {
            if (this.chatSelectComboBox.InvokeRequired)
            {
                SetBackColorToMarked d = new SetBackColorToMarked(markTextBox);
                this.Invoke(d, new object[] { name });
            }
            else
            {
                int index = findTextBoxToIndex(name);
                if (index == -1)
                    return;
                this.textBox[index].BackColor = markColor;
            }

            

        }

        private int findTextBoxToIndex(string name)
        {
            for (int i = 0; i < sizeOfTextBox; i++)
            {
                if (this.textBox[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int getLastTextBoxIndex(int index)
        {
            return textBox[index].TextLength;
        }

        private void setTextColor(int index, int startIndex, int endIndex, Color color)
        {
            textBox[index].Select(startIndex, endIndex);
            textBox[index].SelectionColor = color;
        }
        private int comboBoxNameToIndex(string name)
        {
             return chatSelectComboBox.Items.IndexOf(name);
        }

        public List<string> getMessageList(string name)
        {
            int index = findTextBoxToIndex(name);
            return reserveChatList[index];
        }

        public void printMessageBox(string title, string msg)
        {
            MessageBox.Show(msg, title);
        }
    }
}
