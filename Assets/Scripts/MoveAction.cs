﻿using UnityEngine;
using System.Collections;
using Prime31;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class MoveAction : MonoBehaviour {

    public CharacterController2D CC2D
    {
        get
        {
            if (null == mCC2D)
            {
                mCC2D = Util.TryAddComponent<CharacterController2D>(gameObject);
            }
            return mCC2D;
        }
    }
    private CharacterController2D mCC2D;
    // 存储运动
    private Vector2 movement = Vector2.zero;

    private MoveDir mFaceDir;
    [SerializeField]
    private MoveDir mMovingDir;
    private float mCurrentSpeed;
    private float mTargetSpeed;

    public float gravity = -25f;

    void Awake()
    {
        CC2D.onControllerCollidedEvent += OnCC2DCollider;
        CC2D.onTriggerEnterEvent += OnCC2DTriggerEnter;
        CC2D.onTriggerExitEvent += OnCC2DTriggerExit;
    }

    private void OnCC2DCollider(RaycastHit2D hit)
    {
  //      Debug.Log("OnCC2DCollider " + hit.transform.name);
    }

    private void OnCC2DTriggerEnter(Collider2D collider)
    {
  //      Debug.Log("OnCC2DTriggerEnter " + collider.name);
    }

    private void OnCC2DTriggerExit(Collider2D collider)
    {
 //       Debug.Log("OnCC2DTriggerExit " + collider.name);
    }
    float flag = 0;
    public void Move(MoveDir dir, float moveSpeed)
    {
        if (dir != mMovingDir)
        {
            mMovingDir = dir;
            mFaceDir = dir;
            flag = mMovingDir == MoveDir.Right ? 1 : -1;
            mCurrentSpeed = 2f * flag; // init speed
        }
        mTargetSpeed = flag * moveSpeed;
    }

    public void EndMove()
    {
        mMovingDir = MoveDir.None;
        movement.x = 0;  
    }

    void FixedUpdate()
    {
        if (mCC2D.isGrounded)
            movement.y = 0;

        // apply gravity before moving
        movement.y += gravity * Time.deltaTime;

        if (mMovingDir == MoveDir.None)
        {
            movement.x = 0;
        }
        else
        {
            movement.x = mTargetSpeed;
        }


        if (null != mCC2D)
            mCC2D.move(movement * Time.fixedDeltaTime);
    }

    public MoveDir GetCurrentFaceDir()
    {
        if (mFaceDir == MoveDir.None)
            mFaceDir = MoveDir.Right;   //right for default
        return mFaceDir;
    }

    public void SetFacingDir(MoveDir moveDir)
    {
        mFaceDir = moveDir;
    }

    public void MoveForward(float moveSpeed)
    {
        Move(GetCurrentFaceDir(), moveSpeed);
    }
}


