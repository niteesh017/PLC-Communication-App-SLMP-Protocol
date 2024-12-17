namespace PLC_SLMP
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtIpAddress = new TextBox();
            lblIp = new Label();
            label1 = new Label();
            txtPort = new TextBox();
            btnConnect = new Button();
            lblCommand = new Label();
            txtCommand = new TextBox();
            btnSend = new Button();
            txtResponse = new TextBox();
            btnDisconnect = new Button();
            lblStatus = new Label();
            chkWriteOperation = new CheckBox();
            txtWriteData = new TextBox();
            chkReadOperation = new CheckBox();
            pictureBoxStatus = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxStatus).BeginInit();
            SuspendLayout();
            // 
            // txtIpAddress
            // 
            txtIpAddress.Location = new Point(187, 31);
            txtIpAddress.Name = "txtIpAddress";
            txtIpAddress.Size = new Size(454, 23);
            txtIpAddress.TabIndex = 0;
            txtIpAddress.Text = "192.168.1.2";
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(15, 31);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(86, 15);
            lblIp.TabIndex = 1;
            lblIp.Text = "PLC IP Address";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 66);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 2;
            label1.Text = "PLC Port";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(187, 66);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(454, 23);
            txtPort.TabIndex = 3;
            txtPort.Text = "2000";
            txtPort.TextChanged += txtPort_TextChanged;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(187, 95);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(113, 31);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "Connect to PLC";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // lblCommand
            // 
            lblCommand.AutoSize = true;
            lblCommand.Location = new Point(15, 182);
            lblCommand.Name = "lblCommand";
            lblCommand.Size = new Size(103, 15);
            lblCommand.TabIndex = 5;
            lblCommand.Text = "Enter PLC Register";
            // 
            // txtCommand
            // 
            txtCommand.Location = new Point(187, 179);
            txtCommand.Name = "txtCommand";
            txtCommand.PlaceholderText = "Enter the PLC register (e.g., D200, D201, D202)";
            txtCommand.Size = new Size(454, 23);
            txtCommand.TabIndex = 6;
            txtCommand.TextChanged += txtCommand_TextChanged;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(528, 248);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(113, 31);
            btnSend.TabIndex = 7;
            btnSend.Text = "Send Command";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtResponse
            // 
            txtResponse.Location = new Point(15, 309);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.Size = new Size(626, 101);
            txtResponse.TabIndex = 8;
            txtResponse.TextChanged += txtResponse_TextChanged;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(497, 95);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(144, 31);
            btnDisconnect.TabIndex = 9;
            btnDisconnect.Text = "DisConnect to PLC";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += button1_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(15, 281);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 17);
            lblStatus.TabIndex = 11;
            lblStatus.Click += label2_Click;
            // 
            // chkWriteOperation
            // 
            chkWriteOperation.AutoSize = true;
            chkWriteOperation.Location = new Point(587, 144);
            chkWriteOperation.Name = "chkWriteOperation";
            chkWriteOperation.Size = new Size(54, 19);
            chkWriteOperation.TabIndex = 12;
            chkWriteOperation.Text = "Write";
            chkWriteOperation.UseVisualStyleBackColor = true;
            chkWriteOperation.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // txtWriteData
            // 
            txtWriteData.Location = new Point(187, 215);
            txtWriteData.Name = "txtWriteData";
            txtWriteData.PlaceholderText = "Enter the value you want to write into the PLC";
            txtWriteData.Size = new Size(454, 23);
            txtWriteData.TabIndex = 13;
            txtWriteData.TextChanged += txtWriteData_TextChanged;
            // 
            // chkReadOperation
            // 
            chkReadOperation.AutoSize = true;
            chkReadOperation.Location = new Point(187, 144);
            chkReadOperation.Name = "chkReadOperation";
            chkReadOperation.Size = new Size(52, 19);
            chkReadOperation.TabIndex = 14;
            chkReadOperation.Text = "Read";
            chkReadOperation.UseVisualStyleBackColor = true;
            chkReadOperation.CheckedChanged += chkReadOperation_CheckedChanged;
            // 
            // pictureBoxStatus
            // 
            pictureBoxStatus.Location = new Point(341, 97);
            pictureBoxStatus.Name = "pictureBoxStatus";
            pictureBoxStatus.Size = new Size(125, 28);
            pictureBoxStatus.TabIndex = 15;
            pictureBoxStatus.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(658, 423);
            Controls.Add(pictureBoxStatus);
            Controls.Add(chkReadOperation);
            Controls.Add(txtWriteData);
            Controls.Add(chkWriteOperation);
            Controls.Add(lblStatus);
            Controls.Add(btnDisconnect);
            Controls.Add(txtResponse);
            Controls.Add(btnSend);
            Controls.Add(txtCommand);
            Controls.Add(lblCommand);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(label1);
            Controls.Add(lblIp);
            Controls.Add(txtIpAddress);
            Name = "Form1";
            Text = "PLC_SLMP";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxStatus).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIpAddress;
        private Label lblIp;
        private Label label1;
        private TextBox txtPort;
        private Button btnConnect;
        private Label lblCommand;
        private TextBox txtCommand;
        private Button btnSend;
        private TextBox txtResponse;
        private Button btnDisconnect;
        private Label lblStatus;
        private CheckBox chkWriteOperation;
        private TextBox txtWriteData;
        private CheckBox chkReadOperation;
        private PictureBox pictureBoxStatus;
    }
}
