﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMediator.Contexts
{
    public class DbContextGame : DbContext
    {
        public DbContextGame(string connectionString) 
            : base(connectionString)
        {
        }
    }
}
