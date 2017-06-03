using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Single<T> where T: class,new(){

    private static T instance;
    private static readonly object syslock = new object();
    public static T Instance()
    {
        if (instance == null)
        {
            lock (syslock)
            {
                if (instance == null)
                {
                    instance = new T();
                    return instance;
                }
                else
                {
                    return instance;
                }
            }
        }
        else
        {
            return instance;
        }
    }
}

