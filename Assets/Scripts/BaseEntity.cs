using UnityEngine;
using System.Collections;

public class BaseEntity : MonoBehaviour {

    public float InitMoveSpeed = 2f;
    public float MaxMoveSpeed = 10f;
    public int Health = 3;
    public bool isDead = false;

    public MoveAction MoveCtrl;

    void Awake()
    {
        MoveCtrl = Util.TryAddComponent<MoveAction>(gameObject);
    }

    public virtual void OnDamaged(int damage)
    {
        //暂时不考虑伤害为负的情况, asvo
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public virtual void Move(MoveDir moveDir)
    {
        if (MoveCtrl != null)
        {
            if (GetComponentInChildren<SpriteRenderer>() != null)
            {
                if (moveDir == MoveDir.Left)
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
                else if (moveDir == MoveDir.Right)
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
            }
            MoveCtrl.Move(moveDir, InitMoveSpeed);
        }
    }

    public virtual void EndMove()
    {
        if (MoveCtrl != null)
        {
            MoveCtrl.EndMove();
        }
    }

    public virtual void Die()
    {

    }
}
