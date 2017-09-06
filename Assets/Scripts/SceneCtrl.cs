using UnityEngine;
using System.Collections;

public class SceneCtrl : MonoBehaviour {

    private Transform mBgFore;
    private Transform BgFore
    {
        get
        {
            if (null == mBgFore)
            {
                mBgFore = GameObject.Find("scenes/bg_fore").transform;
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

    void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        BgFore.GetComponent<SpriteRenderer>().sprite = ResourceMgr.Instance().LoadSpriteByFile("new_bg_1");
        BgMid.GetComponent<SpriteRenderer>().sprite = ResourceMgr.Instance().LoadSpriteByFile("new_bg_2");
        BgBackend.GetComponent<SpriteRenderer>().sprite = ResourceMgr.Instance().LoadSpriteByFile("new_bg_3");
        mMidBgPos = BgMid.localPosition;
        mBackBgPos = BgBackend.localPosition;
    }

    void Update()
    {
        LerpMoveBg();
    }

    private float PlayerMostLeft = -33f;
    private float PlayerMostRight = 14f;
    private float mDelta;

    private float MidPosLeft = -1.5f;
    private float MidPosRight = 10f;
    private Vector3 mMidBgPos;

    private float BackPosLeft = 0f;
    private float BackPosRight = 15f;
    private Vector3 mBackBgPos;

    private void LerpMoveBg()
    {
        float curPlayerX = GameManager.Instance().MainPlayer.transform.position.x;
        mDelta = PlayerMostRight - PlayerMostLeft;
        float f = Mathf.Clamp01((curPlayerX - PlayerMostLeft)/mDelta);
        mMidBgPos.x = Mathf.Lerp(MidPosLeft, MidPosRight, f);
        BgMid.localPosition = mMidBgPos;
        mBackBgPos.x = Mathf.Lerp(BackPosLeft, BackPosRight, f);
        BgBackend.localPosition = mBackBgPos;
    }
}
