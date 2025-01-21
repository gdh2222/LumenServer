using System;
using System.Collections.Generic;

namespace DBMediator.Models;

public partial class Redirectionsinfo
{
    public long Ridx { get; set; }

    public int Mkt { get; set; }

    public string Mktname { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string Authurl { get; set; } = null!;
}
