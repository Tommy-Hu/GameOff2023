using Godot;
using System;

public partial class PlayerProgress : Node2D
{

	public Sprite2D icon;
	public ProgressBar progressBar;

	public float currIconPosition;
	public float playerProgress;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon = this.GetNode<Sprite2D>("PlayerIcon");
		progressBar = this.GetNode<ProgressBar>("FrogProgressBar");
		currIconPosition = icon.GlobalPosition.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		playerProgress = Frog.instance.Position.Y / LilyPadSpawner.instance.height * 1;
		icon.GlobalPosition = new Vector2(icon.GlobalPosition.X, currIconPosition + 480f * playerProgress);

		progressBar.Value = ( Death.instance.GlobalPosition.Y)/ LilyPadSpawner.instance.height * -100;
		GD.Print(progressBar.Value);
	}


}
