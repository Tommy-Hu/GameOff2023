using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class bullet_earth : Area2D
{
	PackedScene plBulletEffect = (PackedScene)GD.Load("res://scenes/earth/bullet_effect_earth.tscn");
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
			var BulletEffect = plBulletEffect.Instantiate<Sprite2D>();
			BulletEffect.Position = Position;
			GetParent().AddChild(BulletEffect);

			var cam = GetViewport().GetCamera2D() as Cam;
			cam.shake(0.1f, 3);

			area.Call("damage","1");
			QueueFree();
		}
		
	}	
}


