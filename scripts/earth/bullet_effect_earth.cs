using Godot;
using System;

public partial class bullet_effect_earth : Sprite2D
{
	private void _on_timer_timeout()
	{
		QueueFree();
	}

}


