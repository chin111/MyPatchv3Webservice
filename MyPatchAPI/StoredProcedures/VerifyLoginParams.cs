using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyPatchAPI
{
    public class VerifyLoginParams : ISqlParametersAble
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string MacAddr { get; set; }

        public string StoredProcedureName
        {
            get { return "SP_SYS_VERIFY_LOGIN"; }
        }
        public IEnumerable<SqlParameter> AsSqlParameters()
        {
            return new List<SqlParameter>()
            {
                new SqlParameter("@@USER_ID", UserID),
                new SqlParameter("@@PASSWORD", Password),
                new SqlParameter("@@MAC_ADDR", MacAddr)
            };
        }
    }
}