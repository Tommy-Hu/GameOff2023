using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerElectron : SubAtomicCharge
{
    int FORCE_ADJUSTMENT = 200;

	List<SubAtomicCharge> electromagneticCharges = new() { };

    public override void _Ready()
    {
        base._Ready();
        Charge = -1;
        GameManager.OnBeat += GameManager_OnBeat;
    }

    private void GameManager_OnBeat(BeatType beat)
    {
        if (beat.HasFlag(BeatType.PhraseBeat))
        {
            ApplyCentralImpulse(CalculateElectromagneticPull(4) / 16);
        }
        if (beat.HasFlag(BeatType.MainBeat))
        {
            ApplyCentralImpulse(CalculateElectromagneticPull(1) / 16);
        }
        if (beat.HasFlag(BeatType.QuarterBeat))
        {
            //ApplyCentralImpulse(CalculateElectromagneticPull(1) / 16);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        ApplyInputForce();
        ApplyForce(CalculateElectromagneticPull(1));
        
    }

    private void ApplyInputForce()
    {
        Vector2 movement = Vector2.Zero;
        if (Input.IsKeyPressed(Key.W)) movement += Vector2.Up;
        if (Input.IsKeyPressed(Key.A)) movement += Vector2.Left;
        if (Input.IsKeyPressed(Key.S)) movement += Vector2.Down;
        if (Input.IsKeyPressed(Key.D)) movement += Vector2.Right;
        if (movement != Vector2.Zero) movement = movement.Normalized();
        ApplyForce(movement * FORCE_ADJUSTMENT / 40);
    }

    private Vector2 CalculateElectromagneticPull(int multiplier) 
	{
		Vector2 forceVector = Vector2.Zero;
		foreach (var e in electromagneticCharges)
		{
            float distance = Position.DistanceTo(e.Position);
			float forceMagnitude = e.Charge / distance;
			forceVector += FORCE_ADJUSTMENT * multiplier * forceMagnitude * (e.Position - Position).Normalized();
		}
        return forceVector;
	}

    public bool AddElectromagneticCharge(SubAtomicCharge type)
    {
        electromagneticCharges.Add(type);
        return true;
	}

    public bool RemoveElectromagneticCharge(SubAtomicCharge type)
    {        
        return electromagneticCharges.Remove(type);
    }

    public void Hit()
    {
        QueueFree();
        GD.Print("You lose");
    }


}
