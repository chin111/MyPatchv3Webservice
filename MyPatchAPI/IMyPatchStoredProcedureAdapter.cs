using System.Collections.Generic;

namespace MyPatchAPI
{
    public interface IMyPatchStoredProcedureAdapter
    {
        IEnumerable<TResult> ExecuteStoredProcedure<TResult>(
            string procName,
            ISqlParametersAble sqlParametersObject);
    }
}