using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMediator.Contexts
{
    public class DbContextAdmin : DbContext
    {
        public DbContextAdmin() 
            : base(DbConfig.Instance.AdminDBString)
        {
        }

        public 

    }
}
