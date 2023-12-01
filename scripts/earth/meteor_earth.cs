using Godot;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

public partial class meteor_earth : Area2D 
{
	[Export]
	public int damageAmount = 1;
	PackedScene plMeteorEffect = (PackedScene)GD.Load("res://scenes/earth/meteor_effect_earth.tscn");
	[Export]
	public float minSpeed = 50;
	[Export]
	public float maxSpeed = 80;
	[Export]
	public float minRotationRate = -30;
	 [Export]
	public float maxRotationRate = -30;
	public float speed = 0;
	public float rotationRate = 0;
	private Random rand = new Random();
	private Vector2 vel = new Vector2(0, 0);

	[Export]
	public double life = 20;

	public Action onDeath;

	public bool isBoss = false;
	

	public override void _Ready()
	{
		speed = (float) (minSpeed + (rand.NextDouble() * maxSpeed));
		rotationRate = (float) (minRotationRate + (rand.NextDouble() * maxRotationRate));
		GameManager.OnBeat += onBeat;

	}

	public void onBeat(BeatType beattype)
	{
		if (isBoss)
		{
			return;
		}

		if (beattype.HasFlag(BeatType.MainBeat))
		{
			vel.X += (GD.Randf() - 0.5f) * 2f * speed * 0.75f;
			var cam = GetViewport().GetCamera2D() as Cam;
			cam.shake(0.25f, 6);
		}

	}

	public override void _PhysicsProcess(double delta)
	{   
		RotationDegrees += (float) (rotationRate * delta);

		vel.Y = speed;
		Position += vel * (float)delta;

		if (GlobalPosition.Y > GetViewportRect().Size.Y + 5)
		{
			QueueFree();
		}
		
	}

	public void damage (int AMOUNT) 
	{
		life -= AMOUNT;
		if (life <= 0) 
		{
			onDeath?.Invoke();
			var effect = plMeteorEffect.Instantiate<CpuParticles2D>();
			effect.Position = Position;
			GetParent().AddChild(effect);
			QueueFree();
		}
	}
	
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
	
		
	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("Earth")) 
		{
			planet_earth.singleton.damage(damageAmount);
			QueueFree();
		}
	}

    public override void _ExitTree()
    {
       GameManager.OnBeat -= onBeat;
    }

}




