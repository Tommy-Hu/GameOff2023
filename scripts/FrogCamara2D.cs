using Godot;
using System;

public partial class FrogCamara2D : Camera2D
{
	public Vector2 topCorner;
	public Vector2 bounds;
	public static FrogCamara2D instance;
	public float followSpeed = 20f;

	public override void _EnterTree()
	{
		instance = this;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		instance = null;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		topCorner = GetCanvasTransform().AffineInverse() * Vector2.Zero;
		bounds = new Vector2(topCorner.X, topCorner.X+2.0f*(GlobalPosition.X-topCorner.X));
		

		GlobalPosition = new Vector2(GlobalPosition.X, Frog.instance.GlobalPosition.Y);

	}
	
}
