using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundMachine
{
    public partial class ProfileForm : Form
    {
        public string NewProfile = "";
        public bool IsRenaming = false;

        public ProfileForm()
        {
            InitializeComponent();
            if(IsRenaming)
            {
                button1.Text = "Rename";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            textBox1.Text = rgx.Replace(textBox1.Text, "");

            if (!Config.CurrentConfig.Profiles.Contains(textBox1.Text) )
            {
                if(!IsRenaming)
                    Config.CurrentConfig.Profiles.Add(textBox1.Text);
                NewProfile = textBox1.Text;
                Close();
            }
            else
                MessageBox.Show("A profile with that name already exists!");
        }
    }
}
