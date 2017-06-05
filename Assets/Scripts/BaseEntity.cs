using UnityEngine;
using System.Collections;

public class BaseEntity : MonoBehaviour {

    public float InitMoveSpeed = 2f;
    public float MaxMoveSpeed = 10f;
    public int Health = 3;
    public bool isDead = false;

    private MoveAction mMoveCtr;

    void Awake()
    {
        mMoveCtr = GetComponent<MoveAction>();
    }

    public virtual void Move(MoveDir moveDir)
    {
        if (mMoveCtr != null)
        {
            mMoveCtr.Move(moveDir, InitMoveSpeed);
        }
    }

    public virtual void EndMove()
    {
        if (mMoveCtr != null)
        {
            mMoveCtr.EndMove();
        }
    }
}
