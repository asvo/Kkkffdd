using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class MoveAction : MonoBehaviour {

    #region 1 - 变量
    public new Rigidbody2D rigidbody2D;
    
    // 存储运动
    private Vector2 movement = Vector2.zero;

    #endregion

    private MoveDir mMovingDir;
    private float mCurrentSpeed;
    private float mTargetSpeed;

    public void Move(MoveDir dir, float moveSpeed)
    {
        if (dir != mMovingDir)
        {
            mMovingDir = dir;
            float flag = mMovingDir == MoveDir.Left ? 1 : -1;
            mCurrentSpeed = 2f * flag; // init speed
            mTargetSpeed = moveSpeed * flag;
        }
    }

    public void EndMove()
    {
        mMovingDir = MoveDir.None;
        movement.x = 0;
        rigidbody2D.velocity = movement;
    }

    void FixedUpdate()
    {
        if (mMovingDir == MoveDir.None)
            return;
        float speed = Mathf.Lerp(2f, mTargetSpeed, 1.5f);
        if (speed > mTargetSpeed)
            speed = mTargetSpeed;
        movement.x = speed;
        // 4 - 让游戏物体移动
        rigidbody2D.velocity = movement;
    }
}


