using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class RollBackAction : MonoBehaviour {

    private float range = 3;
    public float m_RollSpeed = 0.5f;
    private bool m_bOnRoll = false;

    private BaseEntity m_Attacker;
    private Vector2 m_RollDirection = Vector2.left;
    private float rolltime = 0;

    private System.Action CallBack = null;

    public void FireRollBack(BaseEntity Attacker,System.Action callback =null)
    {
        CallBack = callback;

        m_Attacker = Attacker;
        m_RollDirection = Direction();
        rolltime = range / m_RollSpeed;
        this.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (m_bOnRoll == false)
            return;

        rolltime -= Time.deltaTime;
        if (rolltime < 0)
        {
            m_bOnRoll = false;
            OnRollFinish();
        }
        else
        {
            transform.Translate(m_RollDirection * Time.deltaTime * m_RollSpeed);
        }
	}

    private void OnRollFinish()
    {
        if (CallBack != null)
        {
            CallBack();
            CallBack = null;
        }

        this.enabled = false;
    }

    private Vector2 Direction()
    {
        float dist = m_Attacker.transform.position.x - transform.position.x;
        return dist < 0 ? Vector2.left : Vector2.right;
    }
}

