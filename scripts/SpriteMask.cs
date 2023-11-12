using Godot;
using System;

public partial class SpriteMask : Sprite2D
{
    private Vector2I screenPos;
    private float strength;

    public Vector2I ScreenPos
    {
        get => screenPos; set
        {
            screenPos = value;
            (GetParent() as Node2D).QueueRedraw();
        }
    }
    public float Strength
    {
        get => strength; set
        {
            strength = value;
            (GetParent() as Node2D).QueueRedraw();
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        (GetParent().GetParent() as SpriteMaskMaster).QueueRedraw();
    }
}
