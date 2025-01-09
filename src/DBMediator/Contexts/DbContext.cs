using MySqlConnector;

namespace DBMediator.Contexts
{
    public class DbContext : IDisposable
    {
        private readonly MySqlConnection _connection;
        private MySqlTransaction _transaction;

        public DbContext(string connectionString)
        {
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("[DbConfig NotSetup] connectionString empty string");
            }

            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            _transaction = null!;
        }

        public void Dispose()
        {
            if(null != _transaction)
            {
                RollbackTransaction();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }

            GC.SuppressFinalize(this);  // 객체 소멸자를 호출하지 않도록 설정
        }


        // 소멸자 (optional, 필요 시)
        ~DbContext()
        {
            Dispose();
        }

        public void BeginTransaction(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted)
        {
            if(_connection.State != System.Data.ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection is not open.");
            }

            _transaction = _connection.BeginTransaction(isolationLevel);
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null!;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null!;
        }

    }
}
