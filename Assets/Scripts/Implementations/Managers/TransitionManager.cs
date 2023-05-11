using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public Animator animator;
    public void StartAnimationOnRoomOpens()
    {
        animator.Play("StartRoom");
    }
    public void StartAnimationOnRoomEnds()
    {
        animator.Play("EndRoom");
    }
}
