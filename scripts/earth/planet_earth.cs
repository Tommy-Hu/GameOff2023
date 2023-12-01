using Godot;
using System;

public partial class planet_earth : Area2D
{
	[Export]
	public int maxLife = 10;
	private int currentLife = 10;

	public static planet_earth singleton = null;

	public override void _EnterTree()
	{
		singleton = this;
		currentLife = maxLife;
	
	}

	public void damage(int amount)
	{
		currentLife -= amount;
		var progressBar = GetParent().FindChild("HealthBar").GetChild<ProgressBar>(0);
		progressBar.Value = (currentLife / (float) maxLife) * 100;

		var cam = GetViewport().GetCamera2D() as Cam;
		cam.shake(0.5f, 5);
		
		
		GD.Print("Current life: ", currentLife);
		if (currentLife <= 0) 
		{
			GD.Print("Player died!");
			QueueFree();
		}
	}

	public double getLife()
	{
		return this.currentLife;
	}

	public double getCurrentLife()
	{
		return this.currentLife;
	}

	public override void _ExitTree()
	{
		singleton = null;
	}

}
