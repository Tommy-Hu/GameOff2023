using Godot;
using System;

public partial class Nitrogen1 : SubAtomicCharge
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	PlayerElectron player;
	
	public override void _Ready()
	{
		Charge = 7;
		player.AddElectromagneticCharge(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
