using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class bullet_earth : Area2D
{
	public double speed = 500;
	public float AMOUNT = 5;
	
	public override void _PhysicsProcess(double delta)
	{
		this.Position += new Vector2(0,-AMOUNT);
	}

	public void _OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("damageable")) 
		{
			area.Call("damage","2");
			QueueFree();
		}
		
	}	
}


