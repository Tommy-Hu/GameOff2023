using Godot;
using System;

public partial class PlayerController : Node2D
{
	[Export]
	public float speed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 movement = Vector2.Zero;
		if (Input.IsKeyPressed(Key.W))
		{
			--movement.Y;
		}
		if (Input.IsKeyPressed(Key.A))
		{
			--movement.X;
		}
		if (Input.IsKeyPressed(Key.S))
		{
			++movement.Y;
		}
		if (Input.IsKeyPressed(Key.D))
		{
			++movement.X;
		}
		Position += movement * (float)delta * speed;
	}
}
