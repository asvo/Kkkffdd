using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 角色移动按钮管理
/// @asvo
/// </summary>
public class MoveInput : MonoBehaviour
{
    
    public MoveBtnClick BtnLeft;
    public MoveBtnClick BtnRight;

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

    void Awake()
    {
        BtnLeft.Init(this, MoveDir.Left);
        BtnRight.Init(this, MoveDir.Right);
    }

    public void OnClickMove(MoveDir dir)
    {
        if (GetPlayer != null)
            GetPlayer.Move(dir);
    }

    public void OnClickEndMove()
    {
        if (GetPlayer != null)
            GetPlayer.EndMove();
    }

    #region Keyboard-Move

    int mLastMoveVal = 0;
    int moveVal = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            --moveVal;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            ++moveVal;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ++moveVal;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            --moveVal;
        }
        if (mLastMoveVal == moveVal)
            return;
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

    #endregion Keyboard-Move
}
