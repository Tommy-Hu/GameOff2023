using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerCell : RigidBody2D
{
    [Export]
    public float speed = 5f;
    [Export]
    public float shootInterval = 3f;

    private bool mouseWasPressed = false;
    private float shootTimer = 3f;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += PlayerCell_BodyEntered;

        var maskScn = GD.Load<PackedScene>("res://scenes/prefabs/cell_mask.tscn");
        var mask = maskScn.Instantiate<SpriteMask>();
        const float SCALE = 0.20f;
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
        if (CellsManager.lost || CellsManager.won) return;

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
        if (CellsManager.lost || CellsManager.won) return;

        base._Process(delta);
        bool wasGreater = shootTimer > 0;
        shootTimer -= (float)delta;
        CellLevelUIManager.instance.SetGunCountdown((int)(100 - shootTimer * 100f / shootInterval));
        if (shootTimer <= 0)
        {
            if (wasGreater)
                GameManager.PlaySFX("cell_gun_cock.wav");
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                shootTimer = shootInterval;
                if (!mouseWasPressed)
                {
                    GameManager.PlaySFX("cell_gun.mp3");
                    var projScn = GD.Load<PackedScene>("res://scenes/prefabs/cell_projectile.tscn");
                    var proj = projScn.Instantiate<CellProjectile>();
                    GetParent().AddChild(proj);
                    proj.destination = GetGlobalMousePosition();
                    proj.GlobalPosition = GlobalPosition;
                    mouseWasPressed = true;
                }
            }
            else
            {
                mouseWasPressed = false;
            }
        }
        else
        {
            mouseWasPressed = false;
        }
    }

    private void PlayerCell_BodyEntered(Node body)
    {
        if (CellsManager.lost || CellsManager.won) return;

        if (body is Cell cell)
        {
            cell.Slurp(this);
        }
    }
}
