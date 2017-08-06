using UnityEngine;
using System.Collections;
using AIState;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

/// <summary>
/// 精英怪例3 同时具有技能1和2 但获得强化，效果为：技能2不再限定使用次数，但具有5scd。且释放技能2后会立即使用技能1，且不需要读条无视距离。
/// </summary>
public class Monster_SpecialThree : Monster {

    public float _dodgeCd = 5;
    private float _lastDodgeTime = 0;
    public override void OnDamaged(int damage)
    {
        if (m_AttrValue.Health - damage < 0)
        {
            if (Time.time - _lastDodgeTime > _dodgeCd)
            {
                _lastDodgeTime = Time.time;
                if (m_EnemyAI != null)
                {
                    m_EnemyAI.ChangeAIState(new BackRollAIState(GetMonsterValue().RollBackRange));
                    return;
                }
            }
        }
    }
}

