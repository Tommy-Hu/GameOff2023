using Godot;
using System;
using System.Collections.Generic;

public partial class meteor_effect_earth : CpuParticles2D
{
	
	public override void _Ready()
	{
		Emitting = true;
	}

	public override void _Process(double delta)
	{
		if (!Emitting) 
		{
			QueueFree();
		}
	}



}
