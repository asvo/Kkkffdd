using UnityEngine;
using System.Collections;
using Prime31;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class JumpAttackAction : MonoBehaviour {
    
    private float m_JumpTimer = 0;
    private float m_JumpHeight = 0;
    Vector3 startPoint;
    Vector3 endPoint;

    private CharacterController2D _controller;
    private Monster _monster;

    private bool m_bOnJumpPose = false;

    Bezier jumpCurve = null;
    System.Action CallBack = null;

    public float HitBackSpeed = 3;

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _monster = GetComponent<Monster>();        

        // listen to some events for illustration purposes
        //_controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
       // _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>() != null)
        {
            //LogTrigger("onTriggerEnterEvent: " + col.gameObject.name);
            if (_monster != null)
            {
                _monster.ForceDamege();
            } 
        }
    }


    void onTriggerExitEvent(Collider2D col)
    {
        //if (col.gameObject.GetComponent<Player>() != null)
        //{
        //    LogTrigger("onTriggerExitEvent: " + col.gameObject.name);
        //}
    }

    private void LogTrigger(string log)
    {
        Debug.Log(log);
    }

    #endregion

    public void Attack(BaseEntity Target,float JumpHeight, float JumpTime,float PrepareTime, System.Action CallBack = null)
    {
        HitBackSpeed = _monster.GetMonsterValue().JumpHitBackSpeed;

        this.CallBack = CallBack;
        m_JumpTimer = JumpTime;
        m_JumpHeight = JumpHeight;
        startPoint = transform.position;
        endPoint = Target.transform.position;

        m_bOnJumpPose = false;
        PlayPreparePose();

        Invoke("Jump", PrepareTime);
    }


    private void PlayPreparePose()
    {
        _monster.ResetPoseAndPlayAnim("attack2_Continued", true);
    }

    private void Jump()
    {
        AnimationJumpStart();

        jumpCurve = new Bezier(transform, m_JumpHeight, startPoint, endPoint, m_JumpTimer);
        m_bOnJumpPose = true;

        _monster.ChangeCollider(false);
        this.enabled = true;

        Util.LogHW(gameObject.name + " jump start time:" + Time.time);

        Invoke("AnimationOnSpace", 0.1f);
        Invoke("AnimationLand", m_JumpTimer - 0.1f);
    }

    #region 空中动作
    void AnimationJumpStart()
    {
        _monster.ResetPoseAndPlayAnim("attack2_End1", false);
    }

    void AnimationOnSpace()
    {
        _monster.ResetPoseAndPlayAnim("attack2_End2", false);
    }

    void AnimationLand()
    {
        _monster.ResetPoseAndPlayAnim("attack2_End3", false);
    }
    #endregion

    private float PrepareJumpTime(BaseEntity to)
    {
        float prepareTime = Mathf.Abs(Vector2.Distance(_monster.transform.position, to.transform.position)) * 0.2f;
        return prepareTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_bOnJumpPose)
            return;

        m_JumpTimer -= Time.deltaTime;
        if (m_JumpTimer <= 0)
        {
            m_bOnJumpPose = false;
            OnFinish();
        }
        else
        {
            jumpCurve.Update();
        }
    }

    private void OnFinish()
    {
        Util.LogHW(gameObject.name +  " jump over time:" + Time.time);
        this.enabled = false;

        //OpenCollider(true);
        if (CallBack != null)
        {
            CallBack();
            CallBack = null;
        }
    }

    public void ForceStop()
    {
        CallBack = LandGround;
        m_bOnJumpPose = false;
        m_JumpTimer = 1 / HitBackSpeed;
        jumpCurve = new Bezier(transform, m_JumpHeight, transform.position, startPoint, m_JumpTimer);
        _monster.ChangeCollider(false);

        m_bOnJumpPose = true;
    }

    private void LandGround()
    {
        
    }
}

