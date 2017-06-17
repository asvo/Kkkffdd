using UnityEngine;
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

    private MoveDir mMovingDir;
    private float mCurrentSpeed;
    private float mTargetSpeed;

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
            //Debug.Log("set moving dir = " + dir);
            if (GetComponentInChildren<Animator>() != null)
            {
                GetComponentInChildren<Animator>().SetBool("Move", true);
            }

            mMovingDir = dir;
            flag = mMovingDir == MoveDir.Right ? 1 : -1;
            mCurrentSpeed = 2f * flag; // init speed
        }
        mTargetSpeed = flag * moveSpeed;
    }

    public void EndMove()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().SetBool("Move", false);
        }
        mMovingDir = MoveDir.None;
        movement.x = 0;  
    }

    void FixedUpdate()
    {
        if (mMovingDir == MoveDir.None)
            return;
        //float speed = Mathf.Lerp(2f, mTargetSpeed, 1.5f);
        //if (speed > mTargetSpeed)
        float speed = mTargetSpeed;
        movement.x = speed;
        // 4 - 让游戏物体移动
        if (null != mCC2D)
            mCC2D.move(movement * Time.fixedDeltaTime);
    }

    public MoveDir GetCurrentFaceDir()
    {
        MoveDir moveDir = MoveDir.Right;    //right for default
        if (null != mCC2D)
        {
            if (mCC2D.collisionState.right)
                moveDir = MoveDir.Right;
            else if (mCC2D.collisionState.left)
                moveDir = MoveDir.Left;
        }
        return moveDir;
    }
}


