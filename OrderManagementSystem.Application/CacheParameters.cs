﻿namespace OrderManagementSystem.Application
{
    public class CacheParameters<TKey, TValue>
    {
        public TKey? Key { get; set; }
        public TValue? Value { get; set; }
    }
}