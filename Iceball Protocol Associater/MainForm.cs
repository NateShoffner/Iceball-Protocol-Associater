#region

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

#endregion

namespace Iceball_Protocol_Associater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            LoadAssociation();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog() {Description = "Locate Iceball Directory"})
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void LoadAssociation()
        {
            var key = Registry.ClassesRoot.OpenSubKey("iceball\\shell\\open\\command");

            if (key != null)
            {
                var command = key.GetValue(null).ToString();
                var enetEnabled = command.Contains(" -c ");

                var cmdPath = command.Substring(1, command.IndexOf(".exe") + 3);

                checkBox1.Checked = enetEnabled;
                textBox1.Text = Path.GetDirectoryName(cmdPath);
            }
        }

        private void SaveAssocation()
        {
            var enetFlag = checkBox1.Checked ? "-c" : "-C";
            var cmdValue = string.Format("\"{0}\" {1} %1", Path.Combine(textBox1.Text, "iceball.exe"), enetFlag);

            var classKey = Registry.ClassesRoot.CreateSubKey("iceball\\");
            classKey.SetValue(null, "URL:Iceball Protocol Handler");
            classKey.SetValue("URL Protocol", "");

            var commandKey = Registry.ClassesRoot.CreateSubKey("iceball\\shell\\open\\command");
            commandKey.SetValue(null, cmdValue);
        }


        private void RemoveAssociation()
        {
            Registry.ClassesRoot.DeleteSubKeyTree("iceball\\");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveAssociation();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                SaveAssocation();
            }
        }
    }
}