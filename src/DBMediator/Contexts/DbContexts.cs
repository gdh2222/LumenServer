using DBMediator.Models;
using DBMediator.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DBMediator.Contexts
{
    public static class ServiceExtension
    {
        public static string ConnectionString(this IServiceProvider provider, string key)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return configuration.GetConnectionString(key)!;
        }
    }




    public class SingleDbContext : DbContext
    {
        protected string _dbString = string.Empty;

        public SingleDbContext(string dbString)
        {
            _dbString = dbString;
        }

        public SingleDbContext(IServiceProvider serviceProvider, string key)
        {
            _dbString = serviceProvider.ConnectionString(key);
        }

        public async Task<DateTime> Now()
        {
            return await Database.SqlQueryRaw<DateTime>("SELECT NOW();").SingleAsync();
        }
    }

    public class ShardDbContext : DbContext
    {
        protected string _dbString = string.Empty;
        public ShardDbContext(string connectionString)
        {
            _dbString = connectionString;
        }
    }




    public class DbContextAdmin : SingleDbContext
    {
        protected static readonly string StringKey = "AdminDBString";

        public virtual DbSet<Appvalidversion> Appvalidversions { get; set; }

        public virtual DbSet<Cdnsubspec> Cdnsubspecs { get; set; }

        public virtual DbSet<Maintanenceschedule> Maintanenceschedules { get; set; }

        public virtual DbSet<Redirectionsinfo> Redirectionsinfos { get; set; }


        public DbContextAdmin(IServiceProvider serviceProvider)
            : base(serviceProvider, StringKey)
        {
        }

        /// <summary>
        ///  DbContext 가 생성될때 호출
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_dbString);
        }

    }

    public class DbContextAccount : SingleDbContext
    {
        protected static readonly string StringKey = "AccountDBString";

        public virtual DbSet<Accountcredentiallink> Accountcredentiallinks { get; set; }

        public virtual DbSet<Accountmember> Accountmembers { get; set; }

        public virtual DbSet<Accountproperty> Accountproperties { get; set; }

        public virtual DbSet<Accountrestriction> Accountrestrictions { get; set; }

        public virtual DbSet<Accountsessionkey> Accountsessionkeys { get; set; }

        public virtual DbSet<Accountsharddblink> Accountsharddblinks { get; set; }

        public virtual DbSet<ConfigSharddb> ConfigSharddbs { get; set; }


        public DbContextAccount(string dbString)
            : base(dbString)
        {

        }


        public DbContextAccount(IServiceProvider serviceProvider)
            : base(serviceProvider, StringKey)
        {
        }

        /// <summary>
        ///  DbContext 가 생성될때 호출
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_dbString);
        }
    }

    public class DbContextGame : ShardDbContext
    {
        public DbContextGame(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_dbString);
        }
    }

    public class DbContextLog : ShardDbContext
    {
        public DbContextLog(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_dbString);
        }
    }


    public class DbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Create<T>() where T : SingleDbContext
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(IServiceProvider) });

            if(null != constructor)
            {
                return (T)constructor.Invoke(new object[] { _serviceProvider });
            }
            else
            {
                throw new InvalidOperationException("No matching constructor found. [Create<T> SingleDbContext]");
            }
        }

        public T Create<T>(string connectString) where T : ShardDbContext
        {
            var constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });

            if (null != constructor)
            {
                return (T)constructor.Invoke(new object[] { connectString });
            }
            else
            {
                throw new InvalidOperationException("No matching constructor found. [Create<T> ShardDbContext]");
            }
        }
    }
}
