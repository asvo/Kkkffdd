using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 虚拟遥感
/// </summary>
public class VirtualStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public delegate void VsTouchBegin(Vector2 vec2);
    public delegate void VsTouchMove(Vector2 vec2);
    public delegate void VsTouchEnd();

    public event VsTouchBegin OnTouchBegin;
    public event VsTouchMove OnTouchMove;
    public event VsTouchEnd OnTouchEnd;

    private Transform mSlotTrans;
    private Transform Slot
    {
        get
        {
            if (null == mSlotTrans)
            {
                mSlotTrans = transform.FindChild("Inner");
            }
            return mSlotTrans;
        }
    }
    private MoveInput mMoveInput;
    public MoveInput MoveInputModule
    {
        get
        {
            if (null == mMoveInput)
            {
                mMoveInput = transform.parent.GetComponent<MoveInput>();
            }
            return mMoveInput;
        }
    }

    private bool mIsInTouch = false;
    private Vector3 mOriginPos = Vector3.zero;

    float mMaxXDelta = 100f;
    float mMaxYDelta = 100f;

    public void Reset()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        mMaxXDelta = rectTrans.rect.width;
        mMaxYDelta = rectTrans.rect.height;

        Slot.localPosition = Vector3.zero;
        mOriginPos = Slot.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mIsInTouch = true;
        SetSlotPosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mIsInTouch = false;
        Slot.localPosition = Vector3.zero;
        EndMove();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetSlotPosition(eventData.position);
        Move(eventData.position);
    }

    private void Move(Vector3 evtPos)
    {
        float deltaX = evtPos.x - mOriginPos.x;
        if (Mathf.Abs(deltaX) < 3f)
            return;
        MoveDir moveDior = deltaX > 0 ? MoveDir.Right : MoveDir.Left;
        MoveInputModule.OnClickMove(moveDior);
    }

    private void EndMove()
    {
        MoveInputModule.OnClickEndMove();
    }

    private void SetSlotPosition(Vector2 mousePos)
    {
        //遥感中心区域限制
        if (mousePos.x > mMaxXDelta)
            mousePos.x = mMaxXDelta;
        if (mousePos.x < -mMaxXDelta)
            mousePos.x = -mMaxXDelta;
        if (mousePos.y > mMaxYDelta)
            mousePos.y = mMaxYDelta;
        if (mousePos.y < -mMaxYDelta)
            mousePos.y = -mMaxYDelta;
        
        Slot.position = mousePos;
        //Vector3 posMouse = CameraManager.Instance().MainCamera.WorldToScreenPoint(mousePos);
        //Slot.position = posMouse;
    }
}
