using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApplication
{
    public enum DownloadState
    {
        NotStarted,
        Downloading,
        DownloadCompleted,
        DownloadError
    }

    public class DownloadDetails
    {
        public string DownloadUrl { get; set; }
        public string SavePath { get; set; }
        public int TotalSize { get; set; }
        public int DownloadedSize { get; set; }
        public int DownloadSpeed { get; set; }
        public DownloadState State { get; set; }
    }

    public class DownloadFile
    {
        public static Dictionary<string, DownloadDetails> downloadDetails = new Dictionary<string, DownloadDetails>();
    }
}
