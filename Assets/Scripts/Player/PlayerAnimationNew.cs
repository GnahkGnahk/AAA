using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimationNew : Singleton<PlayerAnimationNew>
{
    [SerializeField] Animator animatorController;

    public Animator GetAnimatorController()
    {
        return animatorController;
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
    public void MoveBool(bool isIdle = false, bool isCrouchedWalk = false, bool isStandardWalk = false, bool isRun = false)
    {
        animatorController.SetBool("Idle", isIdle);
        animatorController.SetBool("CrouchedWalk", isCrouchedWalk);
        animatorController.SetBool("StandardWalk", isStandardWalk);
        animatorController.SetBool("Run", isRun);
    }
    public void MoveAnimation(float velocity)
    {
        animatorController.SetFloat(Helper.ANIMATOR_VELOCITY, velocity);
    }

    public void FallFlat()
    {
        animatorController.SetTrigger(Helper.ANIMATOR_FALL_FLAT);
    }

    public void PickItem(PickUpType pickUpType)
    {
        Debug.Log("Choice: " +  pickUpType);
        switch (pickUpType)
        {
            case PickUpType.HIGH:
                animatorController.SetTrigger(Helper.ANIMATOR_PICK_HIGH);
                break;
            case PickUpType.NORMAL:
                animatorController.SetTrigger(Helper.ANIMATOR_PICK_NORMAL);
                break;
            case PickUpType.GROUND:
                animatorController.SetTrigger(Helper.ANIMATOR_PICK_GROUND);
                break;
        }
    }
}