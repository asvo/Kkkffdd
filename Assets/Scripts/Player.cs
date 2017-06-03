using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Player : BaseEntity {

    private MoveAction mMoveCtr;

    void Awake()
    {
        mMoveCtr = GetComponent<MoveAction>();
    }

    public override void Move(MoveDir moveDir)
    {
        base.Move(moveDir);
        if (mMoveCtr != null)
        {
            mMoveCtr.Move(moveDir, MaxMoveSpeed);
        }
    }

    public override void EndMove()
    {
        base.EndMove();
        if (mMoveCtr != null)
        {
            mMoveCtr.EndMove();
        }
    }
}

