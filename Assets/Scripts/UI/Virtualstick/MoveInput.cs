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
