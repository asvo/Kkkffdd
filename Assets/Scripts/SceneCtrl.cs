using UnityEngine;
using System.Collections;

public class SceneCtrl : MonoBehaviour
{

    private Transform mBgFore;
    private Transform BgFore
    {
        get
        {
            if (null == mBgFore)
            {
                mBgFore = GameObject.Find("bg_fore").transform;
            }
            return mBgFore;
        }
    }
    private Transform mBgMid;
    private Transform BgMid
    {
        get
        {
            if (null == mBgMid)
            {
                mBgMid = GameObject.Find("scenes/bg_mid").transform;
            }
            return mBgMid;
        }
    }
    private Transform mBgBackend;
    private Transform BgBackend
    {
        get
        {
            if (null == mBgBackend)
            {
                mBgBackend = GameObject.Find("scenes/bg_backend").transform;
            }
            return mBgBackend;
        }
    }

    private bool mIsForeLoaded;
    private bool mIsMidLoaded;
    private bool mIsBackLoaded;
    private bool mIsLoadSceneBgOver;

    public bool PassLoadImg;

    private void CheckHasLoaded()
    {
        if (mIsLoadSceneBgOver)
        {
            return;
        }
        mIsLoadSceneBgOver = mIsForeLoaded && mIsMidLoaded && mIsBackLoaded;
        if (mIsLoadSceneBgOver)
        {
            LoadedSecne();
        }
    }

    private void LoadedSecne()
    {
        Debug.Log("scene loaded.");
        GameManager.Instance().OnSceneLoaded();
    }

    void Start()
    {
        mIsForeLoaded = false;
        mIsMidLoaded = false;
        mIsBackLoaded = false;
        mIsLoadSceneBgOver = false;


		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_IOS
		mIsLoadSceneBgOver = true;
		LoadedSecne();
		#else
        LoadScene();        
		#endif
		SetBgPosition ();
    }

    private void LoadScene()
    {        
		StartCoroutine(LoadImageA());
		StartCoroutine(LoadImageB());
		StartCoroutine(LoadImageC());
    }

	private void SetBgPosition()
	{
		mMidBgPos = BgMid.localPosition;
		mBackBgPos = BgBackend.localPosition;
	}

    void Update()
    {
        if (!mIsLoadSceneBgOver)
            return;
        if (null != GameManager.Instance().MainPlayer)
            LerpMoveBg();
    }

    private float PlayerMostLeft = -26.2f;
    private float PlayerMostRight = 0.4f;
    private float mDelta;

    private float MidPosLeft = -9f;
    private float MidPosRight = 15f;
    private Vector3 mMidBgPos;

    private float BackPosLeft = -9f;
    private float BackPosRight = 25f;
    private Vector3 mBackBgPos;

    private void LerpMoveBg()
    {
        float curPlayerX = GameManager.Instance().MainPlayer.transform.position.x;
        mDelta = PlayerMostRight - PlayerMostLeft;
        float f = Mathf.Clamp01((curPlayerX - PlayerMostLeft) / mDelta);
        mMidBgPos.x = Mathf.Lerp(MidPosLeft, MidPosRight, f);
        BgMid.localPosition = mMidBgPos;
        mBackBgPos.x = Mathf.Lerp(BackPosLeft, BackPosRight, f);
        BgBackend.localPosition = mBackBgPos;
    }

    IEnumerator LoadImageA()
    {
        string filepath = ResourceMgr.Instance().GetScenePngPath("new_bg_1.png");
        WWW ww = new WWW(filepath);
        yield return ww;
        if (ww != null && string.IsNullOrEmpty(ww.error))
        {
            Texture2D t2d = ww.texture;
            Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            BgFore.GetComponent<SpriteRenderer>().sprite = sp;
            mIsForeLoaded = true;
            CheckHasLoaded();
        }
    }

    IEnumerator LoadImageB()
    {
        string filepath = ResourceMgr.Instance().GetScenePngPath("new_bg_2.png");
        WWW ww = new WWW(filepath);
        yield return ww;
        if (ww != null && string.IsNullOrEmpty(ww.error))
        {
            Texture2D t2d = ww.texture;
            Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            BgMid.GetComponent<SpriteRenderer>().sprite = sp;
            mIsMidLoaded = true;
            CheckHasLoaded();
        }
    }

    IEnumerator LoadImageC()
    {
        string filepath = ResourceMgr.Instance().GetScenePngPath("new_bg_3.png");
        WWW ww = new WWW(filepath);
        yield return ww;
        if (ww != null && string.IsNullOrEmpty(ww.error))
        {
            Texture2D t2d = ww.texture;
            Sprite sp = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            BgBackend.GetComponent<SpriteRenderer>().sprite = sp;
            mIsBackLoaded = true;
            CheckHasLoaded();
        }
    }
}
