using Godot;
using System;

public partial class bullet_earth : Area2D
{
	public double speed = 500;
	public float AMOUNT = 5;
	
	public override void _PhysicsProcess(double delta)
	{
		this.Position += new Vector2(0,-AMOUNT);
	}

	public void _OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
