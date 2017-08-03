namespace RoomChattingServer
{
    partial class MainForm
    {
        public const int sizeOfTextBox = 2 * rowCount;
        const double labelRatio = 0.1;
        const double textBoxRatio = 1-labelRatio;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatSelectComboBox = new System.Windows.Forms.ComboBox();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.chatSendButton = new System.Windows.Forms.Button();
            this.chatGroupBox = new System.Windows.Forms.GroupBox();
            this.serverIpText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chatSelectComboBox
            // 
            this.chatSelectComboBox.Font = new System.Drawing.Font("Gulim", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chatSelectComboBox.FormattingEnabled = true;
            this.chatSelectComboBox.Location = new System.Drawing.Point(12, 639);
            this.chatSelectComboBox.Name = "chatSelectComboBox";
            this.chatSelectComboBox.Size = new System.Drawing.Size(228, 48);
            this.chatSelectComboBox.TabIndex =10;
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(248, 638);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(814, 48);
            this.inputTextBox.TabIndex = 11;
            // 
            // chatSendButton
            // 
            this.chatSendButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chatSendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chatSendButton.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chatSendButton.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.chatSendButton.Location = new System.Drawing.Point(1070, 640);
            this.chatSendButton.Name = "chatSendButton";
            this.chatSendButton.Size = new System.Drawing.Size(100, 48);
            this.chatSendButton.TabIndex = 12;
            this.chatSendButton.Text = "전 송";
            this.chatSendButton.UseVisualStyleBackColor = false;
            this.chatSendButton.Click += new System.EventHandler(this.chatSendButton_Click);
            // 
            // chatGroupBox
            // 
            this.chatGroupBox.Location = new System.Drawing.Point(10, 64);
            this.chatGroupBox.Name = "chatGroupBox";
            this.chatGroupBox.Size = new System.Drawing.Size(1180, 566);
            this.chatGroupBox.TabIndex = 13;
            this.chatGroupBox.TabStop = false;
            this.chatGroupBox.Text = "chatGroupBox";
            this.chatGroupBox.Visible = false;
            // 
            // serverIpText
            // 
            this.serverIpText.ForeColor = System.Drawing.Color.White;
            this.serverIpText.Location = new System.Drawing.Point(10, 10);
            this.serverIpText.Name = "serverIpText";
            this.serverIpText.Size = new System.Drawing.Size(800, 30);
            this.serverIpText.TabIndex = 14;
            this.serverIpText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.serverIpText);
            this.Controls.Add(this.chatGroupBox);
            this.Controls.Add(this.chatSendButton);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.chatSelectComboBox);
            this.Name = "MainForm";
            this.Text = "Server";
            this.Shown += new System.EventHandler(this.MainForm_Loaded);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox chatSelectComboBox;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button chatSendButton;
        private System.Windows.Forms.RichTextBox[] textBox = new System.Windows.Forms.RichTextBox[sizeOfTextBox];
        private System.Windows.Forms.Label[] textBoxLabel = new System.Windows.Forms.Label[sizeOfTextBox];
        private System.Windows.Forms.GroupBox chatGroupBox;
        private System.Windows.Forms.Label serverIpText;
    }
}