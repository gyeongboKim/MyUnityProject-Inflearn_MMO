using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI_EventHandler�� UnityEngine.EventSystems�� �������̽��� �����Ͽ� UI ��ҿ� ���� �̺�Ʈ ó���� ����մϴ�.
//SRP : ���� �̺�Ʈ ó������ ����ϴ� ������Ʈ��, UI ����� Ŭ��, �巡��, ���콺 ���� �� ��Ż �̺�Ʈ�� �����մϴ�.
public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnterHandler != null)
            OnEnterHandler.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitHandler != null)
            OnExitHandler.Invoke(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownHandler != null)
            OnPointerDownHandler.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnPointerUpHandler != null)
            OnPointerUpHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }


}
