using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPatchAPI.Models
{
    public class ResourceUploadErrorModel
    {
        public string Error { get; set; }
        public string Error_Description { get; set; }
        public string UploadDate { get; set; }
    }
}