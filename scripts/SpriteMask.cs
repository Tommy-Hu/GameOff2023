using Godot;
using System;

public partial class SpriteMask : Sprite2D
{
    [Export]
    public float rotateSpeed;

    public Action update = null;

    private Vector2 normalScale;
    private Vector2 smallScale;

    public override void _Ready()
    {
        base._Ready();
        this.rotateSpeed *= (int)(RNG.Gen.Randi() % 2U) * 2 - 1;
        normalScale = Scale;
        smallScale = normalScale * 0.2f;
        Scale = smallScale;
        GameManager.OnBeat += GameManager_OnBeat;
    }

    private void GameManager_OnBeat(BeatType beat)
    {
        if (beat.HasFlag(BeatType.MainBeat))
        {
            Scale = normalScale;
            scaleTimer = 0;
        }
    }

    private const float SCALE_TOTAL_TIME = 2f;
    private float scaleTimer = 0;
    public override void _Process(double delta)
    {
        base._Process(delta);
        scaleTimer += (float)delta;
        scaleTimer = Mathf.Min(scaleTimer, SCALE_TOTAL_TIME);
        Rotate(rotateSpeed * (float)delta);
        update?.Invoke();

        ShaderMaterial mat = (Material as ShaderMaterial);
        float amount = GameManager.GetBeatProgressOffBeat(0.1f);
        mat.SetShaderParameter("radius", amount + 0.2f);
        mat.SetShaderParameter("innerRadius", amount);

        Scale = normalScale.Lerp(smallScale, scaleTimer / SCALE_TOTAL_TIME);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        (GetParent().GetParent() as SpriteMaskMaster).QueueRedraw();
    }
}
