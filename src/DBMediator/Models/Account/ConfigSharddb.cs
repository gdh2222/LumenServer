using System;
using System.Collections.Generic;

namespace DBMediator.Models.Account;

/// <summary>
/// 게임디비의 샤드정보
/// 
/// - 게임디비들의 샤드정보를 기록
/// - uid : AccountShardDBLink 의 shard_uid 와 매칭
/// </summary>
public partial class ConfigSharddb
{
    public long Uid { get; set; }

    public string Aliasname { get; set; } = null!;

    public string Dbcntstring { get; set; } = null!;

    public string? LogdbCntstring { get; set; }
}
