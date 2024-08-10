using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntTimeTextHandler : MonoBehaviour
{
    private Timer timer;
    private TextMeshProUGUI timerText;
    private Camera MainCamera;
    
    private void Awake()
    {
        timer = transform.parent.GetComponent<Timer>();
        timerText = GetComponent<TextMeshProUGUI>();

        timer.OnChangeTimerText += ChangeText;
        
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }

    private void Init()
    {

    }

    private void ChangeText(int time)
    {
        timerText.text = time.ToString();
    }

    private void Update()
    {
        transform.LookAt(transform.position + MainCamera.transform.forward);
    }
}
