using System;
using System.Collections;
using System.Collections.Generic;
using TooltipManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Vector2 startPosition;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        _canvasGroup = transform.GetComponent<CanvasGroup>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
        TooltipSystem.Hide();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.localPosition = startPosition;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        GetComponent<TooltipTrigger>().OnFinishDrop();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       startPosition = _rectTransform.localPosition;
       _canvasGroup.blocksRaycasts = false;
       _canvasGroup.alpha = 0.6f;
    }
    
}

