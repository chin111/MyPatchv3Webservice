using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MyPatchAPI
{
    public class MyPatchStoredProcedureAdapter : IMyPatchStoredProcedureAdapter
    {
        private readonly MyPatchContext DBContext;

        public MyPatchStoredProcedureAdapter()
        {
            this.DBContext = new MyPatchContext("MyPatchContext");
        }

        public IEnumerable<TResult> ExecuteStoredProcedure<TResult>(
            string procName,
            ISqlParametersAble sqlParametersObject)
        {
            var sqlParameters = sqlParametersObject != null
                ? sqlParametersObject.AsSqlParameters()
                : new SqlParameter[0];

            var sqlStatement = this.CreateSPCommand(procName, sqlParameters);

            return DBContext.Database.SqlQuery<TResult>(sqlStatement, sqlParameters.ToArray());
        }

        private string CreateSPCommand(string procName, IEnumerable<SqlParameter> sqlParameters)
        {
            var queryString = string.Format("{0}", procName);
            sqlParameters.ToList().ForEach(x => queryString = string.Format("{0} {1},", queryString, x.ParameterName));

            return queryString.TrimEnd(',');
        }
    }
}