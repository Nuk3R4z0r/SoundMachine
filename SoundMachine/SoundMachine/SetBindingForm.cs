using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SoundMachine
{
    public partial class SetBindingForm : Form
    {
        public static int CurrentButton = 0;
        public static int RemovedBinding;
        public static bool NewBindingSet;
        public static SetBindingForm _currentForm;


        public SetBindingForm()
        {
            InitializeComponent();
            RemovedBinding = -1;
            KeyListener.changeBinding = true;
            NewBindingSet = false;
            FormClosing += new FormClosingEventHandler(Cleanup);
            _currentForm = this;
        }

        private void Cleanup(object sender, FormClosingEventArgs e)
        {
            KeyListener.changeBinding = false;
        }
    }
}
