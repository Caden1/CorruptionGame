using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
    private Animator animator;
    private string currentState;

    public AnimationManager(Animator animator) {
        this.animator = animator;
    }

    public void ChangeState(string newState)
    {
        if (currentState != newState) {
            animator.Play(newState);
            currentState = newState;
        }
    }
}
