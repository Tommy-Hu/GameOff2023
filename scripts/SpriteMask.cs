using Godot;
using System;

public partial class SpriteMask : Sprite2D
{
    [Export]
    public float rotateSpeed;

    public Action update = null;

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

    public override void _Ready()
    {
        base._Ready();
        this.rotateSpeed *= (int)(RNG.Gen.Randi() % 2U) * 2 - 1;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Rotate(rotateSpeed * (float)delta);
        update?.Invoke();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        (GetParent().GetParent() as SpriteMaskMaster).QueueRedraw();
    }
}
