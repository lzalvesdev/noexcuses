using NoExcusesFit.Domain.Interfaces;
using System.Data;

namespace NoExcusesFit.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _context;
        private bool _wasCommitted = false;
        private bool _disposed = false;

        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(DapperContext context)
        {
            _context = context;
        }

        public void Begin()
        {
            Connection = _context.CreateConnection();
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            _wasCommitted = true;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (!_wasCommitted)
                Transaction?.Rollback();

            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}
