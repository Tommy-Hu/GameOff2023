using Godot;
using System;

public partial class CellLevelUIManager : MarginContainer
{
    [Export]
    public NodePath cancerCountLabel;
    [Export]
    public NodePath cancerCountdownLabel;

    public static CellLevelUIManager instance;

    private Label cancerCount;
    private Label cancerCountdown;

    public override void _EnterTree()
    {
        cancerCount = GetNode<Label>(cancerCountLabel);
        cancerCountdown = GetNode<Label>(cancerCountdownLabel);
        instance = this;
    }

    public void SetCancerCountdown(float timer)
    {
        cancerCountdown.Text = $"{timer:.#}s";
    }

    public void SetCancerCount(int count)
    {
        cancerCount.Text = $"{count}";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        instance = null;
    }
}
