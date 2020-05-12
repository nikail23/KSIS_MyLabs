namespace ClientProject
{
    partial class mainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServersListBox = new System.Windows.Forms.ListBox();
            this.ClientNameTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.currentChatLabel = new System.Windows.Forms.Label();
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ParticipantsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.FilesInfoListBox = new System.Windows.Forms.ListBox();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.ShowFilesButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServersListBox
            // 
            this.ServersListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServersListBox.FormattingEnabled = true;
            this.ServersListBox.Location = new System.Drawing.Point(9, 48);
            this.ServersListBox.Name = "ServersListBox";
            this.ServersListBox.Size = new System.Drawing.Size(77, 160);
            this.ServersListBox.TabIndex = 0;
            // 
            // ClientNameTextBox
            // 
            this.ClientNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClientNameTextBox.Location = new System.Drawing.Point(9, 219);
            this.ClientNameTextBox.Name = "ClientNameTextBox";
            this.ClientNameTextBox.Size = new System.Drawing.Size(77, 20);
            this.ClientNameTextBox.TabIndex = 1;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(9, 247);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(77, 39);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Dominican", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(348, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 35);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current chat:";
            // 
            // currentChatLabel
            // 
            this.currentChatLabel.AutoSize = true;
            this.currentChatLabel.Font = new System.Drawing.Font("Dominican", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentChatLabel.Location = new System.Drawing.Point(455, 9);
            this.currentChatLabel.Name = "currentChatLabel";
            this.currentChatLabel.Size = new System.Drawing.Size(0, 35);
            this.currentChatLabel.TabIndex = 4;
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(354, 47);
            this.ChatTextBox.Multiline = true;
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.Size = new System.Drawing.Size(274, 179);
            this.ChatTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Dominican", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(92, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Participants:";
            // 
            // ParticipantsListBox
            // 
            this.ParticipantsListBox.FormattingEnabled = true;
            this.ParticipantsListBox.Location = new System.Drawing.Point(96, 48);
            this.ParticipantsListBox.Name = "ParticipantsListBox";
            this.ParticipantsListBox.Size = new System.Drawing.Size(89, 238);
            this.ParticipantsListBox.TabIndex = 7;
            this.ParticipantsListBox.SelectedIndexChanged += new System.EventHandler(this.ParticipantsListBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Dominican", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Servers list:";
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(354, 236);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(274, 20);
            this.MessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Enabled = false;
            this.SendMessageButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendMessageButton.Location = new System.Drawing.Point(354, 262);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(274, 32);
            this.SendMessageButton.TabIndex = 10;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Enabled = false;
            this.DeleteFileButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteFileButton.Location = new System.Drawing.Point(197, 219);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(128, 24);
            this.DeleteFileButton.TabIndex = 11;
            this.DeleteFileButton.Text = "Delete file";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            // 
            // FilesInfoListBox
            // 
            this.FilesInfoListBox.FormattingEnabled = true;
            this.FilesInfoListBox.Location = new System.Drawing.Point(197, 48);
            this.FilesInfoListBox.Name = "FilesInfoListBox";
            this.FilesInfoListBox.Size = new System.Drawing.Size(128, 108);
            this.FilesInfoListBox.TabIndex = 12;
            // 
            // AddFileButton
            // 
            this.AddFileButton.Enabled = false;
            this.AddFileButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddFileButton.Location = new System.Drawing.Point(197, 176);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(128, 24);
            this.AddFileButton.TabIndex = 13;
            this.AddFileButton.Text = "Add file";
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // ShowFilesButton
            // 
            this.ShowFilesButton.Enabled = false;
            this.ShowFilesButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowFilesButton.Location = new System.Drawing.Point(197, 154);
            this.ShowFilesButton.Name = "ShowFilesButton";
            this.ShowFilesButton.Size = new System.Drawing.Size(128, 24);
            this.ShowFilesButton.TabIndex = 14;
            this.ShowFilesButton.Text = "Show files";
            this.ShowFilesButton.UseVisualStyleBackColor = true;
            this.ShowFilesButton.Click += new System.EventHandler(this.ShowFilesButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Dominican", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(193, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Files:";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(197, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 24);
            this.button1.TabIndex = 16;
            this.button1.Text = "Download file";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 298);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ShowFilesButton);
            this.Controls.Add(this.AddFileButton);
            this.Controls.Add(this.FilesInfoListBox);
            this.Controls.Add(this.DeleteFileButton);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ParticipantsListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.currentChatLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ClientNameTextBox);
            this.Controls.Add(this.ServersListBox);
            this.Name = "mainForm";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ServersListBox;
        private System.Windows.Forms.TextBox ClientNameTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label currentChatLabel;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ParticipantsListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.Button DeleteFileButton;
        private System.Windows.Forms.ListBox FilesInfoListBox;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.Button ShowFilesButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

