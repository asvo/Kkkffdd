using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */

/// <summary>
/// 精英怪例2 只具有技能1，但获得强化，效果为：释放完一次技能1之后，无论是没有击中角色，还是被人物击飞，都会继续对人物使用技能1，且不需要读条无视距离（不管离人物多远）。
/// </summary>
public class Monster_SpecialTwo : Monster {

    public override float PrepareJumpTime(BaseEntity to)
    {
        return 0;
    }
}

