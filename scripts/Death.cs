using Godot;
using System;

public partial class Death : Area2D
{
	public static Death instance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Frog.instance.start)
		{
			Position = new Vector2(Position.X, Position.Y - 1f);
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
