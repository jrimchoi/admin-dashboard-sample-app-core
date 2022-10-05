using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Transactions;

namespace DSELN.Cmm.Filters
{
    //filter factory is used in order to create new filter instance per request
    public class TransactionalAttribute : Attribute, IFilterFactory
    {
        //make sure filter marked as not reusable
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //Console.WriteLine("TransactionalFilter 1");
            return new TransactionalFilter();
        }

        private class TransactionalFilter : IActionFilter
        {
            private TransactionScope _transactionScope;

            public void OnActionExecuting(ActionExecutingContext context)
            {
                //Console.WriteLine("TransactionalFilter 2");
                _transactionScope = new TransactionScope();
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                //if no exception were thrown
                if (context.Exception == null)
                {
                    _transactionScope.Complete();
                    _transactionScope.Dispose(); // ### important 
                    Console.WriteLine("TransactionalFilter Complete OK ");
                }
                else
                {
                    _transactionScope.Dispose(); // ### important 
                    Console.WriteLine("TransactionalFilter rollback : " + context.Exception.Message);
                }
            }
        }
    }
}
