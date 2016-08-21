using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

namespace Lua_Player_Plus_Builder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Changing title ID to uppercase
            textBox2.Text = textBox2.Text.ToUpper();

            //Save VPK dialog
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = textBox1.Text + ".vpk";
            savefile.Filter = "HENkaku Application File (*.vpk)|*.vpk|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                //Building the VPK
                string appname = textBox1.Text;
                string titleid = textBox2.Text;
                string vpkname = savefile.FileName;

                //Checks for the existence of required programs, if not, unpack them.
                if (!File.Exists("vita-mksfoex.exe"))
                {
                    File.WriteAllBytes("vita-mksfoex.exe", Lua_Player_Plus_Builder.Properties.Resources.vita_mksfoex);
                }

                //Make the param.sfo
                ProcessStartInfo startmksfoex = new ProcessStartInfo();
                startmksfoex.Arguments = "-s TITLE_ID="+ titleid + "00001 \"" + appname + "\" param.sfo";
                startmksfoex.FileName = "vita-mksfoex.exe";
                startmksfoex.WindowStyle = ProcessWindowStyle.Hidden;
                startmksfoex.CreateNoWindow = true;
                int exitCode;
                using (Process proc = Process.Start(startmksfoex))
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }

                //Copy the param.sfo to the LPP dir.
                if (File.Exists("build/sce_sys/param.sfo"))
                    File.Delete("build/sce_sys/param.sfo");
                File.Move("param.sfo", "build/sce_sys/param.sfo");

                //Pack the VPK
                ZipFile.CreateFromDirectory("build", vpkname);
                if (File.Exists(vpkname))
                    MessageBox.Show("Success! Created " + vpkname + ".");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
