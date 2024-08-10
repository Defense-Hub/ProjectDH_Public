using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialTouchEventHandler : MonoBehaviour, IPointerDownHandler
{
    private TutorialUIController tutorialUIController;

    private void Awake()
    {
        tutorialUIController = transform.parent.GetComponent<TutorialUIController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tutorialUIController.TouchCallBack();
    }
}
