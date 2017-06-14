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

    void Start()
    {       
        //Debug.LogError("I am monster Spawner");
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

        MonsterManager.Instance().MoveToCachePool();
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

        if (EnemyPrefab == null)
        {
            EnemyPrefab = Resources.Load("ModelPrefab/Enemy") as GameObject;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            for (int j = 0; j < SpawnCount; j++)
            {
                MonsterManager.Instance().SpawnOneMonster(WaveCount * SpawnCount + j, spawnPoints[i],EnemyPrefab);
            }
        }

        WaveCount++;
    }




   


    
}
