using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using CloudPlus.Database.Common.Attributes;

namespace CloudPlus.Database.Common.Interceptors
{
    public class DeleteQueryVisitor : DefaultExpressionVisitor
    {
        public override DbExpression Visit(DbScanExpression expression)
        {
            var column = SoftDeleteAttribute.GetSoftDeleteColumnName(expression.Target.ElementType);

            if (column == null)
                return base.Visit(expression);

            var table = (EntityType)expression.Target.ElementType;

            if (table.Properties.All(p => p.Name != column))
                return base.Visit(expression);

            var binding = expression.Bind();
            return binding.Filter(binding.VariableType.Variable(binding.VariableName).Property(column)
                .NotEqual(DbExpression.FromBoolean(true)));
        }
    }
}
