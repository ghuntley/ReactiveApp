﻿using System;

namespace ReactiveApp.Interfaces
{
    public interface IJournalEntry
    {
        object Parameter { get; }
        Type SourcePageType { get; }
    }
}
