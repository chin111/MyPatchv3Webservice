using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyPatchAPI
{
    public class GetDBFileParams : ISqlParametersAble
    {
        public string UserID { get; set; }
        public string AppID { get; set; }
        public string OS { get; set; }

        public string StoredProcedureName
        {
            get { return "SP_SYS_GET_DB_FILE"; }
        }

        public IEnumerable<SqlParameter> AsSqlParameters()
        {
            return new List<SqlParameter>()
            {
                new SqlParameter("@@USER_ID", UserID),
                new SqlParameter("@@APP_ID", AppID),
                new SqlParameter("@@OS", OS)
            };
        }
    }
}