﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.App
{
    public interface IStartup
    {
        IObservable<bool> Start(object hint = null);
    }
}
