﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DBMediator.Models.AccountDB;

/// <summary>
/// 유저의 기본적인 정보
/// 
/// - 고유번호
/// - 생성일자
/// - 마지막로그인
/// - 마켓타입
/// </summary>
public partial class Accountmember
{
    public long Idx { get; set; }

    public int Stateflag { get; set; }

    public DateTime CreateDt { get; set; }

    public DateTime LastLoginDt { get; set; }

    public int Mkt { get; set; }
}