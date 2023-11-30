using Godot;
using System;

public partial class LilyPad : StaticBody2D
{

	public Texture2D lilyPad1 = ResourceLoader.Load("res://sprites/FrogAssets/lilyPad1.png") as Texture2D;
	public Texture2D lilyPad2 = ResourceLoader.Load("res://sprites/FrogAssets/lilyPad2.png") as Texture2D;
	public Texture2D lilyPad3 = ResourceLoader.Load("res://sprites/FrogAssets/lilyPad3.png") as Texture2D;

	public Sprite2D lilyPadSprite;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
