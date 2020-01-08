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
            // レジストリ情報読み取り
            string KeyName = "MotoRecoViewer";
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + KeyName, true);
            if (rkey != null)
            {
                string strGoogleMapAPIKey = (string)rkey.GetValue("GoogleMapAPI");
                textGoogleAPIKey.Text = strGoogleMapAPIKey;
            }
        }

        private void FormMapOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            // レジストリ情報書き込み
            string KeyName = "MotoRecoViewer";
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + KeyName, true);
            if (rkey == null)
            {
                rkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + KeyName);
            }

            if (rkey != null)
            {
                rkey.SetValue("GoogleMapAPI", textGoogleAPIKey.Text);
            }
        }
    }
}
