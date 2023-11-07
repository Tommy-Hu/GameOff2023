using Godot;
using System;

public partial class Frog : RigidBody2D
{
	public int jumpForce = 10;
	public bool jumpHeld;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D area2D = this.GetNode<Area2D>("GroundCheck");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if (Input.IsActionPressed("FrogJump"))
		{
			if (!jumpHeld)
			{
				ApplyImpulse(new Vector2(0, -800), new Vector2(0, 0));
			}
			jumpHeld = true;
		}
		else if (Input.IsActionJustReleased("FrogJump"))
        {
			jumpHeld = false;
        }
	}


}
