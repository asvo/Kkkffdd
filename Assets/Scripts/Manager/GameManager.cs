﻿using UnityEngine;
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

    [HideInInspector]
    public GameObject player;
    SpawnManager spawnManager;
    public Player MainPlayer;
    void Start()
    {
        LoadPlayer();
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.Init();
    }

    private void LoadPlayer()
    {
        Object playerObj = Resources.Load("ModelPrefab/Player");
        player = GameObject.Instantiate(playerObj) as GameObject;
        MainPlayer = Util.TryAddComponent<Player>(player);
        MainPlayer.InitPlayer();

        SmoothFollow camc = CameraManager.Instance().MainCamera.GetComponent<SmoothFollow>();
        if (null != camc)
        {
            camc.Init(player.transform);
        }
    }
}

