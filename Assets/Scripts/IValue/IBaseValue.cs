using UnityEngine;
using System.Collections;
using System;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/

namespace ValueModule
{
    public interface IBaseValue
    {
        IBaseValue SimpleClone();
        IBaseValue DeepClone();       
    }
}

