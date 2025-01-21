using System;
using System.Collections.Generic;

namespace DBMediator.Models.Account;

/// <summary>
/// 유저 제재
/// </summary>
public partial class Accountrestriction
{
    public long Ridx { get; set; }

    public long Accidx { get; set; }

    /// <summary>
    /// 제재타입(날짜표기, 미표기)
    /// </summary>
    public int ResticType { get; set; }

    /// <summary>
    /// 제재사유에대한 string key
    /// </summary>
    public int Stringkey { get; set; }

    /// <summary>
    /// 제재 종료일
    /// </summary>
    public DateTime Enddate { get; set; }

    /// <summary>
    /// 등록자
    /// </summary>
    public string Regster { get; set; } = null!;

    /// <summary>
    /// 사유에대한 간략설명
    /// </summary>
    public string Comment { get; set; } = null!;
}
