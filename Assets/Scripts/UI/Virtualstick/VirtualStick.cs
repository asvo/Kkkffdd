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

    private bool mIsInTouch = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        mIsInTouch = true;
        if (OnTouchBegin != null)
        {
            //OnTouchBegin(eventData.)
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
