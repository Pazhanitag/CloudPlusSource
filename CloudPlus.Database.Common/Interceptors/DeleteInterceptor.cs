using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using CloudPlus.Database.Common.Attributes;

namespace CloudPlus.Database.Common.Interceptors
{
    public class DeleteInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
                return;

            if (interceptionContext.Result is DbQueryCommandTree queryCommand)
            {
                var newQuery = queryCommand.Query.Accept(new DeleteQueryVisitor());
                interceptionContext.Result = new DbQueryCommandTree(
                    queryCommand.MetadataWorkspace,
                    queryCommand.DataSpace,
                    newQuery);
            }

            if (!(interceptionContext.OriginalResult is DbDeleteCommandTree deleteCommand))
                return;

            var column = SoftDeleteAttribute.GetSoftDeleteColumnName(deleteCommand.Target.VariableType.EdmType);

            if (column == null)
                return;

            var setClauses = new List<DbModificationClause>();
            var table = (EntityType)deleteCommand.Target.VariableType.EdmType;

            if (table.Properties.Any(p => p.Name == column))
                setClauses.Add(DbExpressionBuilder.SetClause(
                    deleteCommand.Target.VariableType.Variable(deleteCommand.Target.VariableName).Property(column),
                    DbExpression.FromBoolean(true)));

            var update = new DbUpdateCommandTree(
                deleteCommand.MetadataWorkspace,
                deleteCommand.DataSpace,
                deleteCommand.Target,
                deleteCommand.Predicate,
                setClauses.AsReadOnly(),
                null);

            interceptionContext.Result = update;
        }
    }
}
