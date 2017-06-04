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
        base.Move(moveDir);
    }

    public override void EndMove()
    {
        base.EndMove();
    }
}

