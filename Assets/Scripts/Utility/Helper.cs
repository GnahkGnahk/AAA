public class Helper
{
    //====================================================================== LAYER
    public const int LAYER_PLATFORM = 9;
    


    //====================================================================== ANIMATOR
    public const string ANIMATOR_VELOCITY = "Velocity";

    public const string ANIMATOR_JUMP_FROM_IDLE = "JumpFromIdle";
    public const string ANIMATOR_JUMP_FROM_MOVING = "JumpFromMoving";

    public const string ANIMATOR_IDLE = "Idle";
    public const string ANIMATOR_CROUCH_IDLE = "CrouchedIdle";
    public const string ANIMATOR_CROUCH_WALK = "CrouchedWalk";
    public const string ANIMATOR_STANDARD_WALK = "StandardWalk";
    public const string ANIMATOR_RUN = "Run";

    public const string ANIMATOR_PICK_HIGH = "PickHigh";
    public const string ANIMATOR_PICK_NORMAL = "PickNormal";
    public const string ANIMATOR_PICK_GROUND = "PickGround";

    public const string ANIMATOR_FALL_FLAT = "FallFlat";


    //====================================================================== TAG
    public const string TAG_LOCKER_N = "Locker_Normal";
    public const string TAG_TRASH_BIN = "TrashBin";
    public const string TAG_ITEM_TRADE = "ItemTrade";
    public const string TAG_UNTAGGED = "Untagged";
}

public enum PickUpType
{
    GROUND,
    NORMAL,
    HIGH
}

public enum CameraType
{
    TOP_DOWN,
    CROUCHING,
}
