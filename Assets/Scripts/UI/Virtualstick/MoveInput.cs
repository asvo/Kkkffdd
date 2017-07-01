using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 角色移动按钮管理
/// @asvo
/// </summary>
public class MoveInput : MonoBehaviour
{
    public TwoBtnModeInput TwoBtnInput;
    private GameObject mVsJoyGobj;
    public GameObject VsJoyGobj
    {
        get
        {
            if (null == mVsJoyGobj)
            {
                mVsJoyGobj = transform.FindChild("JoyStick").gameObject;
            }
            return mVsJoyGobj;
        }
    }

    Player mPlayer;
    private Player GetPlayer
    {
        get
        {
            if (null == mPlayer)
            {
                mPlayer = GameManager.Instance().player.GetComponent<Player>();
            }
            return mPlayer;
        }
    }

    void Start()
    {
        SetInputMode(InputModeStyle.JoyStick);
    }

    public enum InputModeStyle
    {
        TwoBtn = 0,
        JoyStick
    }
    public void SetInputMode(InputModeStyle inputMode)
    {
        if (inputMode == InputModeStyle.TwoBtn)
        {
            VsJoyGobj.SetActive(false);
            TwoBtnInput.SetOn();
        }
        else
        {
            TwoBtnInput.SetOff();
            VirtualStick vstick = VsJoyGobj.GetComponent<VirtualStick>();
            if (null != vstick)
            {
                vstick.Reset();
            }
            VsJoyGobj.SetActive(true);
        }
    }

    public void OnClickMove(MoveDir dir)
    {
        if (GameManager.Instance().IsGameOver)
            return;
        if (GetPlayer != null)
            GetPlayer.Move(dir);
    }

    public void OnClickEndMove()
    {
        if (GameManager.Instance().IsGameOver)
            return;
        if (GetPlayer != null)
            GetPlayer.EndMove();
    }

    #region Keyboard-Move

    int mLastMoveVal = 0;
    int moveVal = 0;
    void Update()
    {
        if (GameManager.Instance().IsGameOver)
            return;

#region move-keyboard
        if (Input.GetKeyDown(KeyCode.A))
        {
            MinusMove();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            AddMove();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddMove();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            MinusMove();
        }
        if (mLastMoveVal != moveVal)
        {
            mLastMoveVal = moveVal;
            if (moveVal < 0)
            {
                OnClickMove(MoveDir.Left);
            }
            else if (moveVal > 0)
            {
                OnClickMove(MoveDir.Right);
            }
            else
            {
                OnClickEndMove();
            }
        }
#endregion move-keyboard

        if (Input.GetKey(KeyCode.J))
        {
            if (GetPlayer != null)
                GetPlayer.FireSkill(0);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            if (GetPlayer != null)
                GetPlayer.FireSkill(1);
        }
    }

    private void AddMove()
    {
        ++moveVal;
        if (moveVal > 1)
            moveVal = 1;
    }

    private void MinusMove()
    {
        --moveVal;
        if (moveVal < -1)
            moveVal = -1;
    }

    #endregion Keyboard-Move
}
