using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

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

    [SerializeField]
    private bool bLooping = false;

    public List<Transform> monsterList = new List<Transform>();
    public Dictionary<int, Transform> dic_CacheMonsterList = new Dictionary<int, Transform>();

    void Start()
    {
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
        monsterList = new List<Transform>();
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
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            for (int j = 0; j < SpawnCount; j++)
            {
                monsterList.Add(SpawnOneMonster(WaveCount * SpawnCount + j, spawnPoints[i]));
            }
        }

        WaveCount++;
    }

    private Transform SpawnOneMonster(int MonsterIndex,Transform spawnPoint)
    {
        if (dic_CacheMonsterList.ContainsKey(MonsterIndex))
        {
            Transform monster = dic_CacheMonsterList[MonsterIndex];
            dic_CacheMonsterList.Remove(MonsterIndex);
            return monster;
        }

        GameObject goMonster = new GameObject(MonsterIndex.ToString());
        goMonster.transform.SetParent(spawnPoint);
        goMonster.transform.localPosition = Vector3.zero;
        goMonster.transform.localRotation = Quaternion.identity;
        return goMonster.transform;
    }

    public void MonsterDie(Transform monster)
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
                Destroy(monster);
            }
        }
    }


    private void MoveToCachePool(List<Transform> Monsters)
    {
        if (Monsters == null)
            return;
        List<Transform> deleterMonster = new List<Transform>();
        foreach (Transform child in Monsters)
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
