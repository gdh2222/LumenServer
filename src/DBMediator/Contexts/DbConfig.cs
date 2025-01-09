using Dapper;
using DBMediator.Models.AccountDB;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
namespace DBMediator.Contexts
{
    public class DbConfig
    {
        #region Lazy Singleton
        private static readonly Lazy<DbConfig> _instance = new Lazy<DbConfig>(() => new DbConfig());
        public static DbConfig Instance
        {
            get => _instance.Value;
        }
        #endregion


        #region SingleDB Connection Strings
        public string AccountDBString { get; private set; } = string.Empty;
        public string AdminDBString { get; private set; } = string.Empty;
        #endregion

        #region
        protected List<ConfigSharddb> _shardlist = new List<ConfigSharddb>();
        #endregion

        public void Setup(IConfiguration configuration)
        {
            AccountDBString = configuration.GetConnectionString("AccountDBString") ?? string.Empty;
            AdminDBString = configuration.GetConnectionString("AdminDBString") ?? string.Empty;

            LoadGameDBStrings();
        }

        private void LoadGameDBStrings()
        {
            using (MySqlConnection connection = new MySqlConnection(AccountDBString))
            {
                connection.Open();
                var shardList = connection.Query<ConfigSharddb>("SELECT * FROM config_sharddb;").ToList();
                if(0 >= shardList.Count)
                {
                    throw new InvalidDataException("no rows, accountdb -> gamedb strings [config_db]");
                }

                foreach(var shardInfo in shardList)
                {
                    if( null != shardList.FirstOrDefault(row => row.Uid == shardInfo.Uid))
                    {
                        throw new InvalidDataException($"duplicate shard data, uid:{shardInfo.Uid}");
                    }


                    _shardlist.Add(shardInfo);
                }
            }
        }

        public ConfigSharddb Determine(long accidx)
        {
            long index = accidx % _shardlist.Count;
            return _shardlist.ElementAt((int)index);
        }

        public ConfigSharddb Get(long sharduid)
        {
            return _shardlist.FirstOrDefault(row => row.Uid == sharduid)!;
        }

        public ConfigSharddb GetShardInfo(long accidx)
        {
            long index = accidx % _shardlist.Count;
            return _shardlist.ElementAt((int)index);
        }

    }
}
