using Dapper;
using DBMediator.Models.AccountDB;

namespace DBMediator.Contexts
{
    public class DbContextAccount : DbContext
    {
        public DbContextAccount() 
            : base(DbConfig.Instance.AccountDBString)
        {
        }

        public Accountsharddblink GetShardLink(long accidx)
        {
            return _connection.Query<Accountsharddblink>("SELECT * FROM accountsharddblink WHERE accidx = @_accidx", new { _accidx = accidx }).FirstOrDefault()!;
        }

        public int AddShardLink(long accidx, long chooseuid)
        {
            return _connection.Query<int>("INSERT accountsharddblink(accidx, shard_uid) VALUES( @_accidx, @_uid )", new { _accidx = accidx, _uid = chooseuid }).FirstOrDefault();
        }

        public void AddAccountProperties(long accidx, string nickname, int changeable)
        {
            _connection.Query<int>("INSERT accountproperties(accidx,nickname,changeable_nick,recent_nick_dt) VALUES( @_accidx, @_nickname, @_changeable, NOW() )", new { _accidx = accidx, _nickname = nickname, _changeable = changeable }).FirstOrDefault();
        }

        public Accountmember CommonLogin(int platformType, int marketType, string acctoken, ref bool isNewJoin)
        {
            isNewJoin = false;

            long accidx = _connection.Query<long>("SELECT accidx FROM accountcredentiallink where cdtype = @_platformType and acctoken = @_acctoken", new { _platformType = (int)platformType, _acctoken = acctoken }).FirstOrDefault();
            if (0 < accidx)
            {
                int rt = _connection.QueryFirstOrDefault<int>("UPDATE accountcredentiallink set recent_dt = NOW() WHERE accidx = @_accidx; UPDATE accountmember set last_login_dt = NOW() WHERE idx = @_accidx;", new { _accidx = accidx });
            }
            else
            {
                accidx = _connection.Query<long>("INSERT accountmember(stateflag, create_dt, last_login_dt, mkt) VALUES(0, NOW(), NOW(), @_mkt); SELECT LAST_INSERT_ID();", new { _mkt = marketType }).FirstOrDefault();
                int rt = _connection.Query<int>("INSERT accountcredentiallink(accidx, cdtype, acctoken, creation_dt, recent_dt) VALUES(@_accidx, @_platformType, @_token, NOW(), NOW())", new { _accidx = accidx, CTYPE = platformType, _token = acctoken }).FirstOrDefault();
                isNewJoin = true;
            }

            return _connection.Query<Accountmember>("SELECT * FROM accountmember where idx = @_accidx", new { _accidx = accidx }).FirstOrDefault()!;
        }

        public Accountrestriction GetAccountRestric(long accidx)
        {
            return _connection.Query<Accountrestriction>(@"SELECT ridx, accidx, restic_type, stringkey, enddate from accountrestriction where accidx = @_accidx and now() <= enddate;",
                                        new { _accidx = accidx }).FirstOrDefault()!;
        }

        public void RefreshSessionKey(long accidx, string skey)
        {
            _connection.Execute("INSERT INTO accountsessionkey VALUES(@_accidx, @_sessionKey) ON DUPLICATE KEY UPDATE skey = @_sessionKey;", new { _accidx = accidx, _sessionKey = skey });
        }
    }
}
