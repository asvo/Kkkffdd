using UnityEngine;
using System.Collections;

/// <summary>
/// 伤害处理
/// </summary>
public class DamagerHandler : Single<DamagerHandler>
{
    public void CalculateDamage(BaseEntity from, BaseEntity to, int damage)
    {
        if (null != to)
        {
            //Debug.LogFormat("{0} is OnDamge", to.transform.name);
            to.OnDamaged(damage);
        }
    }
}
