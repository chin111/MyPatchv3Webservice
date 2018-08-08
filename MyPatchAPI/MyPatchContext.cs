using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyPatchAPI
{
    public class MyPatchContext : DbContext
    {
        public MyPatchContext(string conString)
        {
            var str = Settings.GetConnectionString(conString);
            this.Database.Connection.ConnectionString = str;
        }
    }
}