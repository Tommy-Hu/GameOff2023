using Godot;
using System;

public partial class Cam : Camera2D
{
	private float shakeAmount = 0.0f;
	private float shakeTime = 0;

	public override void _Process(double delta)
	{
		shakeTime -= (float) delta;

		if (shakeTime > 0)
		{
			GlobalPosition = new Vector2((GD.Randf() - 0.5f) * 2f * shakeAmount, (GD.Randf() - 0.5f) * 2f * shakeAmount);
		
		} else {
			GlobalPosition = Vector2.Zero;
		}
	}

	public void shake(float time, float amount)
	{
		shakeAmount = amount; 
		shakeTime = time;
	}

}
