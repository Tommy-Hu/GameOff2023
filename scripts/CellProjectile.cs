using Godot;
using System;

public partial class CellProjectile : Sprite2D
{
    [Export]
    public float speed = 1000f;

    public Vector2 destination;

    private bool activated = false;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (activated)
        {
            return;
        }

        GlobalPosition = GlobalPosition.MoveToward(destination, (float)delta * speed);
        if (GlobalPosition.DistanceSquaredTo(destination) < (1 * 1))
        {
            var maskScn = GD.Load<PackedScene>("res://scenes/prefabs/cell_mask.tscn");
            var mask = maskScn.Instantiate<SpriteMask>();
            const bool SHOTGUN = false;
            const float SCALE = 0.20f;
            if (SHOTGUN)
            {
                mask.Texture = ResourceLoader.Load<Texture2D>("res://sprites/reveal_gradient_shot_gun.png");
                mask.rotateSpeed = 0f;
                Transform2D t = Transform2D.Identity;
                t = t.Rotated((destination - GlobalPosition).Angle() + Mathf.Pi * 0.25f);
                t[2] += GlobalTransform[2];
                //t[2] += GetGlobalMousePosition();
                t = t.TranslatedLocal(new Vector2(1f, -1f) * (Vector2)mask.Texture.GetImage().GetSize() * 0.5f * SCALE);
                mask.GlobalTransform = t;
            }
            else
            {
                mask.Texture = ResourceLoader.Load<Texture2D>("res://sprites/reveal_gradient_0.png");
                mask.GlobalPosition = destination;
            }
            mask.GlobalScale = Vector2.One * SCALE;
            SpriteMaskMaster.AddMask(mask);
            GameManager.PlayRandomSFX("cell_bullet_splat.wav","cell_bullet_splat1.wav");
            activated = true;
        }
    }
}
