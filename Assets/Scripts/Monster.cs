using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class Monster : BaseEntity {

    public EnemyAI enemyAi = null;
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
            MoveDir moveDir = GameManager.Instance().player.transform.position.x - transform.position.x > 0 ? MoveDir.Right : MoveDir.Left;
            if (moveDir == MoveDir.Left)
            {
                GetComponent<SpriteRenderer>().flipX =true;
            }
            else if (moveDir == MoveDir.Right)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            base.Move(moveDir);
        }
        else
        {
            Debug.LogError("Player is not exist!");
        }
    }

    public void Restats()
    {

    }
}

