using UnityEngine;
using System.Collections;
/*
 *  功能需求 ： 
 *  编写者     ： 林鸿伟
 *  version  ：1.0
 */


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            instance = new GameObject(typeof(GameManager).ToString()).AddComponent<GameManager>();
        }

        return instance;
    }

    public bool bPause = false;

    public const float NearestDistance = 1.2f;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject player;
    SpawnManager spawnManager;
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.Init();
    }
}

