using System.Windows.Forms;

namespace SoundMachine
{
    public partial class SetBindingForm : Form
    {
        public int CurrentButton = 0;
        public int RemovedBinding;
        public bool NewBindingSet;
        public static SetBindingForm _currentForm;
        public KeyListener.KeyBinding BindingType;

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
