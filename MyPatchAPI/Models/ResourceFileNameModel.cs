using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPatchAPI.Models
{
    public class ResourceFileNameModel
    {
        public string UserName { get; set; }
        public List<DBFileNameModel> DbFileNames { get; set; }
    }
}