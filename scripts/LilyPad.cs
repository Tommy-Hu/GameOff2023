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
		lilyPadSprite =  this.GetNode<Sprite2D>("LilyPadSprite2D");


		int num = (int)(GD.Randi() % 3u);

        switch (num)
        {
			case 0:
				lilyPadSprite.Texture = lilyPad1;
				break;
			case 1:
				lilyPadSprite.Texture = lilyPad2;
				break;
			case 2:
				lilyPadSprite.Texture = lilyPad3;
				break;
			default:
                break;
        }
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
