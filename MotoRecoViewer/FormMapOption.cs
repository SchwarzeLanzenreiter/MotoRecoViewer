using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MotoRecoViewer
{
    public partial class FormMapOption : Form
    {
        public FormMapOption()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormMapOption_Load(object sender, EventArgs e)
        {
            // Google API Keyのロード
            textGoogleAPIKey.Text = Properties.Settings.Default.GoogleAPI;
        }

        private void FormMapOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Google API Keyのセーブ
            Properties.Settings.Default.GoogleAPI = textGoogleAPIKey.Text;
            Properties.Settings.Default.Save();
        }
    }
}
