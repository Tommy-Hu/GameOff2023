using Godot;
using System;

public partial class SuperiorLilyPad : StaticBody2D
{
	public Sprite2D lilyPadSprite;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lilyPadSprite = this.GetNode<Sprite2D>("LilyPadGold");		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setSpriteZIndex(int z)
	{
		lilyPadSprite.ZIndex = z;

	}
}
