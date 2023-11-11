using Godot;
using System;

public partial class PlayerCell : RigidBody2D
{
    [Export]
    public float speed = 5f;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += PlayerCell_BodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Vector2 movement = Vector2.Zero;
        if (Input.IsKeyPressed(Key.W)) movement += Vector2.Up;
        if (Input.IsKeyPressed(Key.A)) movement += Vector2.Left;
        if (Input.IsKeyPressed(Key.S)) movement += Vector2.Down;
        if (Input.IsKeyPressed(Key.D)) movement += Vector2.Right;
        if (movement != Vector2.Zero) movement = movement.Normalized();
        ApplyForce(movement * speed);
    }

    private void PlayerCell_BodyEntered(Node body)
    {
        if (body is Cell cell)
        {
            cell.Slurp(this);
        }
    }
}
