using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIEventlistener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    public delegate void OnClickPointerDown();
    public delegate void OnClickPointerUp();

    public event OnClickPointerDown OnClickPointerDownEvt;
    public event OnClickPointerUp OnClickPointerUpEvt;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (null != OnClickPointerDownEvt)
            OnClickPointerDownEvt();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (null != OnClickPointerUpEvt)
            OnClickPointerUpEvt();
    }

    public static void RegistPointerDownEvent(GameObject gobj, OnClickPointerDown onPointerDown)
    {
        UIEventlistener uiListener = gobj.GetComponent<UIEventlistener>();
        if (null == uiListener)
        {
            uiListener = gobj.AddComponent<UIEventlistener>();            
        }
        if (null != uiListener)
            uiListener.OnClickPointerDownEvt = onPointerDown;
    }

    public static void RegistPointerUpEvent(GameObject gobj, OnClickPointerUp onPointerUp)
    {
        UIEventlistener uiListener = gobj.GetComponent<UIEventlistener>();
        if (null == uiListener)
        {
            uiListener = gobj.AddComponent<UIEventlistener>();
        }
        if (null != uiListener)
            uiListener.OnClickPointerUpEvt = onPointerUp;
    }
}
