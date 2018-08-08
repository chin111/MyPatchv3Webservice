using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPatchAPI
{
    public class VerifyLoginModel
    {
        public string USER_ID { get; set; }
        public string USER_TYPE { get; set; }
        public string User_Msg { get; set; }
        public int IsActive { get; set; }
    }
}