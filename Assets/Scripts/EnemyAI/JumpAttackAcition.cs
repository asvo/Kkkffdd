using UnityEngine;
using System.Collections;
using Prime31;
/*
*  功能需求 ： 
*  编写者     ： 林鸿伟
*  version  ：1.0
*/


public class JumpAttackAcition : MonoBehaviour {
    
    private float m_JumpTimer = 0;
    private CharacterController2D _controller;

    Bezier jumpCurve = null;
    System.Action CallBack = null;

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
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

    public void PrepareForJump(float time)
    {
      
    }

    public void Attack(BaseEntity Target,float JumpHeight, float JumpTime,System.Action CallBack = null)
    {
        this.CallBack = CallBack;
        m_JumpTimer = JumpTime;
        Vector3 startPoint = transform.position;
        Vector3 endPoint = Target.transform.position;
        jumpCurve = new Bezier(transform, JumpHeight, startPoint, endPoint, JumpTime);
        this.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {

        m_JumpTimer -= Time.deltaTime;
        if (m_JumpTimer <= 0)
        {
            OnFinish();
        }
        else
        {
            jumpCurve.Update();
        }
	}

    private void OnFinish()
    { 
        this.enabled = false;
        if (CallBack != null)
        {
            CallBack();
            CallBack = null;
        }
    }
}

