using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundMachine
{
    public partial class Overlay : Form
    {
        public static Overlay _currentOverlay;
        Label lblBehavior;
        Label lblProfile;

        public Overlay()
        {
            InitializeComponent();
            Size = new Size(70, 26);
            StartPosition = FormStartPosition.Manual;
            Location = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - Size.Width / 2, 0);
            MinimumSize = new Size(1, 1);
            BackColor = KeyListener._listenerEnabled == true ? Color.Green : Color.PaleVioletRed;

            lblProfile = new Label();
            lblProfile.Size = new Size(Size.Width, 13);
            lblProfile.Location = new Point(0, 0);
            lblProfile.TextAlign = ContentAlignment.MiddleCenter;
            lblBehavior = new Label();
            lblBehavior.Size = new Size(Size.Width, 13);
            lblProfile.Location = new Point(0, 13);
            lblBehavior.TextAlign = ContentAlignment.MiddleCenter;

            Controls.Add(lblBehavior);
            Controls.Add(lblProfile);
            _currentOverlay = this;
        }

        public void UpdateProfileText()
        {
            lblProfile.Text = SoundProfile.CurrentSoundProfile.ProfileName;
        }

        public void UpdateBehaviorText()
        {
            lblBehavior.Text = Config._currentConfig.InputMode.ToString();
        }

        public void UpdateStatusColor()
        {
            BackColor = KeyListener._listenerEnabled == true ? Color.Green : Color.PaleVioletRed;
        }
    }
}
