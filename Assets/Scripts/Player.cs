using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Player : BaseEntity {
    

    public override void Move(MoveDir moveDir)
    {
        if (TooNearToMonster(moveDir))
            EndMove();
        else
            base.Move(moveDir);
    }

    public override void EndMove()
    {
        base.EndMove();
    }

    public bool TooNearToMonster(MoveDir moveDir)
    {
        foreach (Monster monster in MonsterManager.Instance().ActiveMonsters)
        {
            if (MonsterManager.Instance().DirToTarget(transform, monster.transform) == moveDir)
            {
                if (Vector2.Distance(monster.transform.position, transform.position) <= GameManager.NearestDistance + monster.AttackRange)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

