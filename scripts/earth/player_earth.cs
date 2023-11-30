using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class player_earth : Area2D
{	PackedScene plBullet = (PackedScene)GD.Load("res://scenes/earth/bullet_earth.tscn");

	private AnimatedSprite2D animatedSprite;
	private Node2D firingPositions;
	private Timer fireDelayTimer;
	
	[Export]
	public double life = 3;

	[Export]
	public float speed = 300;

	[Export]
	public float fireDelay = 0.1f;

	private Vector2 vel = new Vector2(0, 0);

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		firingPositions = GetNode<Node2D>("FiringPositions");
		fireDelayTimer = GetNode<Timer>("FireDelayTimer");
	}

	public override void _Process(double delta)
	{
		// Animate
		if (vel.X < 0)
			animatedSprite.Play("Left");
		else if (vel.X > 0)
			animatedSprite.Play("Right");
		else
			animatedSprite.Play("Idle");

		// Check if shooting
		if (Input.IsActionPressed("shoot") && fireDelayTimer.IsStopped())
		{
			fireDelayTimer.Start(fireDelay);
			foreach (Node2D child in firingPositions.GetChildren())
			{
				var bullet = plBullet.Instantiate<Node2D>();
				bullet.GlobalPosition = child.GlobalPosition;
				GetTree().CurrentScene.AddChild(bullet);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		vel.X = 0;
		vel.Y = 0;
		if (Input.IsActionPressed("move_left"))
			vel.X -= speed;
		if (Input.IsActionPressed("move_right"))
			vel.X += speed;
		if (Input.IsActionPressed("move_up"))
			vel.Y -= speed;
		if (Input.IsActionPressed("move_down"))
			vel.Y += speed;

		vel = vel.Normalized() * speed;
		Position += vel * (float)delta;

		// Make sure we stay within the screen
		Rect2 viewRect = GetViewportRect();
		var newPosition = Position;
		newPosition.X = Mathf.Clamp(Position.X, 0, 1153);
		Position = newPosition;
		
	}

	public void damage(int amount)
	{
		life -= amount;
		EmitSignal("OnPlayerLifeChangedEventHandler", life);
		

		Console.WriteLine("Current life: ", life);
		if (life <= 0) 
		{
			Console.WriteLine("Player died!");
			QueueFree();
		}
	}
}