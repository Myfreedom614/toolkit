﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IStep
    {
        string Description { get; }

        Task RunAsync();
    }
}
