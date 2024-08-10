using System;
using System.Collections;
using System.Collections.Generic;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private TransitionAnimator transitionAnimator;
    [SerializeField] private TransitionProfile[] transitionProfiles;

    private void Awake()
    {
        GameManager.Instance.Transition = this;
    }

    public void StartTransition(UnityAction callback)
    {
        transitionAnimator.profile = transitionProfiles[Random.Range(0, transitionProfiles.Length)];
        
        transitionAnimator.progress = 0f;
        transitionAnimator.profile.invert = false;
        
        transitionAnimator.onTransitionEnd.RemoveAllListeners();
        transitionAnimator.onTransitionEnd.AddListener(callback);
        
        transitionAnimator.Play();
    }
    
    public void EndTransition(UnityAction callback)
    {
        transitionAnimator.onTransitionEnd.RemoveAllListeners();
        transitionAnimator.onTransitionEnd.AddListener(callback);
        
        transitionAnimator.progress = 0f;
        transitionAnimator.profile.invert = true;
        
        transitionAnimator.Play();
    }


}
