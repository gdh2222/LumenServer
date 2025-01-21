using System;
using System.Collections.Generic;

namespace DBMediator.Models.Account;

/// <summary>
/// 유저의 닉네임정보 테이블
/// 
/// </summary>
public partial class Accountproperty
{
    public long Ridx { get; set; }

    public long Accidx { get; set; }

    public string Nickname { get; set; } = null!;

    public int ChangeableNick { get; set; }

    public DateTime RecentNickDt { get; set; }
}
