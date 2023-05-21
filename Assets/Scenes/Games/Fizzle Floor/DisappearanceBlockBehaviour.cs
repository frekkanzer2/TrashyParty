using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearanceBlockBehaviour : MonoBehaviour
{
    public Animator animator;
    public void Disappear()
    {
        animator.Play("Disappear");
    }
}
