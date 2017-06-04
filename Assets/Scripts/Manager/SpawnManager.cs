using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public GameObject EnemyPrefab = null;
    //刷怪位置
    public Transform[] spawnPoints;
    private float leftTimeToNextSpawn = 0;
    [SerializeField]
    private float spawnTimer = 0;
    //每个刷怪点刷新怪物间隔
    public float SpawnIntervalTime = 3;
    //每个刷怪点每次刷新怪物个数
    public int SpawnCount = 5;
    //怪物刷新波数
    public int WaveCount = 0;
    public int MaxWave = 3;

    [SerializeField]
    private bool bLooping = false;

    public List<Monster> monsterList = new List<Monster>();
    public Dictionary<int, Monster> dic_CacheMonsterList = new Dictionary<int, Monster>();

    void Start()
    {
        if (EnemyPrefab == null)
            EnemyPrefab = Resources.Load("ModelPrefab/Enemy") as GameObject;
        Debug.LogError("I am monster Spawner");
        bLooping = false;
    }

    void Update()
    {
        if (bLooping)
        {
            if (Time.time - spawnTimer >= SpawnIntervalTime)
            {
                spawnTimer = Time.time;
                Spawn();
            }
        }
    }

    public void Init()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No Spwan Points");
            return;
        }

        WaveCount = 0;
        leftTimeToNextSpawn = 0;
        spawnTimer = Time.time;

        MoveToCachePool(monsterList);
        monsterList = new List<Monster>();
        bLooping = true;
    }

    public void Pause()
    {
        bLooping = false;
        leftTimeToNextSpawn = SpawnIntervalTime - (Time.time - spawnTimer);
    }

    public void Continue()
    {
        spawnTimer = Time.time - leftTimeToNextSpawn;
        bLooping = true;
    }


    private void Spawn()
    {
        if (WaveCount >= MaxWave)
            return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            for (int j = 0; j < SpawnCount; j++)
            {
                monsterList.Add(SpawnOneMonster(WaveCount * SpawnCount + j, spawnPoints[i]));
            }
        }

        WaveCount++;
    }

    private Monster SpawnOneMonster(int MonsterIndex,Transform spawnPoint)
    {
        Monster monster = null;
        if (dic_CacheMonsterList.ContainsKey(MonsterIndex))
        {
            monster = dic_CacheMonsterList[MonsterIndex];
            monster.Spawn();
            dic_CacheMonsterList.Remove(MonsterIndex);
            return monster;
        }

        GameObject goMonster = Instantiate(EnemyPrefab);
        monster = Util.TryAddComponent<Monster>(goMonster);
        monster.Spawn();
        goMonster.name = MonsterIndex.ToString();
        goMonster.transform.SetParent(spawnPoint);
        goMonster.transform.localPosition = Vector3.zero;
        goMonster.transform.localRotation = Quaternion.identity;
        return monster;
    }

    public void MonsterDie(Monster monster)
    {
        monsterList.Remove(monster);
        int monsterIndex = 0;
        if (int.TryParse(monster.name, out monsterIndex))
        {
            if (!dic_CacheMonsterList.ContainsKey(monsterIndex))
            {
                dic_CacheMonsterList.Add(monsterIndex, monster);
            }
            else
            {
                Destroy(monster.gameObject);
            }
        }
    }


    private void MoveToCachePool(List<Monster> Monsters)
    {
        if (Monsters == null)
            return;
        List<Monster> deleterMonster = new List<Monster>();
        foreach (Monster child in Monsters)
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
            Destroy(deleterMonster[i].gameObject);
        }
    }
}
