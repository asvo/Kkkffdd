using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */
public enum e_EnemyState
{
    /// <summary>
    /// 迷茫
    /// </summary>
    confused,
    /// <summary>
    /// 进攻
    /// </summary>
    offensive,
    /// <summary>
    /// 硬直
    /// </summary>
    restats
}

public class EnemyAI : MonoBehaviour {

    public Monster monster = null;
    /// <summary>
    /// 怪物状态
    /// </summary>
    public e_EnemyState EnemyCurState = e_EnemyState.confused;

    private e_EnemyState enemyCurState
    {
        get
        {
            return EnemyCurState;
        }
        set
        {
            EnemyCurState = value;
            //Debug.LogError(gameObject.name + " change state to " + EnemyCurState);
        }
    }
    private bool canChangeState = false;
    /// <summary>
    /// 进攻距离，进攻依据1，距离玩家小于该值。
    /// 优先级低于迷茫时间
    /// </summary>
    public float OffensiveRange = 5;
    public float MaxConfusedTime = 20;
    /// <summary>
    /// 迷茫时间,进攻依据2，迷茫时间等于0
    /// 优先级高于进攻距离
    /// </summary>
    public float confusedTime = 20;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange = 3;
    public float AttckRate = 1;
    private float attackSecond = 1;
    private bool canAttack = false;

    /// <summary>
    /// 硬直时间
    /// </summary>
    private float restatsTime = 0.3f;

    // Use this for initialization
    public void InitEnemyAI () {

        monster = GetComponent<Monster>();
        confusedTime = MaxConfusedTime;
        enemyCurState = e_EnemyState.confused;
        canChangeState = true;

    }
	
	// Update is called once per frame
	void Update () {

        if (monster.isDead)
        {
            this.enabled = false;
            return;
        }

        MakeDecisions();
        switch (enemyCurState)
        {
            case e_EnemyState.confused:
                break;
            case e_EnemyState.offensive:
                Offensive();
                break;
            case e_EnemyState.restats:
                Restats();
                break;
        }
	}

    /// <summary>
    /// 进攻决策
    /// </summary>
    private void MakeDecisions()
    {
        if (!canChangeState)
            return;
        if (enemyCurState == e_EnemyState.confused)
        {
            if (distanceToPlayer() < OffensiveRange)
            {
                enemyCurState = e_EnemyState.offensive;
                return;
            }
            if (confusedTime > 0)
            {
                confusedTime -= Time.deltaTime;
            }
            else
            {
                confusedTime = 0;
                enemyCurState = e_EnemyState.offensive;
            }
        }
        else if (enemyCurState == e_EnemyState.restats)
        {
            if (confusedTime > 0)
            {
                confusedTime -= Time.deltaTime;
                enemyCurState = e_EnemyState.confused;
            }
            else
            {
                enemyCurState = e_EnemyState.offensive;
            }
        }
    }

    /// <summary>
    /// 开始进攻
    /// </summary>
    private void Offensive()
    {
        if (distanceToPlayer() > AttackRange)
        {
            canAttack = false;
        }
        else
        {
            if (monster != null)
                monster.EndMove();
            canAttack = true;
        }
        if (canAttack)
        {
            if (attackSecond > 0)
            {
                attackSecond -= Time.deltaTime;
            }
            else
            {
                attackSecond = AttckRate;
                monster.Attack();
            }
        }
        else
        {
            monster.MoveToPlayer();
        }
    }

    /// <summary>
    /// 进入硬直状态
    /// </summary>
    private void Restats()
    {
        if (restatsTime > 0)
        {
            restatsTime -= Time.deltaTime;
        }
        else
        {
            restatsTime = 0;
            canChangeState = true;
        }
    }

    private float distanceToPlayer()
    {
        float distance = 100000;

        if (GameManager.Instance().player != null)
        {
            distance = Vector2.Distance(GameManager.Instance().player.transform.position, transform.position);
        }
        return distance;
    }

    /// <summary>
    /// 其他怪物死亡，减少迷茫时间
    /// </summary>
    /// <param name="scecond"></param>
    public void ReduceConfusedTime(float scecond = 1)
    {
        if (confusedTime > 0)
            confusedTime -= 1; 
    }

    /// <summary>
    /// 受击
    /// </summary>
    public void Damage()
    {
        Debug.LogError("I am restats");
        canChangeState = false;
        canAttack = false;
        attackSecond = 0;
        enemyCurState = e_EnemyState.restats;
    }
}

