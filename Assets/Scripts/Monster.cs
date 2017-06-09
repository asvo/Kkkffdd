using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI enemyAi = null;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange = 0.1f;
    /// <summary>
    /// 攻击频率
    /// </summary>
    public float AttckRate = 1;
    public void Spawn()
    {
        Health = 3;
        isDead = false;
        enemyAi = Util.TryAddComponent<EnemyAI>(gameObject);
        enemyAi.InitEnemyAI();
    }

    public void Attack()
    {
        Debug.LogError(gameObject.name + " is Attack!");
    }

    public void Damage()
    {               
        if (enemyAi != null)
            enemyAi.Damage();

        Restats();
    }

    public void MoveToPlayer()
    {
        if (GameManager.Instance().player != null)
        {
            float distanceToPlayer = Vector2.Distance(GameManager.Instance().player.transform.position, transform.position);
            if (distanceToPlayer < GameManager.NearestDistance)
            {
                //Debug.Log(gameObject.name + " distance to player " + distanceToPlayer);
                base.EndMove();
                return;
            }

            MoveDir moveDir = MonsterManager.Instance().DirToPlayer(this);
            Debug.LogError(moveDir);

            base.Move(moveDir);
        }
    }

    public void Restats()
    {

    }
}

