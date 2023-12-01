using Godot;
using System;

public partial class LilyPadSpawner : Node2D
{
	public PackedScene scene = GD.Load<PackedScene>("res://scenes/prefabs/lily_pad.tscn");

	[Export]
	public float height = 4000;
	public float curHeight = 0;
	public int curZIndex= 999;

	public static LilyPadSpawner instance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		curZIndex = 999;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (curHeight < height)
		{
			int num = (int)(GD.Randi() % 2u);
			SpawnLilyPad(curHeight, curZIndex, num);
			curHeight += 250;
			curZIndex -= 1;
		}
	}

	public void SpawnLilyPad(float height, int z, int amount)
	{
		for (int i = 0; i <= amount; i++)
		{
			float xpos = (float)GD.RandRange((double)FrogCamara2D.instance.bounds.X, (double)FrogCamara2D.instance.bounds.Y);
			LilyPad instance = scene.Instantiate<LilyPad>();

			AddChild(instance);
			instance.GlobalPosition = new Vector2(xpos, -1 * height);
			instance.setSpriteZIndex(z);
		}
	}

	public override void _EnterTree()
	{
		instance = this;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		instance = null;
	}
}
