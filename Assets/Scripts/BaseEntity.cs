using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BaseEntity : MonoBehaviour
{

    #region cfg-data
    public float NormalAttackCd = 1.5f;
    public float NormalAttackRange = 2.5f;
    public float NormalAttackDamgePoint = 0.3f;
    public int NormalAttackDamge = 1;

    #endregion cfg-data

    public float InitMoveSpeed = 2f;
    public float MaxMoveSpeed = 10f;
    public int Health = 3;
    public bool isDead = false;

    public MoveAction MoveCtrl;

    private SkeletonAnimation mSkeletonAnim;
    public SkeletonAnimation SkeletonAnim
    {
        get
        {
            if (null == mSkeletonAnim)
            {
                Transform skeletonTrans = transform.FindChild("entitySkeleton");
                mSkeletonAnim = skeletonTrans.GetComponent<SkeletonAnimation>();
            }
            return mSkeletonAnim;
        }
    }

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
        //     Debug.LogError("moveDir"+ moveDir);
        if (MoveCtrl != null)
        {
            if (moveDir == MoveDir.Left)
            {
                SkeletonAnim.Skeleton.FlipX = true;
            }
            else if (moveDir == MoveDir.Right)
            {
                SkeletonAnim.Skeleton.FlipX = false;
            }
            PlayAnim("run", true);
            MoveCtrl.Move(moveDir, InitMoveSpeed);
        }
    }

    public virtual void EndMove()
    {
        //      Debug.LogError("EndMove");
        if (MoveCtrl != null)
        {            
            MoveCtrl.EndMove();
        }
    }

    public virtual void Die()
    {
        Debug.Log(this.transform.name + " died");
        isDead = true;
    }

    public void PlayAnim(string animName, bool isloop = false)
    {
        if (SkeletonAnim == null)
        {
            return;
        }
        Spine.AnimationState animState = SkeletonAnim.state;
        animState.SetAnimation(0, animName, isloop);
    }

    /// <summary>
    /// todo, asvo. 暂时没想好咋个停止动画。这里直接重置动作，清除tracks.
    /// </summary>
    /// <param name="animName"></param>
    public void StopAnim(string animName)
    {
        if (SkeletonAnim == null)
        {
            return;
        }
        SkeletonAnim.timeScale = 1;
        SkeletonAnim.skeleton.SetToSetupPose();
        SkeletonAnim.state.ClearTracks();
    }
}
