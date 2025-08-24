using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject targetGameObject) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(targetGameObject);
    }

    public static void BindEvent(this GameObject gameObject, Action<PointerEventData> action, Define.UIEvent uiEventType = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(gameObject, action, uiEventType);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

}