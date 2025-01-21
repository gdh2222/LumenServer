using System;
using System.Collections.Generic;

namespace DBMediator.Models.Account;

/// <summary>
/// 유저가 할당된 DB(GameDB) 의 샤드 정보를 기록한 테이블
/// </summary>
public partial class Accountsharddblink
{
    public long Ridx { get; set; }

    public long Accidx { get; set; }

    public long ShardUid { get; set; }
}
