using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMediator.Contexts
{
    public class DbContextLog : DbContext
    {
        public DbContextLog(string connectionString) 
            : base(connectionString)
        {
        }
    }
}
