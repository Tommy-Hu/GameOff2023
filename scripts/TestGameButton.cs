using Godot;
using System;

public partial class TestGameButton : Button
{
    [Export]
    public string levelName;
    [Export]
    public string musicName;

    public override void _Ready()
    {
        this.Pressed += TestGameButton_Pressed;
    }

    private void TestGameButton_Pressed()
    {
        GameManager.PlayLevelFade(levelName, musicName);
    }
}
