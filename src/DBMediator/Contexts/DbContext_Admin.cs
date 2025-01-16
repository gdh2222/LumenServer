using Dapper;
using DBMediator.Models.Admin;

namespace DBMediator.Contexts
{

// allow return null
#pragma warning disable CS8603
    public class DbContextAdmin : DbContext
    {
        public DbContextAdmin() 
            : base(DbConfig.Instance.AdminDBString)
        {
        }
        
        public string GetGameVersionByMarket(int marketType)
        {
            return _connection.Query<string>($"SELECT validver FROM appvalidversion WHERE mkt = @_mkt;", new { _mkt = marketType }).FirstOrDefault() ?? string.Empty;
        }


        public Redirectionsinfo GetRedirectionInfo(int marketType, string version)
        {
            return _connection.Query<Redirectionsinfo>($"SELECT * FROM redirectionsinfo WHERE mkt = _mkt AND version = @_ver", new { _mkt = marketType, _ver = version }).FirstOrDefault();
        }

    }

#pragma warning restore CS8600
}
