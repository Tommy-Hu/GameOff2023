using Godot;
using System;

public partial class Spawner : Node2D
{
	[Export]
	public int meteorsTilBoss = 5;
	[Export]
	public float spawnInterval = 2;
	float nextSpawnTime = 2;
	PackedScene plMeteor ;



	public override void _Ready()
	{
		plMeteor = (PackedScene)GD.Load("res://scenes/earth/meteor_earth.tscn");
	}

	public override void _Process(double delta)
	{
		nextSpawnTime -= (float) delta;
		if (nextSpawnTime <= 0)
		{
			nextSpawnTime = spawnInterval;
			
			meteorsTilBoss -= 1;
			meteor_earth spawnedMeteor = plMeteor.Instantiate<meteor_earth>();

			AddChild(spawnedMeteor);
			float spawnX = GD.Randi() % (uint) GetViewportRect().Size.X;

			if (meteorsTilBoss == 0)
			{
				spawnedMeteor.GlobalScale *= 5;
				spawnedMeteor.speed = 20;
				spawnedMeteor.life = 300;
				spawnX = GetViewportRect().Size.X / 2;
				spawnedMeteor.damageAmount = 100;
				spawnedMeteor.ZIndex = -1;
			}

			spawnedMeteor.GlobalPosition = new Vector2(spawnX, this.GlobalPosition.Y);
		}
	}

}


