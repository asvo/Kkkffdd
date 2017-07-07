using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 简单的事件管理
/// </summary>
public class EventMgr : Single<EventMgr>
{
    public delegate void CommonEvent(object data);

    private Dictionary<EventsType, CommonEvent> mEventsData = new Dictionary<EventsType, CommonEvent>();

    public void AttachEvent(EventsType eventType, CommonEvent commonEvent)
    {
        if (mEventsData.ContainsKey(eventType))
        {
            mEventsData[eventType] += commonEvent;
        }
        else
        {
            mEventsData.Add(eventType, commonEvent);
        }
    }

    public void DettachEvent(EventsType eventType, CommonEvent dettachEvent)
    {
        if (mEventsData.ContainsKey(eventType))
        {
            mEventsData[eventType] -= dettachEvent;
        }
        else
        {
            Debug.Log("No such EventType: " + eventType);
        }
    }

    public void TriggerEvent(EventsType triggerEvent, object param)
    {
        if (mEventsData.ContainsKey(triggerEvent))
        {
            mEventsData[triggerEvent](param);
        }
        else
        {
            Debug.Log("No such EventType: " + triggerEvent);
        }
    }
}

