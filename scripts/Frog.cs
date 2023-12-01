using Godot;
using System;

public partial class Frog : RigidBody2D
{
	public bool jumpIncreaseing;
	public bool jumpHeld;
	public Node2D direction;
	public bool onGround = true;
	public Sprite2D sprite;
	public Area2D area;
	public Vector2 screenBounds;

	public bool start;

	public static Frog instance;

	public CollisionShape2D collider;
	public bool win =false;

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
		win = false;
		
		
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
		ScreenWrap();
		screenBounds = FrogCamara2D.instance.bounds;
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
			direction.Scale = new Vector2(direction.Scale.X, 1 * (curJumpForce - 200) / 800+1);
			
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
				start = true;
				direction.Scale = new Vector2(1, 1);
				direction.Visible = false;

				GameManager.PlaySFX("FrogJump.wav");
				

			}
		}
	}

	private void OnFrogGroundCheckBodyEntered(Node2D body)
    {
		if (this.LinearVelocity.Y >= 0)
		{
			onGround = true;
			//collider.Disabled = false;
			direction.Visible = true;
		}
	
		if (body.Name == "SuperiorLilyPad")
        {
			win = true;
			Lightning.instance.angle = 0;
			GameManager.PlayLevel("earth", "Earth");

        }
		


	}

	private void OnFrogGroundCheckBodyExited(Node2D body)
    {
		onGround = false;
		
	}

	private void OnDeathBodyEntered(Node2D body)
    {
		if (body.Name == "Frog")
		{
			GameManager.PlayLevelFade("frog","Frog");
	
		}
		
	}

	private void ScreenWrap()
    {
		if (GlobalPosition.X < screenBounds.X)
        {
			GlobalPosition = new Vector2 (screenBounds.Y, GlobalPosition.Y);
        }
		if (GlobalPosition.X > screenBounds.Y)
        {
			GlobalPosition = new Vector2(screenBounds.X, GlobalPosition.Y);

		}
	
    }

	public Vector2 ToUV()
    {
		Vector2 UV;
		UV = FrogCamara2D.instance.GetCanvasTransform() * this.GlobalPosition;
		return new Vector2((UV.X/FrogCamara2D.instance.bounds.Y)/2, 0.5f);
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
