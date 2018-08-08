using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyPatchAPI
{
    public class AddDownloadLogParams : ISqlParametersAble
    {
        public string Download_Date { get; set; }
        public string Login_ID { get; set; }
        public string Status { get; set; }
        public string DB_Path { get; set; }
        public float Total_Time { get; set; }
        public string App_Version { get; set; }
        public string OS_Type { get; set; }
        public string OS_Version { get; set; }
        public string MacAddr { get; set; }

        public string StoredProcedureName
        {
            get { return "SP_SYS_ADD_DOWNLOAD_LOG"; }
        }

        public IEnumerable<SqlParameter> AsSqlParameters()
        {
            return new List<SqlParameter>()
            {
                new SqlParameter("@@DOWNLOAD_Date", Download_Date),
                new SqlParameter("@@LOGIN_ID", Login_ID),
                new SqlParameter("@@Status", Status),
                new SqlParameter("@@Total_Time", Total_Time),
                new SqlParameter("@@DB_Path", DB_Path),               
                new SqlParameter("@@App_Version", App_Version),
                new SqlParameter("@@OS_Type", OS_Type),
                new SqlParameter("@@OS_Version", OS_Version),
                new SqlParameter("@@MAC_ADDR", MacAddr)
            };
        }
    }
}