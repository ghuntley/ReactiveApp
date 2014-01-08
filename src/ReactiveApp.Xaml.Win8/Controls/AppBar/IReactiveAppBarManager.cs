﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Xaml.Controls
{
    public interface IReactiveAppBarManager
    {
        IDisposable RegisterAppBar(ReactiveAppBar appbar);
    }
}
