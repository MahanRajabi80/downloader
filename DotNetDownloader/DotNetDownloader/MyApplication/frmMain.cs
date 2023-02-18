using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyApplication
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuNewDownload_Click(object sender, EventArgs e)
        {
            frmNewDownload newDownload = new frmNewDownload(this);
            newDownload.ShowDialog();
        }

        public void SetListViewItems(string DownloadUrl, string FilePath)
        {
            ListViewItem lvw = new ListViewItem(DownloadUrl);
            lvw.SubItems.Add(FilePath);
            lvw.SubItems.Add("0 KB");
            lvw.SubItems.Add("0 KB");
            lvw.SubItems.Add("0 KB");
            lvw.SubItems.Add("?");

            lvw.Tag = DownloadUrl;

            listView1.Items.Add(lvw);
        }

        public void UpdateList(string dlLink)
        {
            Thread thread = new Thread(() => RefreshListView(dlLink));
            thread.IsBackground = true;
            thread.Start();
        }

        public void RefreshListView(string link)
        {
            var state = DownloadFile.downloadDetails[link].State;

            while (state == DownloadState.Downloading)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (link == listView1.Items[i].Tag.ToString())
                        {
                            ListViewItem item = listView1.Items[i];

                            DownloadDetails d = DownloadFile.downloadDetails[link];

                            item.SubItems[2].Text = d.TotalSize.ToString();
                            item.SubItems[3].Text = d.DownloadedSize.ToString();
                            item.SubItems[4].Text = d.DownloadSpeed.ToString();
                            item.SubItems[5].Text = d.State.ToString();
                        }
                    }
                }));
                Thread.Sleep(1000);
            }
        }
    }
}
