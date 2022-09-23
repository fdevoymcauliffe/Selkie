using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Usermanual : Form
    {
        public Usermanual()
        {
            InitializeComponent();
            OpenGuide();
        }

        private void OpenGuide()
        {
            string installPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(installPath, "SELKIE_logistics_tool_user_guide.pdf");
            webBrowser1.Navigate(filePath);
        }
    }
}
