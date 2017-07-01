using UnityEngine;
using System.Collections.Generic;
/*
 *  功能需求 ： AI 状态接口
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public abstract class IAIState
{
    protected ICharacterAI m_CharacterAI = null;//怪物AI（状态拥有者）

    public IAIState()
    { }

    //设置CharacterAI的对象
    public void SetCharacterAI(ICharacterAI CharacterAI)
    {
        m_CharacterAI = CharacterAI;
    }

    //设置要攻击的目标
    public virtual void SetAttckPosition(Vector3 AttackPosition)
    {}

    //更新
    public abstract void Update(List<BaseEntity> Targets);

    //目标被删除
    public virtual void RemoveTarget(BaseEntity Target)
    { }
}

