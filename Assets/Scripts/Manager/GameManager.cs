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

    #region log

    public bool LogAsvo = false;
    public bool LogHW = false;

    #endregion

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SkillCfgMgr.Instance().Load();
        SkillDataMgr.Instance().InitSkillCdData();
    }

    [HideInInspector]
    public GameObject player;
    SpawnManager spawnManager;
    public Player MainPlayer;
    void Start()
    {
        GameOverUI.Instance.HideUi();
        LoadPlayer();
        LoadSettingData();
        spawnManager = FindObjectOfType<SpawnManager>();
        
        StartGame();
    }

    private void LoadSettingData()
    {
        ValueManager.Instance().Load(true);
        ValueManager.Instance().Load(false);        
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
        SkillDataMgr.Instance().ClearSkillCds();
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

    public static MoveInput.InputModeStyle CurrentInputMode = MoveInput.InputModeStyle.TwoBtn;
    public void ChangeInputMode()
    {
        if (null == mMoveInputTrans)
        {
            mMoveInputTrans = UiRoot.FindChild("HUD/MoveInput");
        }
        if (CurrentInputMode == MoveInput.InputModeStyle.JoyStick)
        {
            CurrentInputMode = MoveInput.InputModeStyle.TwoBtn;
        }
        else
        {
            CurrentInputMode = MoveInput.InputModeStyle.JoyStick;            
        }
        MoveInput moveinputSc = mMoveInputTrans.GetComponent<MoveInput>();
        moveinputSc.SetInputMode(CurrentInputMode);
    }

    private Transform mMoveInputTrans;
}

