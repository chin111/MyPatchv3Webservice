using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyPatchAPI
{
    public class AddUploadLogParams : ISqlParametersAble
    {
        public string Upload_Date { get; set; }
        public string Login_ID { get; set; }
        public string Status { get; set; }
        public string File_Path { get; set; }
        public float Total_Time { get; set; }
        public string App_Version { get; set; }
        public string OS_Type { get; set; }
        public string OS_Version { get; set; }
        public string MacAddr { get; set; }

        public string StoredProcedureName
        {
            get { return "SP_SYS_ADD_UPLOAD_LOG"; }
        }

        public IEnumerable<SqlParameter> AsSqlParameters()
        {
            return new List<SqlParameter>()
            {
                new SqlParameter("@@Upload_Date", Upload_Date),
                new SqlParameter("@@LOGIN_ID", Login_ID),
                new SqlParameter("@@Status", Status),
                new SqlParameter("@@Total_Time", Total_Time),
                new SqlParameter("@@File_Path", File_Path),
                new SqlParameter("@@App_Version", App_Version),
                new SqlParameter("@@OS_Type", OS_Type),
                new SqlParameter("@@OS_Version", OS_Version),
                new SqlParameter("@@MAC_ADDR", MacAddr)
            };
        }
    }
}