using UnityEngine;
using System.Collections;

public class TwoBtnModeInput : MonoBehaviour {

    public MoveBtnClick BtnLeft;
    public MoveBtnClick BtnRight;

    void Start()
    {
        MoveInput moveInput = transform.parent.GetComponent<MoveInput>();
        if (null != moveInput)
        {
            Init(moveInput);
        }
    }

    public void SetOn()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void SetOff()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void Init(MoveInput moveInput)
    {
        BtnLeft.Init(moveInput, MoveDir.Left);
        BtnRight.Init(moveInput, MoveDir.Right);
    }
}
