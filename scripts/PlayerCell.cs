using Godot;
using System;

public partial class PlayerCell : RigidBody2D
{
    [Export]
    public float speed = 5f;

    private bool mouseWasPressed = false;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += PlayerCell_BodyEntered;

        var maskScn = GD.Load<PackedScene>("res://scenes/prefabs/cell_mask.tscn");
        var mask = maskScn.Instantiate<SpriteMask>();
        const float SCALE = 0.15f;
        mask.Texture = ResourceLoader.Load<Texture2D>("res://sprites/reveal_gradient_0.png");
        mask.GlobalPosition = GlobalPosition;
        mask.GlobalScale = Vector2.One * SCALE;
        mask.update = () =>
        {
            mask.GlobalPosition = GlobalPosition + GetViewportRect().Size * 0.5f;
        };
        SpriteMaskMaster.AddMask(mask);
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

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if (!mouseWasPressed)
            {
                var maskScn = GD.Load<PackedScene>("res://scenes/prefabs/cell_mask.tscn");
                var mask = maskScn.Instantiate<SpriteMask>();
                const bool SHOTGUN = true;
                const float SCALE = 0.15f;
                if (SHOTGUN)
                {
                    mask.Texture = ResourceLoader.Load<Texture2D>("res://sprites/reveal_gradient_shot_gun.png");
                    mask.rotateSpeed = 0f;
                    Transform2D t = Transform2D.Identity;
                    t = t.Rotated((GetGlobalMousePosition() - GlobalPosition).Angle() + Mathf.Pi * 0.25f);
                    t[2] += GlobalTransform[2];
                    //t[2] += GetGlobalMousePosition();
                    t = t.TranslatedLocal(new Vector2(1f, -1f) * (Vector2)mask.Texture.GetImage().GetSize() * 0.5f * SCALE);
                    mask.GlobalTransform = t;
                }
                else
                {
                    mask.Texture = ResourceLoader.Load<Texture2D>("res://sprites/reveal_gradient_0.png");
                    mask.GlobalPosition = GetGlobalMousePosition();
                }
                mask.GlobalScale = Vector2.One * SCALE;
                SpriteMaskMaster.AddMask(mask);
                mouseWasPressed = true;
            }
        }
        else
        {
            mouseWasPressed = false;
        }
    }

    private void PlayerCell_BodyEntered(Node body)
    {
        if (body is Cell cell)
        {
            cell.Slurp(this);
        }
    }
}
