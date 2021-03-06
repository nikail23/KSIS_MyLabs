﻿namespace ClientProject
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
            this.label2 = new System.Windows.Forms.Label();
            this.ParticipantsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.FilesToLoadListBox = new System.Windows.Forms.ListBox();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DownloadFileButton = new System.Windows.Forms.Button();
            this.AvaibleFilesListBox = new System.Windows.Forms.ListBox();
            this.ChatListBox = new System.Windows.Forms.ListBox();
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
            this.label1.Location = new System.Drawing.Point(467, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 35);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current chat:";
            // 
            // currentChatLabel
            // 
            this.currentChatLabel.AutoSize = true;
            this.currentChatLabel.Font = new System.Drawing.Font("Dominican", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentChatLabel.Location = new System.Drawing.Point(595, 14);
            this.currentChatLabel.Name = "currentChatLabel";
            this.currentChatLabel.Size = new System.Drawing.Size(0, 35);
            this.currentChatLabel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Dominican", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(96, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Participants:";
            // 
            // ParticipantsListBox
            // 
            this.ParticipantsListBox.FormattingEnabled = true;
            this.ParticipantsListBox.Location = new System.Drawing.Point(100, 48);
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
            this.MessageTextBox.Location = new System.Drawing.Point(554, 220);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(253, 20);
            this.MessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Enabled = false;
            this.SendMessageButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendMessageButton.Location = new System.Drawing.Point(554, 241);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(253, 32);
            this.SendMessageButton.TabIndex = 10;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Enabled = false;
            this.DeleteFileButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteFileButton.Location = new System.Drawing.Point(386, 262);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(162, 24);
            this.DeleteFileButton.TabIndex = 11;
            this.DeleteFileButton.Text = "Delete file";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // FilesToLoadListBox
            // 
            this.FilesToLoadListBox.AllowDrop = true;
            this.FilesToLoadListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FilesToLoadListBox.FormattingEnabled = true;
            this.FilesToLoadListBox.ItemHeight = 15;
            this.FilesToLoadListBox.Location = new System.Drawing.Point(386, 220);
            this.FilesToLoadListBox.Name = "FilesToLoadListBox";
            this.FilesToLoadListBox.ScrollAlwaysVisible = true;
            this.FilesToLoadListBox.Size = new System.Drawing.Size(162, 19);
            this.FilesToLoadListBox.TabIndex = 12;
            // 
            // AddFileButton
            // 
            this.AddFileButton.Enabled = false;
            this.AddFileButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddFileButton.Location = new System.Drawing.Point(386, 241);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(162, 24);
            this.AddFileButton.TabIndex = 13;
            this.AddFileButton.Text = "Add file";
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Dominican", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(198, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Message Files:";
            // 
            // DownloadFileButton
            // 
            this.DownloadFileButton.Enabled = false;
            this.DownloadFileButton.Font = new System.Drawing.Font("Dominican", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadFileButton.Location = new System.Drawing.Point(202, 262);
            this.DownloadFileButton.Name = "DownloadFileButton";
            this.DownloadFileButton.Size = new System.Drawing.Size(160, 24);
            this.DownloadFileButton.TabIndex = 16;
            this.DownloadFileButton.Text = "Download file";
            this.DownloadFileButton.UseVisualStyleBackColor = true;
            this.DownloadFileButton.Click += new System.EventHandler(this.DownloadFileButton_Click);
            // 
            // AvaibleFilesListBox
            // 
            this.AvaibleFilesListBox.FormattingEnabled = true;
            this.AvaibleFilesListBox.Location = new System.Drawing.Point(202, 48);
            this.AvaibleFilesListBox.Name = "AvaibleFilesListBox";
            this.AvaibleFilesListBox.Size = new System.Drawing.Size(160, 212);
            this.AvaibleFilesListBox.TabIndex = 17;
            // 
            // ChatListBox
            // 
            this.ChatListBox.FormattingEnabled = true;
            this.ChatListBox.Location = new System.Drawing.Point(386, 48);
            this.ChatListBox.Name = "ChatListBox";
            this.ChatListBox.Size = new System.Drawing.Size(421, 173);
            this.ChatListBox.TabIndex = 18;
            this.ChatListBox.SelectedIndexChanged += new System.EventHandler(this.ChatListBox_SelectedIndexChanged);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 309);
            this.Controls.Add(this.ChatListBox);
            this.Controls.Add(this.AvaibleFilesListBox);
            this.Controls.Add(this.DownloadFileButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AddFileButton);
            this.Controls.Add(this.FilesToLoadListBox);
            this.Controls.Add(this.DeleteFileButton);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ParticipantsListBox);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ParticipantsListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.Button DeleteFileButton;
        private System.Windows.Forms.ListBox FilesToLoadListBox;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DownloadFileButton;
        private System.Windows.Forms.ListBox AvaibleFilesListBox;
        private System.Windows.Forms.ListBox ChatListBox;
    }
}

