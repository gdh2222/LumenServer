using DBMediator.Models.Account;
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

        #region
        protected List<ConfigSharddb> _shardlist = new List<ConfigSharddb>();
        #endregion

        public void Setup(string accountDbString)
        {
            LoadGameDBStrings(accountDbString);
        }

        private void LoadGameDBStrings(string dbString)
        {
            using (var dbContext = new DbContextAccount(dbString))
            {
                var shardList = dbContext.ConfigSharddbs.ToList();
                if (0 >= shardList.Count)
                {
                    throw new InvalidDataException("no rows, accountdb -> gamedb strings [config_db]");
                }


                foreach (var shardInfo in shardList)
                {
                    if (null != _shardlist.FirstOrDefault(row => row.Uid == shardInfo.Uid))
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
