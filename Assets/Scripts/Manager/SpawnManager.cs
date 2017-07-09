using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    public GameObject EnemyPrefab = null;
    //刷怪位置
    public Transform[] spawnPoints;
    [SerializeField]
    private float spawnTimer = 0;
    //每个刷怪点刷新怪物间隔
    public float SpawnIntervalTime = 3;
    //每个刷怪点每次刷新怪物个数
    public int m_CurWaveSpawnCount = 5;
    private int m_HaveSpawnCount = 0;
    [SerializeField]
    private bool m_bFinishSpwanWave = false;

    //怪物刷新波数
    public int WaveCount = 0;
    public int MaxWave = 3;

    public float SpawnOffset = 2;

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
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnTimer = SpawnIntervalTime;
                SpawnOneWave();
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

        m_HaveSpawnCount = 0;
        WaveCount = 0;
        spawnTimer = 0;

        MonsterManager.Instance().MoveToCachePool();
        bLooping = true;
        m_bFinishSpwanWave = false;
    }

    public void Pause()
    {
        bLooping = false;
    }

    public void Continue()
    {
        bLooping = true;
    }

    public void SpawnNextWave()
    {
        if (WaveCount > MaxWave)
            return;
        m_bFinishSpwanWave = false;
    }

    private void SpawnOneWave()
    {
        if (!m_bFinishSpwanWave)
        {
            SpawnOneMonster();
        }
    }

    private void SpawnOneMonster()
    {
        if (m_HaveSpawnCount > m_CurWaveSpawnCount)
        {
            WaveCount++;
            m_bFinishSpwanWave = true;
            return;
        }

        if (EnemyPrefab == null)
        {
            EnemyPrefab = Resources.Load("ModelPrefab/Enemy") as GameObject;
        }

        int spawnSide = m_HaveSpawnCount % 2;
        //玩家太靠近出生点时，选择另一个出生点出生
        if (checkSpwanSideNearestToPlayer(spawnSide))
            spawnSide = spawnSide == 0 ? 1 : 0;
        MonsterManager.Instance().SpawnOneMonster(m_HaveSpawnCount, spawnPoints[spawnSide], EnemyPrefab, new Vector2(0, 0));
        m_HaveSpawnCount++;
    }

    //检查出生点是否离玩家太近
    private bool checkSpwanSideNearestToPlayer(int index)
    {
        float dist = Vector2.Distance(spawnPoints[index].position, GameManager.Instance().MainPlayer.transform.position);
        return dist < SpawnOffset;
    }
}
