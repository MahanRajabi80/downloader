using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApplication
{
    public partial class frmDownloading : Form
    {
        public frmMain MainForm { get; set; }
        public string downloadLink { get; set; }
        public string savePath { get; set; }

        public frmDownloading(frmMain mainForm, string DownloadLink, string SavePath)
        {
            MainForm = mainForm;
            downloadLink = DownloadLink;
            savePath = SavePath;

            InitializeComponent();

            //Thread myThread = new Thread(() => DownloadData(DownloadLink, SavePath));
            //myThread.Start();

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(downloadLink);

            int lenght = Convert.ToInt32(client.ResponseHeaders["Content-Length"]);
            int Downloaded = 0;

            byte[] buffer = new byte[8192];

            lblName.Invoke(new MethodInvoker(delegate { lblName.Text = Path.GetFileName(savePath); }));
            lblTotalSize.Invoke(new MethodInvoker(delegate { lblTotalSize.Text = (lenght / 1024).ToString() + " کیلوبایت"; }));
            lblDateTime.Invoke(new MethodInvoker(delegate { lblDateTime.Text = DateTime.Now.ToString(); }));

            DownloadDetails downloadDetails = new DownloadDetails()
            {
                DownloadUrl = downloadLink,
                SavePath = savePath,
                DownloadedSize = 0,
                DownloadSpeed = 0,
                State = DownloadState.NotStarted,
                TotalSize = lenght
            };

            DownloadFile.downloadDetails.Add(downloadLink, downloadDetails);

            MainForm.UpdateList(downloadLink);

            Stopwatch Duration = new Stopwatch();
            Duration.Start();
            int DownloadSpeed = 0;

            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                while (Downloaded < lenght)
                {
                    DownloadFile.downloadDetails[downloadLink].State = DownloadState.Downloading;

                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, bytes);

                    Downloaded = Downloaded + bytes;

                    DownloadFile.downloadDetails[downloadLink].DownloadedSize = Downloaded;

                    lblDownloadedSize.Invoke(new MethodInvoker(delegate { lblDownloadedSize.Text = (Downloaded / 1024).ToString() + " کیلوبایت"; }));
                    int percent = (Downloaded * 100) / lenght;
                    int Elapsed = Convert.ToInt32(Duration.Elapsed.TotalSeconds);
                    if (Elapsed > 0)
                    {
                        DownloadSpeed = Downloaded / Elapsed;

                        DownloadFile.downloadDetails[downloadLink].DownloadSpeed = DownloadSpeed;

                        string speed = DownloadSpeed / 1024 + " KB/s";
                        label6.Invoke(new MethodInvoker(delegate { label6.Text = speed; }));
                    }

                    lblPercent.Invoke(new MethodInvoker(delegate { lblPercent.Text = percent.ToString(); }));
                    progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = percent; }));
                }
            }
            DownloadFile.downloadDetails[downloadLink].State = DownloadState.DownloadCompleted;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (e.Cancelled)
            {

            }
            else
            {
                //Form view for Download Completed
                MessageBox.Show("اتمام دانلود فایل", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void DownloadData(string downloadLink, string savePath)
        {
            
        }
    }
}
