using System;
using System.Collections.Generic;

namespace DBMediator.Models;

public partial class Maintanenceschedule
{
    public long Uid { get; set; }

    public DateTime Regdate { get; set; }

    public DateTime Startdt { get; set; }

    public DateTime Enddt { get; set; }

    public string Who { get; set; } = null!;

    public string Why { get; set; } = null!;
}
