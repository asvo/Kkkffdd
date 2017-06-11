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

    [HideInInspector]
    public GameObject player;
    SpawnManager spawnManager;
    public Player MainPlayer;
    void Start()
    {
        GameOverUI.Instance.HideUi();
        LoadPlayer();
        spawnManager = FindObjectOfType<SpawnManager>();
        
        StartGame();
    }

    public Vector3 PlayerInitPos = Vector3.zero;
    private void LoadPlayer()
    {
        Object playerObj = Resources.Load("ModelPrefab/Player");
        player = GameObject.Instantiate(playerObj) as GameObject;
        MainPlayer = Util.TryAddComponent<Player>(player);

        SmoothFollow camc = CameraManager.Instance().MainCamera.GetComponent<SmoothFollow>();
        if (null != camc)
        {
            camc.Init(player.transform);
        }
    }

    public bool IsGameOver = false;
    public void StartGame()
    {
        GameOverUI.Instance.HideUi();
        player.transform.position = PlayerInitPos;
        MainPlayer.InitPlayer();
        spawnManager.Init();
        IsGameOver = false;
    }

    public void PauseGame()
    {
        if (null != spawnManager)
            spawnManager.Pause();
    }

    private Transform mUiRoot = null;
    public Transform UiRoot
    {
        get
        {
            if (null == mUiRoot)
            {
                GameObject canvasGobj = GameObject.Find("Canvas");
                mUiRoot = canvasGobj.transform.FindChild("UIRoot");
            }
            return mUiRoot;
        }
    }
}

