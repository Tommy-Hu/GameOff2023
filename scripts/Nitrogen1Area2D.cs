using Godot;
using System;
using System.ComponentModel;

public partial class Nitrogen1Area2D : Area2D
{
	SubAtomicCharge parentNode;
	PlayerElectron player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		parentNode = (SubAtomicCharge) GetParent();
		player = (PlayerElectron) GetParent().GetParent().GetParent().GetChild(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		// Polling for checking whether the player is in designated radius
		if (OverlapsBody(player))
		{
			parentNode.InPlayerRange(true);
		} 
		else
		{
			parentNode.InPlayerRange(false);
		}

	}
}
