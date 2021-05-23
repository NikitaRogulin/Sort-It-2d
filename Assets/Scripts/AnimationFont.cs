using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFont : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("Failed");
    }
}
