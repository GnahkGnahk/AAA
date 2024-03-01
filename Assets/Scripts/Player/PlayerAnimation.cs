using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : Singleton<PlayerAnimation>
{
    [SerializeField] Animator animatorController;

    public void Move(bool isIdle = false, bool isCrouchedWalk = false, bool isStandardWalk = false, bool isRun = false)
    {
        animatorController.SetBool(Helper.ANIMATOR_IDLE, isIdle);
        animatorController.SetBool(Helper.ANIMATOR_CROUCH_WALK, isCrouchedWalk);
        animatorController.SetBool(Helper.ANIMATOR_STANDARD_WALK, isStandardWalk);
        animatorController.SetBool(Helper.ANIMATOR_RUN, isRun);
    }

    public void Jump()
    {
        if (animatorController.GetBool(Helper.ANIMATOR_IDLE))
        {
            //Debug.Log("Jump from idle");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_IDLE);
            return;
        }

        if (animatorController.GetBool(Helper.ANIMATOR_STANDARD_WALK))
        {
            //Debug.Log("Jump from StandardWalk");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_MOVING);
            return;
        }
        if (animatorController.GetBool(Helper.ANIMATOR_RUN))
        {
            //Debug.Log("Jump from Run");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_MOVING);
            return;
        }
    }

    public Animator GetAnimatorController()
    {
        return animatorController;
    }

    public void FallFlat()
    {
        animatorController.SetTrigger(Helper.ANIMATOR_FALL_FLAT);
    }

    public void PickItem(PickUpType pickUpType)
    {
        Debug.Log("Choice: " + pickUpType);
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