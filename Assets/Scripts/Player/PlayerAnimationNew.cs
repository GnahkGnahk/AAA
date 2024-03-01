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