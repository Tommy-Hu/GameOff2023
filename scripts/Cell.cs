using Godot;
using System;

/// <summary>
/// Represents a non-player cell.
/// </summary>
public partial class Cell : StaticBody2D
{
    public static readonly Color GOOD_COLOR = new Color(1, 1, 1, 1f);
    public static readonly Color BAD_COLOR = new Color(1, .1f, .15f, 1f);

    [Export]
    public bool good;

    private bool squishing = false;
    private PlayerCell squishTo = null;
    private Sprite2D sprite;
    private ShaderMaterial material;
    private float slurpProcess;
    private ParticleProcessMaterial ppm;

    public void SetGood(bool good)
    {
        this.good = good;
        material.SetShaderParameter("tint", good ? GOOD_COLOR : BAD_COLOR);
        var colorRamp = ResourceLoader.Load<GradientTexture1D>($"res://scenes/prefabs/{(good ? "good" : "bad")}_cell_color_ramp.tres");
        ppm.ColorRamp = colorRamp;
    }

    public override void _Ready()
    {
        base._Ready();
        sprite = GetChild<Sprite2D>(1);
        material = (sprite.Material.Duplicate() as ShaderMaterial);
        sprite.Material = material;
        material.SetShaderParameter("offset", new Vector2(RNG.Gen.Randf(), RNG.Gen.Randf()));
        material.SetShaderParameter("tint", good ? GOOD_COLOR : BAD_COLOR);
        material.SetShaderParameter("squish", 0.0f);
        material.SetShaderParameter("mask_texture", SpriteMaskMaster.Singleton.Texture);
        var particle = GetChild<SelfDestroyingParticle>(2);
        ppm = (particle.ProcessMaterial.Duplicate() as ParticleProcessMaterial);
        particle.ProcessMaterial = ppm;
        var colorRamp = ResourceLoader.Load<GradientTexture1D>($"res://scenes/prefabs/{(good ? "good" : "bad")}_cell_color_ramp.tres");
        ppm.ColorRamp = colorRamp;
    }

    public void Slurp(PlayerCell into)
    {
        if (!squishing)
        {
            GameManager.PlayRandomSFX("wobble1.wav", "wobble2.wav");
            CellsManager.instance.CellDie(this);
            if (!good)
            {
                GameManager.PlayRandomSFX("kill_bad_cell1.wav", "kill_bad_cell2.wav");
            }
            squishing = true;
            squishTo = into;
            var particle = GetChild<SelfDestroyingParticle>(2);
            particle.StartSelfDestroy(2f);
            particle.Emitting = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (squishing)
        {
            GetChild<CollisionShape2D>(0).Disabled = true;
            CollisionLayer = 0;
            CollisionMask = 0;
            if (slurpProcess >= 1.0f)
            {
                var particle = GetChild<SelfDestroyingParticle>(2);
                particle.Detatch(GetParent());
                QueueFree();
                return;
            }
            material.SetShaderParameter("squish", slurpProcess);
            GlobalPosition = GlobalPosition.Lerp(squishTo.GlobalPosition, slurpProcess * slurpProcess);
            // compare the second column of the basis matrices to rotate
            //GlobalRotation = Mathf.Lerp(GlobalRotation, GlobalRotation + GlobalTransform.Y.AngleTo(squishTo.GlobalTransform.Y), 1);
            float angle = (squishTo.GlobalPosition - GlobalPosition).Angle() + Mathf.Pi * 0.5f;
            GlobalRotation = Mathf.LerpAngle(GlobalRotation, angle, 1);
            slurpProcess += (float)delta;
        }
    }
}
