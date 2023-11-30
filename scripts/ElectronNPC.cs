using Godot;
using System;

public partial class ElectronNPC : SubAtomicCharge
{
    // Called when the node enters the scene tree for the first time.
    PlayerElectron player;
    [Export]
    Nitrogen1 center;
    bool nearPlayer = false;
    bool previousNearPlayer = false;
    float orbitalRadius;
    float BASE_FORCE_CORRECTION = 20f;
    float BASE_TANGENTIAL_CORRECTION = 250f;

    public override void _Ready()
    {
        base._Ready();
        Charge = -1;
        player = (PlayerElectron)GetParent().GetParent().GetParent().GetParent().GetChild(0);
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
        Vector2 vectorToCenter = center.Position - Position;       
        ApplyCentripetalForce(vectorToCenter);
        ApplyTangentialForce(vectorToCenter);
    }

    public override void InPlayerRange(bool status)
    {
        previousNearPlayer = nearPlayer;
        nearPlayer = status;
    }

    private void ApplyCentripetalForce(Vector2 vectorToCenter)
    {
        float offset = vectorToCenter.Length() - orbitalRadius;
        if (offset > 0)
        {
            ApplyForce(vectorToCenter / orbitalRadius * BASE_FORCE_CORRECTION * 5);
        }
        else if (offset < 0)
        {
            float a = 1 - vectorToCenter.Length() / orbitalRadius;
            ApplyForce(-1 * a * vectorToCenter * BASE_FORCE_CORRECTION);
        }
        else
        {
            ApplyTangentialForce(vectorToCenter);
        }
        GD.Print("Offset", offset);
    }

    private void ApplyTangentialForce(Vector2 vectorToCenter)
    {
        ApplyForce(vectorToCenter.Rotated((float)Math.PI / 2) / BASE_TANGENTIAL_CORRECTION / BASE_TANGENTIAL_CORRECTION * orbitalRadius);
    }

    public void Initialize(Nitrogen1 centerPivot, float distanceValue)
    {
        center = centerPivot;
        orbitalRadius = distanceValue;
    }
}
