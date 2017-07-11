using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class MonsterManager : Single<MonsterManager> {

    public List<Monster> ActiveMonsters = new List<Monster>();
    public Dictionary<int, Monster> dic_CacheMonsterList = new Dictionary<int, Monster>();

    /// <summary>
    /// 在场景中生成一只怪物
    /// </summary>
    /// <param name="MonsterIndex"></param>
    /// <param name="spawnPoint"></param>
    /// <param name="EnemyPrefab"></param>
    /// <returns></returns>
    public Monster SpawnOneMonster(int MonsterIndex, Transform spawnPoint,GameObject EnemyPrefab,Vector3 offset)
    {
        Monster monster = null;
        if (dic_CacheMonsterList.ContainsKey(MonsterIndex))
        {
            monster = dic_CacheMonsterList[MonsterIndex];            
            dic_CacheMonsterList.Remove(MonsterIndex);
        }
        else
        {
            GameObject goMonster = GameObject.Instantiate(EnemyPrefab);
            monster = Util.TryAddComponent<Monster>(goMonster);
        }

        monster.MoveCtrl.CC2D.platformMask = 1 << Util.GroundLayer | 1 << Util.WallLayer | 1 << Util.PlayerLayer | 1 << Util.MonsterLayer;
        monster.gameObject.name = MonsterIndex.ToString();
        monster.gameObject.layer = Util.MonsterLayer;
        monster.transform.position = spawnPoint.position - offset;
        monster.transform.rotation = Quaternion.identity;
        monster.gameObject.SetActive(true);
        BuildAiType(monster, EnemyType.Monster_Normal);

        ActiveMonsters.Add(monster);
        return monster;
    }

    private void BuildAiType(Monster monster, EnemyType AiType = EnemyType.Monster_Normal)
    {
        EnemyAI monsterAI = null;
        switch (AiType)
        {
            case EnemyType.Monster_Normal:
                monsterAI = new EnemyAI(monster, EnemyType.Monster_Normal);
                monsterAI.ChangeAIState(new ConfuseAIState(monster.MaxConfusedTime));
                break;
        }

        monster.Spawn(monsterAI);
    }

    /// <summary>
    /// 怪物加入缓存池
    /// </summary>
    public void MoveToCachePool()
    {
        if (ActiveMonsters == null)
            return;
        List<Monster> deleterMonster = new List<Monster>();
        foreach (Monster child in ActiveMonsters)
        {
            if (child != null)
            {
                child.gameObject.SetActive(false);
                int monsterIndex = 0;
                if (int.TryParse(child.name, out monsterIndex))
                {
                    if (!dic_CacheMonsterList.ContainsKey(monsterIndex))
                    {
                        dic_CacheMonsterList.Add(monsterIndex, child);
                    }
                    else
                    {
                        deleterMonster.Add(child);
                    }
                }
            }
        }

        for (int i = 0; i < deleterMonster.Count; i++)
        {
           GameObject.Destroy(deleterMonster[i].gameObject);
        }

        ActiveMonsters = new List<Monster>();
    }

    /// <summary>
    /// 怪物死亡处理
    /// </summary>
    /// <param name="monster"></param>
    public void MonsterDie(Monster monster)
    {
        monster.gameObject.SetActive(false);
        ActiveMonsters.Remove(monster);
        int monsterIndex = 0;
        if (int.TryParse(monster.name, out monsterIndex))
        {
            if (!dic_CacheMonsterList.ContainsKey(monsterIndex))
            {
                dic_CacheMonsterList.Add(monsterIndex, monster);
            }
            else
            {
                GameObject.Destroy(monster.gameObject);
            }
        }
    }

    /// <summary>
    /// 检查怪物移动方向上是否有怪物阻挡
    /// </summary>
    /// <param name="mMonster"></param>
    /// <returns></returns>
    public List<Monster> GetMonsterInfrontToTarget(Monster mMonster,BaseEntity Target)
    {
        float targetX = Target.transform.position.x;
        float distToTarget = targetX - mMonster.transform.position.x; 

        List<Monster> samedirectmonsters = new List<Monster>();
        foreach (Monster monster in ActiveMonsters)
        {
            if (monster == mMonster)
                continue;

            float thirdToTarget = targetX - monster.transform.position.x;

            if (thirdToTarget * distToTarget >= 0  && Mathf.Abs(distToTarget) > Mathf.Abs(thirdToTarget))
            {
                samedirectmonsters.Add(monster);
            }
        }

        return samedirectmonsters;
    }

    /// <summary>
    /// 怪物指向目标的方向
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    public MoveDir DirToTarget(Transform source,Transform target)
    {
        MoveDir moveDir = target.position.x - source.position.x > 0 ? MoveDir.Right : MoveDir.Left;
        return moveDir;
    }

    public MoveDir DirToPlayer(Monster monster)
    {
        MoveDir moveDir = MoveDir.Right;
        if (GameManager.Instance().player != null)
        {
            moveDir = DirToTarget(monster.transform, GameManager.Instance().player.transform);           
        }
        else
        {
            Debug.LogError("Player is not exist!");
        }

        return moveDir;
    }
}

