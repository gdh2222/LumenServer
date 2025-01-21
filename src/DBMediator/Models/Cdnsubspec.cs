using System;
using System.Collections.Generic;

namespace DBMediator.Models;

public partial class Cdnsubspec
{
    public int Mkt { get; set; }

    public string Mktname { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string Subfolder { get; set; } = null!;
}
