﻿namespace FlashCards.Api.Infrastructure;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public int ExpireHours { get; set; }
}