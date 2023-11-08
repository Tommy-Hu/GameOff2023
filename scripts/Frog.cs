using Godot;
using System;

public partial class Frog : RigidBody2D
{
	public float jumpForceMin = 200;
	public float jumpForceMax = 1000;
	public float jumpinterval = 15;
	public float curJumpForce = 200;
	public bool jumpIncreaseing;
	public bool jumpHeld;
	public Sprite2D direction;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D area2D = this.GetNode<Area2D>("GroundCheck");
		direction = this.GetNode<FrogJumpDirector>("FrogJumpDirector");
		curJumpForce = jumpForceMin;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
        
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if (jumpHeld)
		{
			if (curJumpForce <= jumpForceMin)
			{
				jumpIncreaseing = true;
			}
			if (curJumpForce >= jumpForceMax)
            {
				jumpIncreaseing = false;
			}
			curJumpForce = jumpIncreaseing ? (curJumpForce + jumpinterval) : (curJumpForce - jumpinterval);
			
			
			GD.Print(curJumpForce);
			GD.Print(jumpIncreaseing);

		}

		if (Input.IsActionPressed("FrogJump"))
		{
			
			jumpHeld = true;
		}
		else if (Input.IsActionJustReleased("FrogJump"))
        {
			jumpHeld = false;
			ApplyImpulse(new Vector2((float)curJumpForce * direction.Position.Normalized().X, 1.5f* curJumpForce * direction.Position.Normalized().Y), new Vector2(0, 0));
			curJumpForce = jumpForceMin;
		}
	}


}
