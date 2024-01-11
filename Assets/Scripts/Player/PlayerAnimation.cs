using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : Singleton<PlayerAnimation>
{
    [SerializeField] Animator animatorController;

    public void Move(bool isIdle = false, bool isStandardWalk = false, bool isRunning = false)
    {
        animatorController.SetBool("StandardWalk", isStandardWalk);
        animatorController.SetBool("Idle", isIdle);
    }

    public void Jump()
    {
        if (animatorController.GetBool("Idle"))
        {
            Debug.Log("Jump from idle");
            animatorController.SetTrigger("JumpFromIdle");
            return;
        }

        if (animatorController.GetBool("StandardWalk"))
        {
            Debug.Log("Jump from StandardWalk");
            animatorController.SetTrigger("JumpFromRunning");
            return;
        }
    }
}
