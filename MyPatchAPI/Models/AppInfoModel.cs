using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPatchAPI.Models
{
    public class AppInfoModel
    {
        public string Id { get; set; }
        public string ClientOS { get; set; }
        public string Version { get; set; }
        public string AppStoreURL { get; set; }
        public string ForceUpdate { get; set; }
        public string Remark { get; set; }
    }
}