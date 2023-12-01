using Godot;
using System;

public partial class FrogJumpDirector : Node2D
{
	private float distance;
	private float cycleTime = 2;
	private double angle = 0;
	private bool clockwise;
	private bool canSpin = true;
	public Frog frog;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		frog = GetParent<Frog>();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!frog.jumpHeld && frog.onGround)
		{

			if (angle >= Math.PI)
			{
				clockwise = false;
			}
			else if (angle <= 0)
			{
				clockwise = true;
			}

			if (!clockwise)
			{
				angle = angle - 0.07;
			}
			else if (clockwise)
			{
				angle = angle + 0.07;
			}

			
			this.Rotation = -(float)angle + (float)Math.PI / 2;
			Position = new Vector2((float)Math.Cos(angle) * 50, (float)Math.Sin(angle) * -50);
			
			
			


			
		}

	}
}
