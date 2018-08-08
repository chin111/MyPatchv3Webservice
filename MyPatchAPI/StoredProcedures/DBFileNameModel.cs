using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPatchAPI
{
    public class DBFileNameModel
    {
        public string File_Type { get; set; }
        public string File_Name { get; set; }
        public Int64 File_Size { get; set; }
        public string Version { get; set; }
        public string Path_HTTP { get; set; }
        public Int32 IsComplete { get; set; }
        public string AppVersion { get; set; }
        public string AppURL { get; set; }
        public int FORCE_UPDATE { get; set; }
    }
}