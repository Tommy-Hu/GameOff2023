using Godot;
using System;

public partial class Frog : RigidBody2D
{
	public bool jumpIncreaseing;
	public bool jumpHeld;
	public Sprite2D direction;
	public bool onGround = true;
	public Sprite2D sprite;
	public Area2D area;


	public CollisionShape2D collider;

	public Texture2D idle = ResourceLoader.Load("res://sprites/FrogAssets/frog_idle.png") as Texture2D;
	public Texture2D midJump = ResourceLoader.Load("res://sprites/FrogAssets/frog_jump.png") as Texture2D;

	[ExportCategory("Frog")]
	[ExportGroup("Jump Params")]
	[Export] 
	private float jumpForceMin = 200;
	[Export]
	public float jumpForceMax = 1000;
	[Export]
	public float jumpinterval = 15;
	[Export]
	public float curJumpForce = 200;

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		area = this.GetNode<Area2D>("FrogGroundCheck");
		direction = this.GetNode<FrogJumpDirector>("FrogJumpDirector");
		sprite = this.GetNode<Sprite2D>("FrogSprite");

		collider = this.GetNode<CollisionShape2D>("FrogCollider");
		curJumpForce = jumpForceMin;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!onGround)
        {
			sprite.Texture = midJump;
		} else
        {
			sprite.Texture = idle;
        }
	}

    public override void _PhysicsProcess(double delta)
    {
		GD.Print(collider.Disabled);

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
			
			
			//GD.Print(curJumpForce);
			

		}
		if (onGround)
		{
			if (Input.IsActionPressed("FrogJump"))
			{

				jumpHeld = true;
			}
			else if (Input.IsActionJustReleased("FrogJump"))
			{
				jumpHeld = false;
				ApplyImpulse(new Vector2((float)curJumpForce * direction.Position.Normalized().X, 1.5f * curJumpForce * direction.Position.Normalized().Y), new Vector2(0, 0));
				curJumpForce = jumpForceMin;
				//collider.Disabled = true;
				
			}
		}
	}

	private void OnFrogGroundCheckBodyEntered(Node2D body)
    {
		if (this.LinearVelocity.Y >= 0)
		{
			onGround = true;
			//collider.Disabled = false;
		}
		
    }

	private void OnFrogGroundCheckBodyExited(Node2D body)
    {
		onGround = false;
		
	}


}
