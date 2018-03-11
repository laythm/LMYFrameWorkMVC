using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.DAL
{
    public class SoftDeleteInterceptor : IDbCommandTreeInterceptor
    {
        public const string IsDeletedColumnName = "IsDeleted";

        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            //context
           // bool shouldFetchSoftDeleted = LMY_FrameWork_MVCEntities != null && context.ShouldFetchSoftDeleted;

            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
            {
                return;
            }

            var queryCommand = interceptionContext.Result as DbQueryCommandTree;
            if (queryCommand != null)
            {
                interceptionContext.Result = HandleQueryCommand(queryCommand, false);
            }

            var deleteCommand = interceptionContext.OriginalResult as DbDeleteCommandTree;
            if (deleteCommand != null)
            {
                interceptionContext.Result = HandleDeleteCommand(deleteCommand);
            }
        }

        private static DbCommandTree HandleDeleteCommand(DbDeleteCommandTree deleteCommand)
        {
            var setClauses = new List<DbModificationClause>();
            var table = (EntityType)deleteCommand.Target.VariableType.EdmType;

            if (table.Properties.All(p => p.Name != IsDeletedColumnName))
            {
                return deleteCommand;
            }

            setClauses.Add(DbExpressionBuilder.SetClause(
                deleteCommand.Target.VariableType.Variable(deleteCommand.Target.VariableName).Property(IsDeletedColumnName),
                DbExpression.FromBoolean(true)));

            return new DbUpdateCommandTree(
                deleteCommand.MetadataWorkspace,
                deleteCommand.DataSpace,
                deleteCommand.Target,
                deleteCommand.Predicate,
                setClauses.AsReadOnly(), null);
        }

        private static DbCommandTree HandleQueryCommand(DbQueryCommandTree queryCommand,bool EnableSoftDeleteQuery )
        {
            var newQuery = queryCommand.Query.Accept(new SoftDeleteQueryVisitor());
            return new DbQueryCommandTree(
                queryCommand.MetadataWorkspace,
                queryCommand.DataSpace,
                newQuery);
        }

        public class SoftDeleteQueryVisitor : DefaultExpressionVisitor
        {
            public SoftDeleteQueryVisitor(bool EnableSoftDeleteQuery = false)
            {
                this.EnableSoftDeleteQuery = EnableSoftDeleteQuery;
            }
            private bool EnableSoftDeleteQuery;

            public override DbExpression Visit(DbScanExpression expression)
            {
                var table = (EntityType)expression.Target.ElementType;
                if (table.Properties.All(p => p.Name != IsDeletedColumnName) || this.EnableSoftDeleteQuery)
                {
                    return base.Visit(expression);
                }

                var binding = expression.Bind();
                return binding.Filter(
                    binding.VariableType
                        .Variable(binding.VariableName)
                        .Property(IsDeletedColumnName)
                        .NotEqual(DbExpression.FromBoolean(true)));
            }
        }
    }
}
