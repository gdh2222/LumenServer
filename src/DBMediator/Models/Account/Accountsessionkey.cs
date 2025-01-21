using System;
using System.Collections.Generic;

namespace DBMediator.Models.Account;

public partial class Accountsessionkey
{
    public long Accidx { get; set; }

    public string Skey { get; set; } = null!;
}
