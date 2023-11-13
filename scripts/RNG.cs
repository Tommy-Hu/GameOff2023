using Godot;
using System;

public partial class RNG : Node2D
{
    private static RandomNumberGenerator rng = null;
    public static RandomNumberGenerator Gen => rng;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (rng == null)
        {
            rng = new RandomNumberGenerator();
            rng.Seed = ~0x7531357UL ^ Time.GetTicksMsec();
        }
    }
}
