using Godot;
using System;

public partial class Goal : AnimatedSprite2D
{
    public static bool finished = false;
    public override void _Ready()
    {
        Play("default");
        finished = false;
    }

    public override void _Process(double delta)
    {
        if (finished) return;
        var player = GetParent().GetChild<PlayerElectron>(0);
        if (player.GlobalPosition.X >= GlobalPosition.X && player.GlobalPosition.Y >= GlobalPosition.Y)
        {
            GameManager.PlayLevel("cells", "Cells");
            finished = true;
        }
    }
}
