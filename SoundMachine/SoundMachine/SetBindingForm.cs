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
        public static int currentButton = 0;
        public static bool isListening;
        public static bool newBindingSet;

        public SetBindingForm()
        {
            InitializeComponent();
            isListening = true;
            KeyLogger.changeBinding = true;
            newBindingSet = false;
            this.FormClosing += new FormClosingEventHandler(Cleanup);
            Thread t = new Thread(WaitForExit);
            t.Start();
        }

        private void WaitForExit()
        {
            while (isListening)
                Thread.Sleep(10);

            BeginInvoke(new MethodInvoker(delegate
            {
                Close();
            }));
        }

        private void Cleanup(object sender, FormClosingEventArgs e)
        {
            KeyLogger.changeBinding = false;
            isListening = false;
        }
    }
}
