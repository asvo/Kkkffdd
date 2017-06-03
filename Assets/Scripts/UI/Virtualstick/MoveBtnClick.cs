using UnityEngine;
using System.Collections;

/// <summary>
/// 移动按钮
/// @asvo
/// </summary>
public class MoveBtnClick : MonoBehaviour {

    public MoveDir MoveDir;

    private MoveInput mMoveCtrl;

	void Awake()
    {
        UIEventlistener.RegistPointerDownEvent(gameObject, OnClickDown);
        UIEventlistener.RegistPointerUpEvent(gameObject, OnClickUp);
    }

    public void Init(MoveInput moveCtrl, MoveDir moveDir)
    {
        mMoveCtrl = moveCtrl;
        MoveDir = moveDir;
    }

    private void OnClickDown()
    {
        if (null != mMoveCtrl)
        {
            mMoveCtrl.OnClickMove(MoveDir);
        }
    }

    private void OnClickUp()
    {
        if (null != mMoveCtrl)
        {
            mMoveCtrl.OnClickEndMove();
        }
    }
}

public enum MoveDir
{
    None = 0,
    Left,
    Right
}