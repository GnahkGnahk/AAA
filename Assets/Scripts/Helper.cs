using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    //====================================================================== LAYER
    public const int LAYER_PLATFORM = 9;


    //====================================================================== ANIMATOR
    public const string ANIMATOR_VELOCITY = "Velocity";

    public const string ANIMATOR_PICK_HIGH = "PickHigh";
    public const string ANIMATOR_PICK_NORMAL = "PickNormal";
    public const string ANIMATOR_PICK_GROUND = "PickGround";
    public const string ANIMATOR_FALL_FLAT = "FallFlat";
}

public enum PickUpType
{
    GROUND,
    NORMAL,
    HIGH
}
