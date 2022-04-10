namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        private string Path = "";

        public MainWindowBackend()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Path = "D:\\repos\\Findin\\Findin";
        }
    }
}