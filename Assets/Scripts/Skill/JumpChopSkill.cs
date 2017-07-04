using UnityEngine;
using System.Collections;

public class JumpChopSkill : SkillBase
{

    #region cfg

    public float JumpMoveX = 0.5f; //有正负，负表示向后移动
    public float JumpHeight = 0.8f; //跳起高度
    public float JumpTime = 0.5f;
    public float PlayActionChopTimeline = 0.5f; //跳起动作完成后多久开始播放攻击动作
    public float PlayActionFallTimeline = 0.1f; //攻击动作完成后多久开始播放下落动作
    
    #endregion

    public string Action1Name = "skill2_1";
    public string Action2Name = "skill2_2";
    public string Action3Name = "skill2_3";

    private BaseEntity mBaseEnity;
    private float mSpeedX;
    private float mSpeedY;
    private float mCurJumpTime;

    protected override void OnCast()
    {
        mBaseEnity = GetComponent<BaseEntity>();
        //float speedY = 0.5f * Mathf.Sqrt(JumpHeight) * 8f;
        mCurJumpTime = 0f;
        mSpeedY = JumpHeight / JumpTime;     //先用匀速
        mSpeedX = JumpMoveX / JumpTime;
        mBaseEnity.MoveCtrl.MoveXY(mSpeedX, mSpeedY);
        mBaseEnity.SkeletonAnim.state.SetAnimation(0, Action1Name, false);
        mBaseEnity.SkeletonAnim.state.AddAnimation(0, Action2Name, false, PlayActionChopTimeline);
        mBaseEnity.SkeletonAnim.state.AddAnimation(0, Action3Name, false, PlayActionFallTimeline);        
        mLerpUp = true;
    }

    private bool mLerpUp;

    void FixedUpdate()
    {
        if (!mLerpUp)
            return;
        mCurJumpTime += Time.fixedDeltaTime;
        float freq = Mathf.Clamp01(mCurJumpTime/2 * JumpTime);
        float curSpeedY = Mathf.Lerp(mSpeedY, -mSpeedY, freq);
        mBaseEnity.MoveCtrl.MoveXY(mSpeedX, curSpeedY);
        if (curSpeedY - 0.001f < -mSpeedY)
        {
            mLerpUp = false;
            SkillMoveEnd();
        }
    }

    private void SkillMoveEnd()
    {
        mBaseEnity.MoveCtrl.EndMove();
    }

    protected override void OnCalculateDamage()
    {
        BaseEntity target = Util.FindNereastTargetMonsterByDist(mBaseEnity, DamageRange);
        Util.LogAsvo("Attack JumpChopSkill !");
        if (null != target)
        {
            Util.LogAsvo("Attack JumpChopSkill : " + target.name);
            DamagerHandler.Instance().CalculateDamage(mBaseEnity, target, Damage);
        }
    }
}
