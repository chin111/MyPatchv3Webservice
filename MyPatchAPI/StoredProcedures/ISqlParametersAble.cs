using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MyPatchAPI
{
    public interface ISqlParametersAble
    {
        string StoredProcedureName { get; }
        IEnumerable<SqlParameter> AsSqlParameters();
    }
}