﻿using UnityEngine;
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

    public virtual void Move(MoveDir moveDir)
    {
        if (MoveCtrl != null)
        {
            if (moveDir == MoveDir.Left)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (moveDir == MoveDir.Right)
            {
                GetComponent<SpriteRenderer>().flipX = false;
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
}
