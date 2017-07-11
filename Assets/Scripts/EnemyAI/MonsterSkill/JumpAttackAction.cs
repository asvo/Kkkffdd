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

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _monster = GetComponent<Monster>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion

    public void Attack(BaseEntity Target,float JumpHeight, float JumpTime,System.Action CallBack = null)
    {
        this.CallBack = CallBack;
        m_JumpTimer = JumpTime;
        m_JumpHeight = JumpHeight;
        startPoint = transform.position;
        endPoint = Target.transform.position;

        m_bOnJumpPose = false;

        float preparetime = PrepareJumpTime(Target);
        PlayPreparePose();

        Invoke("Jump", preparetime);
    }


    private void PlayPreparePose()
    {
        _monster.ResetPoseAndPlayAnim("attack2_Continued", true);
    }

    private void Jump()
    {
        _monster.ResetPoseAndPlayAnim("attack2_End", false);
        jumpCurve = new Bezier(transform, m_JumpHeight, startPoint, endPoint, m_JumpTimer);
        m_bOnJumpPose = true;
        this.enabled = true;
        Util.LogHW(gameObject.name + " jump start time:" + Time.time);
    }


    private float PrepareJumpTime(BaseEntity to)
    {
        float prepareTime = Mathf.Abs(Vector2.Distance(_monster.transform.position, to.transform.position)) * 0.1f;
        return prepareTime;
    }

    // Update is called once per frame
    void Update () {

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
        if (CallBack != null)
        {
            CallBack();
            CallBack = null;
        }
    }
}

