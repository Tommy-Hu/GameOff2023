using Godot;
using System;

public partial class CellLevelUIManager : MarginContainer
{
    [Export]
    public NodePath cancerCountProgress;
    [Export]
    public NodePath cancerCountLabel;
    [Export]
    public NodePath cancerCountdownProgress;
    [Export]
    public NodePath gunCountdownProgress;
    [Export]
    public NodePath deathUIPath;

    public static CellLevelUIManager instance;

    private TextureProgressBar cancerCountBar;
    private Label cancerCountText;
    private TextureProgressBar cancerCountdown;
    private TextureProgressBar gunCountdown;
    private Panel deathUI;

    public override void _EnterTree()
    {
        cancerCountBar = GetNode<TextureProgressBar>(cancerCountProgress);
        cancerCountText = GetNode<Label>(cancerCountLabel);
        cancerCountdown = GetNode<TextureProgressBar>(cancerCountdownProgress);
        gunCountdown = GetNode<TextureProgressBar>(gunCountdownProgress);
        deathUI = GetNode<Panel>(deathUIPath);
        instance = this;
    }

    public void SetCancerCountdown(int progress)
    {
        cancerCountdown.Value = progress;
    }

    public void SetCancerCount(int count, int progress)
    {
        cancerCountText.Text = $"{count}";
        cancerCountBar.Value = progress;
        cancerCountBar.TintProgress = Color.FromHsv(Mathf.Lerp(0f, 57f, 1f - progress / 100f) / 360f, 79f / 100f, 83f / 100f, 1f);
    }

    public void SetGunCountdown(int progress)
    {
        gunCountdown.Value = progress;
    }

    public void SetDeathUIVisible(bool visible)
    {
        deathUI.Visible = visible;
    }

    public void _on_restart_pressed()
    {
        GameManager.PlayLevelFade("cells", "Cell");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        instance = null;
    }
}
