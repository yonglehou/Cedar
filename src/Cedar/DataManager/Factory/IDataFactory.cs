﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cedar.DataManager
{
    public interface IDataFactory
    {
        IDataReader GetdataReader();

      
    }
}