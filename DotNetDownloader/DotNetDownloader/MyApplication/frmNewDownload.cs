using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MyApplication
{
    public partial class frmNewDownload : Form
    {
        public frmMain MainForm { get; set; }

        public frmNewDownload(frmMain mainForm)
        {
            MainForm = mainForm;

            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "لطفا محل ذخیره سازی فایل را انتخاب کنید";
            saveFile.FileName = Path.GetFileName(txtDownloadLink.Text);
            saveFile.ShowDialog();
            txtPath.Text = saveFile.FileName;
        }

        private void ckhDownloadLater_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDownloadLater.Checked == true)
            {
                txtTime.Enabled = true;
                txtDate.Enabled = true;
            }
            else
            {
                txtTime.Enabled = false;
                txtDate.Enabled = false;
            }
        }

        private void frmNewDownload_Load(object sender, EventArgs e)
        {
            try
            {
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtTime.Text = DateTime.Now.ToShortTimeString();

                if (Clipboard.ContainsText())
                {
                    Uri fileuri = new Uri(Clipboard.GetText());

                    if(fileuri.Scheme == Uri.UriSchemeHttp || fileuri.Scheme == Uri.UriSchemeHttps)
                        txtDownloadLink.Text = Clipboard.GetText();
                }
            }
            catch { }
        }

        private void btnStartDownload_Click(object sender, EventArgs e)
        {
            if (txtDownloadLink.Text != "")
            {
                //Download Codes
                if (txtPath.Text != "")
                {
                    MainForm.SetListViewItems(txtDownloadLink.Text, txtPath.Text);

                    (new frmDownloading(MainForm, txtDownloadLink.Text, txtPath.Text)).Show();
                    Close();
                }
                else
                    MessageBox.Show("محل ذخیره سازی فایل انتخاب نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("لینک دانلود فایل خالی می باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
