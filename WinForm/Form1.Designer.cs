namespace WinForm
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            BtStart = new Button();
            BtStop = new Button();
            LblTemperature = new Label();
            LblStatus = new Label();
            LblDeviceId = new Label();
            txtDeviceId = new TextBox();
            txtHost = new TextBox();
            txtPort = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            txtInterval = new TextBox();
            LblLastSend = new Label();
            StatusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            timer = new System.Windows.Forms.Timer(components);
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // BtStart
            // 
            BtStart.Location = new Point(14, 275);
            BtStart.Margin = new Padding(4);
            BtStart.Name = "BtStart";
            BtStart.Size = new Size(117, 46);
            BtStart.TabIndex = 0;
            BtStart.Text = "开始采集";
            BtStart.UseVisualStyleBackColor = true;
            BtStart.Click += BtStart_Click;
            // 
            // BtStop
            // 
            BtStop.Enabled = false;
            BtStop.Location = new Point(138, 275);
            BtStop.Margin = new Padding(4);
            BtStop.Name = "BtStop";
            BtStop.Size = new Size(117, 46);
            BtStop.TabIndex = 1;
            BtStop.Text = "停止采集";
            BtStop.UseVisualStyleBackColor = true;
            BtStop.Click += BtStop_Click;
            // 
            // LblTemperature
            // 
            LblTemperature.AutoSize = true;
            LblTemperature.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold, GraphicsUnit.Point, 134);
            LblTemperature.Location = new Point(14, 157);
            LblTemperature.Margin = new Padding(4, 0, 4, 0);
            LblTemperature.Name = "LblTemperature";
            LblTemperature.Size = new Size(71, 37);
            LblTemperature.TabIndex = 2;
            LblTemperature.Text = "--.--";
            // 
            // LblStatus
            // 
            LblStatus.AutoSize = true;
            LblStatus.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            LblStatus.Location = new Point(13, 235);
            LblStatus.Margin = new Padding(4, 0, 4, 0);
            LblStatus.Name = "LblStatus";
            LblStatus.Size = new Size(57, 20);
            LblStatus.TabIndex = 3;
            LblStatus.Text = "状态：";
            // 
            // LblDeviceId
            // 
            LblDeviceId.AutoSize = true;
            LblDeviceId.Location = new Point(21, 25);
            LblDeviceId.Margin = new Padding(4, 0, 4, 0);
            LblDeviceId.Name = "LblDeviceId";
            LblDeviceId.Size = new Size(68, 17);
            LblDeviceId.TabIndex = 4;
            LblDeviceId.Text = "设备编号：";
            LblDeviceId.TextAlign = ContentAlignment.TopRight;
            // 
            // txtDeviceId
            // 
            txtDeviceId.Location = new Point(97, 22);
            txtDeviceId.Margin = new Padding(4);
            txtDeviceId.Name = "txtDeviceId";
            txtDeviceId.Size = new Size(157, 23);
            txtDeviceId.TabIndex = 5;
            txtDeviceId.Text = "TEMP-001";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(97, 56);
            txtHost.Margin = new Padding(4);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(157, 23);
            txtHost.TabIndex = 6;
            txtHost.Text = "106.14.146.154";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(97, 90);
            txtPort.Margin = new Padding(4);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(157, 23);
            txtPort.TabIndex = 7;
            txtPort.Text = "1883";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(45, 59);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 8;
            label1.Text = "主机：";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(47, 93);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 9;
            label2.Text = "端口：";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 128);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(64, 17);
            label3.TabIndex = 10;
            label3.Text = "间隔(秒)：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.Location = new Point(146, 170);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(25, 20);
            label4.TabIndex = 11;
            label4.Text = "°C";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 127);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(68, 17);
            label5.TabIndex = 13;
            label5.Text = "采集间隔：";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 209);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(68, 17);
            label6.TabIndex = 14;
            label6.Text = "上次发送：";
            // 
            // txtInterval
            // 
            txtInterval.Location = new Point(97, 124);
            txtInterval.Margin = new Padding(4);
            txtInterval.Name = "txtInterval";
            txtInterval.Size = new Size(157, 23);
            txtInterval.TabIndex = 15;
            txtInterval.Text = "3";
            // 
            // LblLastSend
            // 
            LblLastSend.AutoSize = true;
            LblLastSend.Location = new Point(97, 209);
            LblLastSend.Margin = new Padding(4, 0, 4, 0);
            LblLastSend.Name = "LblLastSend";
            LblLastSend.Size = new Size(32, 17);
            LblLastSend.TabIndex = 16;
            LblLastSend.Text = "从未";
            // 
            // StatusStrip
            // 
            StatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            StatusStrip.Location = new Point(0, 347);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Padding = new Padding(1, 0, 16, 0);
            StatusStrip.Size = new Size(274, 22);
            StatusStrip.TabIndex = 17;
            StatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(117, 17);
            toolStripStatusLabel.Text = "模拟测温设备 - 就绪";
            // 
            // timer
            // 
            timer.Tick += timer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(274, 369);
            Controls.Add(LblLastSend);
            Controls.Add(txtInterval);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Controls.Add(txtDeviceId);
            Controls.Add(LblDeviceId);
            Controls.Add(LblStatus);
            Controls.Add(LblTemperature);
            Controls.Add(BtStop);
            Controls.Add(BtStart);
            Controls.Add(StatusStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "模拟测温设备";
            FormClosing += Form1_FormClosing;
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtStart;
        private System.Windows.Forms.Button BtStop;
        private System.Windows.Forms.Label LblTemperature;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label LblDeviceId;
        private System.Windows.Forms.TextBox txtDeviceId;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label LblLastSend;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Timer timer;
    }
}