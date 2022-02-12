namespace SoundMachine
{
    partial class SettingsForm
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.button1 = new System.Windows.Forms.Button();
            this.interruptInputBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numSoundsBox = new System.Windows.Forms.NumericUpDown();
            this.inputCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.muteInputCheckBox = new System.Windows.Forms.CheckBox();
            this.inputSamplerateBox = new System.Windows.Forms.ComboBox();
            this.inputChannelsBox = new System.Windows.Forms.ComboBox();
            this.playbackCheckBox = new System.Windows.Forms.CheckBox();
            this.keyPressBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnToggleBinding = new System.Windows.Forms.Button();
            this.btnBehaviorBinding = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DeviceOutBox = new System.Windows.Forms.ComboBox();
            this.DeviceInBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnRecordBinding = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnOverlayBinding = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnProfileBinding = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnHomeDir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSoundsBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Reload sound devices";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnLoadDevices);
            // 
            // interruptInputBox
            // 
            this.interruptInputBox.AutoSize = true;
            this.interruptInputBox.Location = new System.Drawing.Point(6, 19);
            this.interruptInputBox.Name = "interruptInputBox";
            this.interruptInputBox.Size = new System.Drawing.Size(143, 17);
            this.interruptInputBox.TabIndex = 2;
            this.interruptInputBox.Text = "Interrupt bound keypress";
            this.interruptInputBox.UseVisualStyleBackColor = true;
            this.interruptInputBox.CheckedChanged += new System.EventHandler(this.interruptInputBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of sounds";
            // 
            // numSoundsBox
            // 
            this.numSoundsBox.Location = new System.Drawing.Point(6, 59);
            this.numSoundsBox.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numSoundsBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSoundsBox.Name = "numSoundsBox";
            this.numSoundsBox.ReadOnly = true;
            this.numSoundsBox.Size = new System.Drawing.Size(120, 20);
            this.numSoundsBox.TabIndex = 4;
            this.numSoundsBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSoundsBox.ValueChanged += new System.EventHandler(this.numSoundsBox_ValueChanged);
            // 
            // inputCheckBox
            // 
            this.inputCheckBox.AutoSize = true;
            this.inputCheckBox.Location = new System.Drawing.Point(6, 19);
            this.inputCheckBox.Name = "inputCheckBox";
            this.inputCheckBox.Size = new System.Drawing.Size(146, 17);
            this.inputCheckBox.TabIndex = 5;
            this.inputCheckBox.Text = "Enable input passthrough";
            this.inputCheckBox.UseVisualStyleBackColor = true;
            this.inputCheckBox.CheckedChanged += new System.EventHandler(this.inputCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Channels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Sample rate";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.muteInputCheckBox);
            this.groupBox1.Controls.Add(this.inputSamplerateBox);
            this.groupBox1.Controls.Add(this.inputChannelsBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.inputCheckBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 134);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 150);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input settings";
            // 
            // muteInputCheckBox
            // 
            this.muteInputCheckBox.AutoSize = true;
            this.muteInputCheckBox.Location = new System.Drawing.Point(6, 122);
            this.muteInputCheckBox.Name = "muteInputCheckBox";
            this.muteInputCheckBox.Size = new System.Drawing.Size(158, 17);
            this.muteInputCheckBox.TabIndex = 14;
            this.muteInputCheckBox.Text = "Disable with SoundMachine";
            this.muteInputCheckBox.UseVisualStyleBackColor = true;
            this.muteInputCheckBox.CheckedChanged += new System.EventHandler(this.muteInputCheckBox_CheckedChanged);
            // 
            // inputSamplerateBox
            // 
            this.inputSamplerateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputSamplerateBox.FormattingEnabled = true;
            this.inputSamplerateBox.Items.AddRange(new object[] {
            "44100",
            "48000"});
            this.inputSamplerateBox.Location = new System.Drawing.Point(9, 95);
            this.inputSamplerateBox.Name = "inputSamplerateBox";
            this.inputSamplerateBox.Size = new System.Drawing.Size(121, 21);
            this.inputSamplerateBox.TabIndex = 12;
            this.inputSamplerateBox.SelectedIndexChanged += new System.EventHandler(this.inputSamplerateBox_SelectedIndexChanged);
            // 
            // inputChannelsBox
            // 
            this.inputChannelsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputChannelsBox.FormattingEnabled = true;
            this.inputChannelsBox.Items.AddRange(new object[] {
            "Mono",
            "Stereo"});
            this.inputChannelsBox.Location = new System.Drawing.Point(9, 55);
            this.inputChannelsBox.Name = "inputChannelsBox";
            this.inputChannelsBox.Size = new System.Drawing.Size(121, 21);
            this.inputChannelsBox.TabIndex = 11;
            this.inputChannelsBox.SelectedIndexChanged += new System.EventHandler(this.inputChannelsBox_SelectedIndexChanged);
            // 
            // playbackCheckBox
            // 
            this.playbackCheckBox.AutoSize = true;
            this.playbackCheckBox.Location = new System.Drawing.Point(6, 85);
            this.playbackCheckBox.Name = "playbackCheckBox";
            this.playbackCheckBox.Size = new System.Drawing.Size(183, 17);
            this.playbackCheckBox.TabIndex = 11;
            this.playbackCheckBox.Text = "Duplicate sound to default output";
            this.playbackCheckBox.UseVisualStyleBackColor = true;
            this.playbackCheckBox.CheckedChanged += new System.EventHandler(this.playbackCheckBox_CheckedChanged);
            // 
            // keyPressBox
            // 
            this.keyPressBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyPressBox.FormattingEnabled = true;
            this.keyPressBox.Items.AddRange(new object[] {
            "Stack",
            "Interrupt",
            "StartStop",
            "Loop"});
            this.keyPressBox.Location = new System.Drawing.Point(6, 19);
            this.keyPressBox.Name = "keyPressBox";
            this.keyPressBox.Size = new System.Drawing.Size(121, 21);
            this.keyPressBox.TabIndex = 13;
            this.keyPressBox.SelectedIndexChanged += new System.EventHandler(this.keyPressBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.interruptInputBox);
            this.groupBox2.Location = new System.Drawing.Point(13, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 45);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keypress behavior";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Enable/Disable SoundMachine:";
            // 
            // btnToggleBinding
            // 
            this.btnToggleBinding.Location = new System.Drawing.Point(6, 32);
            this.btnToggleBinding.Name = "btnToggleBinding";
            this.btnToggleBinding.Size = new System.Drawing.Size(75, 23);
            this.btnToggleBinding.TabIndex = 16;
            this.btnToggleBinding.UseVisualStyleBackColor = true;
            this.btnToggleBinding.Click += new System.EventHandler(this.btnToggleBinding_Click);
            // 
            // btnBehaviorBinding
            // 
            this.btnBehaviorBinding.Location = new System.Drawing.Point(6, 74);
            this.btnBehaviorBinding.Name = "btnBehaviorBinding";
            this.btnBehaviorBinding.Size = new System.Drawing.Size(75, 23);
            this.btnBehaviorBinding.TabIndex = 17;
            this.btnBehaviorBinding.UseVisualStyleBackColor = true;
            this.btnBehaviorBinding.Click += new System.EventHandler(this.btnBehaviorBinding_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Change Sound Behavior:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.playbackCheckBox);
            this.groupBox3.Controls.Add(this.keyPressBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.numSoundsBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(193, 116);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sound behavior";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.DeviceOutBox);
            this.groupBox4.Controls.Add(this.DeviceInBox);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Location = new System.Drawing.Point(211, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(242, 158);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sound Devices";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Output Device";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Input Device";
            // 
            // DeviceOutBox
            // 
            this.DeviceOutBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceOutBox.FormattingEnabled = true;
            this.DeviceOutBox.Items.AddRange(new object[] {
            ""});
            this.DeviceOutBox.Location = new System.Drawing.Point(6, 122);
            this.DeviceOutBox.Name = "DeviceOutBox";
            this.DeviceOutBox.Size = new System.Drawing.Size(230, 21);
            this.DeviceOutBox.TabIndex = 19;
            this.DeviceOutBox.SelectedIndexChanged += new System.EventHandler(this.SetOutputDeviceNumber);
            // 
            // DeviceInBox
            // 
            this.DeviceInBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceInBox.FormattingEnabled = true;
            this.DeviceInBox.Location = new System.Drawing.Point(6, 77);
            this.DeviceInBox.Name = "DeviceInBox";
            this.DeviceInBox.Size = new System.Drawing.Size(230, 21);
            this.DeviceInBox.TabIndex = 18;
            this.DeviceInBox.SelectedIndexChanged += new System.EventHandler(this.SetInputDeviceNumber);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.btnRecordBinding);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.btnOverlayBinding);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.btnProfileBinding);
            this.groupBox5.Controls.Add(this.btnToggleBinding);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.btnBehaviorBinding);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(211, 176);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(242, 195);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Keybindings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(125, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Record:";
            // 
            // btnRecordBinding
            // 
            this.btnRecordBinding.Location = new System.Drawing.Point(128, 158);
            this.btnRecordBinding.Name = "btnRecordBinding";
            this.btnRecordBinding.Size = new System.Drawing.Size(75, 23);
            this.btnRecordBinding.TabIndex = 22;
            this.btnRecordBinding.UseVisualStyleBackColor = true;
            this.btnRecordBinding.Click += new System.EventHandler(this.btnRecordBinding_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Toggle Overlay:";
            // 
            // btnOverlayBinding
            // 
            this.btnOverlayBinding.Location = new System.Drawing.Point(6, 158);
            this.btnOverlayBinding.Name = "btnOverlayBinding";
            this.btnOverlayBinding.Size = new System.Drawing.Size(75, 23);
            this.btnOverlayBinding.TabIndex = 21;
            this.btnOverlayBinding.UseVisualStyleBackColor = true;
            this.btnOverlayBinding.Click += new System.EventHandler(this.btnOverlayBinding_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Change Profile:";
            // 
            // btnProfileBinding
            // 
            this.btnProfileBinding.Location = new System.Drawing.Point(6, 116);
            this.btnProfileBinding.Name = "btnProfileBinding";
            this.btnProfileBinding.Size = new System.Drawing.Size(75, 23);
            this.btnProfileBinding.TabIndex = 19;
            this.btnProfileBinding.UseVisualStyleBackColor = true;
            this.btnProfileBinding.Click += new System.EventHandler(this.btnProfileBinding_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(193, 374);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 13);
            this.lblVersion.TabIndex = 18;
            // 
            // btnHomeDir
            // 
            this.btnHomeDir.Location = new System.Drawing.Point(21, 341);
            this.btnHomeDir.Name = "btnHomeDir";
            this.btnHomeDir.Size = new System.Drawing.Size(164, 23);
            this.btnHomeDir.TabIndex = 19;
            this.btnHomeDir.Text = "Set Home Directory";
            this.btnHomeDir.UseVisualStyleBackColor = true;
            this.btnHomeDir.Click += new System.EventHandler(this.btnHomeDir_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 395);
            this.Controls.Add(this.btnHomeDir);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numSoundsBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox interruptInputBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numSoundsBox;
        private System.Windows.Forms.CheckBox inputCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox inputSamplerateBox;
        private System.Windows.Forms.ComboBox inputChannelsBox;
        private System.Windows.Forms.CheckBox playbackCheckBox;
        private System.Windows.Forms.ComboBox keyPressBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnToggleBinding;
        private System.Windows.Forms.Button btnBehaviorBinding;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox DeviceOutBox;
        private System.Windows.Forms.ComboBox DeviceInBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnOverlayBinding;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnProfileBinding;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.CheckBox muteInputCheckBox;
        private System.Windows.Forms.Button btnHomeDir;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnRecordBinding;
    }
}