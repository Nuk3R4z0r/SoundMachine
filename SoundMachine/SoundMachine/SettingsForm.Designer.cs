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
            this.button1 = new System.Windows.Forms.Button();
            this.interruptInputBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numSoundsBox = new System.Windows.Forms.NumericUpDown();
            this.inputCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.inputChannelsBox = new System.Windows.Forms.ComboBox();
            this.inputSamplerateBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numSoundsBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Reload sound devices";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // interruptInputBox
            // 
            this.interruptInputBox.AutoSize = true;
            this.interruptInputBox.Location = new System.Drawing.Point(13, 42);
            this.interruptInputBox.Name = "interruptInputBox";
            this.interruptInputBox.Size = new System.Drawing.Size(124, 17);
            this.interruptInputBox.TabIndex = 2;
            this.interruptInputBox.Text = "Interrupt bound input";
            this.interruptInputBox.UseVisualStyleBackColor = true;
            this.interruptInputBox.CheckedChanged += new System.EventHandler(this.interruptInputBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of sounds";
            // 
            // numSoundsBox
            // 
            this.numSoundsBox.Location = new System.Drawing.Point(188, 29);
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
            this.inputCheckBox.Location = new System.Drawing.Point(13, 66);
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
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Channels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Sample rate";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.inputSamplerateBox);
            this.groupBox1.Controls.Add(this.inputChannelsBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 110);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input settings";
            // 
            // inputChannelsBox
            // 
            this.inputChannelsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputChannelsBox.FormattingEnabled = true;
            this.inputChannelsBox.Items.AddRange(new object[] {
            "Mono",
            "Stereo"});
            this.inputChannelsBox.Location = new System.Drawing.Point(9, 32);
            this.inputChannelsBox.Name = "inputChannelsBox";
            this.inputChannelsBox.Size = new System.Drawing.Size(121, 21);
            this.inputChannelsBox.TabIndex = 11;
            this.inputChannelsBox.SelectedIndexChanged += new System.EventHandler(this.inputChannelsBox_SelectedIndexChanged);
            // 
            // inputSamplerateBox
            // 
            this.inputSamplerateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputSamplerateBox.FormattingEnabled = true;
            this.inputSamplerateBox.Items.AddRange(new object[] {
            "44100",
            "48000"});
            this.inputSamplerateBox.Location = new System.Drawing.Point(9, 72);
            this.inputSamplerateBox.Name = "inputSamplerateBox";
            this.inputSamplerateBox.Size = new System.Drawing.Size(121, 21);
            this.inputSamplerateBox.TabIndex = 12;
            this.inputSamplerateBox.SelectedIndexChanged += new System.EventHandler(this.inputSamplerateBox_SelectedIndexChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 263);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.inputCheckBox);
            this.Controls.Add(this.numSoundsBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.interruptInputBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numSoundsBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}