using Godot;
using System;

public partial class PlayerElectron : SubAtomicCharge
{
	[Export]
	float speed = 5f;


	Godot.Collections.Dictionary<Vector2, int> electromagneticCharges = new() { };

    public override void _Ready()
    {
        base._Ready();
        Charge = -1;
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        ApplyInputForce();
    }

    private void ApplyInputForce()
    {
        Vector2 movement = Vector2.Zero;
        if (Input.IsKeyPressed(Key.W)) movement += Vector2.Up;
        if (Input.IsKeyPressed(Key.A)) movement += Vector2.Left;
        if (Input.IsKeyPressed(Key.S)) movement += Vector2.Down;
        if (Input.IsKeyPressed(Key.D)) movement += Vector2.Right;
        if (movement != Vector2.Zero) movement = movement.Normalized();
        ApplyForce(movement * speed);
        ApplyElectromagneticForce();
    }

    public Vector2 ApplyElectromagneticForce() 
	{
		Vector2 forceVector = Vector2.Zero;
		foreach (var keyValuePairs in electromagneticCharges)
		{
            float distanceSquared = Position.DistanceSquaredTo(keyValuePairs.Key);
			float forceMagnitude = keyValuePairs.Value / distanceSquared;
			forceVector += 10_000 * forceMagnitude * (keyValuePairs.Key - Position).Normalized();
		}
        ApplyForce(forceVector);
        return forceVector;
	}

	public bool AddElectromagneticCharge(SubAtomicCharge type)
    { 
        electromagneticCharges.Add(type.Position, type.Charge);
        return true;
	}

    public bool RemoveElectromagneticCharge(SubAtomicCharge type)
    {
        return electromagneticCharges.Remove(type.Position);
    }

    public override void InPlayerRange(bool status)
    {
        // pass, do nothing
    }

    public void Hit()
    {
        GD.Print("You lose");
    }
}
