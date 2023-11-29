using Godot;
using System;

public partial class Nitrogen1 : SubAtomicCharge
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	PlayerElectron player;
	bool nearPlayer = false;
	bool previousNearPlayer = false;
	
	public override void _Ready()
	{
		base._Ready();
		Charge = 7;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        if (!nearPlayer && previousNearPlayer)
        {
            player.RemoveElectromagneticCharge(this);
        }
        else if (nearPlayer && !previousNearPlayer)
        {
            player.AddElectromagneticCharge(this);
        }
		else
		{
			// pass, no need modifications
		}
    }

	public override void InPlayerRange(bool status)
	{
		previousNearPlayer = nearPlayer;
		nearPlayer = status;
	}
}
