using DSELN.Cmm.Exceptions;
using System.Transactions;

namespace DSELN.Cmm.Helper
{
    public static class TransactionHelper
    {
        public static void SetRollbackOnly(string message)
        {
            var _transactionScope = new TransactionScope();
            _transactionScope.Dispose();

            throw new TransactionAbortByBizLogException(message);
        }

    }

}
