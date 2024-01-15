using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : Singleton<PlayerAnimation>
{
    [SerializeField] Animator animatorController;

    public void Move(bool isIdle = false, bool isCrouchedWalk = false, bool isStandardWalk = false, bool isRun = false)
    {
        animatorController.SetBool("Idle", isIdle);
        animatorController.SetBool("CrouchedWalk", isCrouchedWalk);
        animatorController.SetBool("StandardWalk", isStandardWalk);
        animatorController.SetBool("Run", isRun);
    }

    public void Jump()
    {
        if (animatorController.GetBool("Idle"))
        {
            //Debug.Log("Jump from idle");
            animatorController.SetTrigger("JumpFromIdle");
            return;
        }

        if (animatorController.GetBool("StandardWalk"))
        {
            //Debug.Log("Jump from StandardWalk");
            animatorController.SetTrigger("JumpFromMoving");
            return;
        }
        if (animatorController.GetBool("Run"))
        {
            //Debug.Log("Jump from Run");
            animatorController.SetTrigger("JumpFromMoving");
            return;
        }
    }

    public Animator GetAnimatorController()
    {
        return animatorController;
    }

    public void FallFlat()
    {
        animatorController.SetTrigger("FallFlat");
    }

    public void PickItem(PickUpType pickUpType)
    {
        Debug.Log("Choice: " +  pickUpType);
        switch (pickUpType)
        {
            case PickUpType.HIGH:
                animatorController.SetTrigger("PickHigh");
                break;
            case PickUpType.NORMAL:
                animatorController.SetTrigger("PickNormal");
                break;
            case PickUpType.GROUND:
                animatorController.SetTrigger("PickGround");
                break;
        }
    }
}

public enum PickUpType
{
    GROUND,
    NORMAL,
    HIGH
}