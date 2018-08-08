using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyPatchAPI
{
    public class EmployeeListParams : ISqlParametersAble
    {
        public string SupervisorID { get; set; }

        public string StoredProcedureName
        {
            get { return "SP_SYS_GET_EMP_LIST"; }
        }
        public IEnumerable<SqlParameter> AsSqlParameters()
        {
            return new List<SqlParameter>()
            {
                new SqlParameter("@@SUP_ID", SupervisorID)
            };
        }
    }
}