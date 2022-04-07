using System.IO;

namespace Findin
{
    public partial class MainWindow : Form
    {
        private string Path = "";
        private const string FileToLookForSearchDirectory = "config.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Path = File.ReadAllText(FileToLookForSearchDirectory).Trim();

                if (string.IsNullOrEmpty(Path))
                {
                    MessageBox.Show(text: "The file config.txt did not have any content.", caption: "title", buttons: MessageBoxButtons.OK);
                    Application.Exit();
                }
                    
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(text: "Did not find 'config.txt' file in the same directory with the path to look for.", caption: "title", buttons: MessageBoxButtons.OK);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(text: $"An error occurred while booting up the program. Please report the following error: {ex.InnerException?.Message ?? ex.Message}", caption: "title", buttons: MessageBoxButtons.OK);
                Application.Exit();
            }
        }
    }
}