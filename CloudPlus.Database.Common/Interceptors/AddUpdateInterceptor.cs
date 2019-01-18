using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using CloudPlus.Database.Common.Attributes;
using CloudPlus.Database.Common.Extensions;

namespace CloudPlus.Database.Common.Interceptors
{
    public class AddUpdateInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
                return;

            if (interceptionContext.Result is DbInsertCommandTree insertCommand)
            {
                var createDateColumnName = CreateDateAttribute.GetCreateDateColumnName(insertCommand.Target.VariableType.EdmType);
                var updateDateColumnName = UpdateDateAttribute.GetUpdateDateColumnName(insertCommand.Target.VariableType.EdmType);

                interceptionContext.Result = HandleInsertCommand(insertCommand, createDateColumnName, updateDateColumnName);
            }

            if (!(interceptionContext.OriginalResult is DbUpdateCommandTree updateCommand))
                return;

            {
                var updateDateColumnName = UpdateDateAttribute.GetUpdateDateColumnName(updateCommand.Target.VariableType.EdmType);

                interceptionContext.Result = HandleUpdateCommand(updateCommand, updateDateColumnName);
            }
        }

        private static DbCommandTree HandleInsertCommand(DbInsertCommandTree insertCommand, string createDateColumnName, string updateDateColumnName)
        {
            var now = DateTime.UtcNow;

            var setClauses = insertCommand.SetClauses
                .Select(clause => clause.UpdateIfMatch(createDateColumnName, DbExpression.FromDateTime(now)))
                .Select(clause => clause.UpdateIfMatch(updateDateColumnName, DbExpression.FromDateTime(now)))
                .ToList();

            return new DbInsertCommandTree(
                insertCommand.MetadataWorkspace,
                insertCommand.DataSpace,
                insertCommand.Target,
                setClauses.AsReadOnly(),
                insertCommand.Returning);
        }

        private static DbCommandTree HandleUpdateCommand(DbUpdateCommandTree updateCommand, string updateDateColumnName)
        {
            var now = DateTime.UtcNow;

            var setClauses = updateCommand.SetClauses
                .Select(clause => clause.UpdateIfMatch(updateDateColumnName, DbExpression.FromDateTime(now)))
                .ToList();

            return new DbUpdateCommandTree(
                updateCommand.MetadataWorkspace,
                updateCommand.DataSpace,
                updateCommand.Target,
                updateCommand.Predicate,
                setClauses.AsReadOnly(), null);
        }
    }
}
