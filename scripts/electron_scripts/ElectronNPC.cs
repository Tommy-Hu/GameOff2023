using Godot;
using System;
using System.Diagnostics;

public partial class ElectronNPC : SubAtomicCharge
{
    // Called when the node enters the scene tree for the first time.
    PlayerElectron player;
    [Export]
    Nitrogen1 center;
    private AnimatedSprite2D daSprite;

    private const float SPIN = 1f;
    float orbitalRadius;
    float revolutionAngle;

    public override void _Ready()
    {
        base._Ready();
        Charge = -1;
        Freeze = true;
        daSprite = GetChild<AnimatedSprite2D>(1);
        CollisionShape2D shape = GetChild<ElectronA1Area2D>(2).GetChild<CollisionShape2D>(0);
        shape.Shape = (Shape2D)shape.Shape.Duplicate();
        ((CircleShape2D)shape.Shape).Radius = 0.5f * Nitrogen1.DetermineRadius(1);
    }

    public override void _Process(double delta)
    {
        daSprite.Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Revolve(delta);
    }

    private void Revolve(double delta)
    {
        revolutionAngle += (float)delta * SPIN;
        Vector2 newPosition = Vector2.Zero;
        newPosition.X = (float)Math.Cos(revolutionAngle)*orbitalRadius;
        newPosition.Y = (float)Math.Sin(revolutionAngle)*orbitalRadius;
        Position = newPosition;
    }

    public void Initialize(Nitrogen1 centerPivot, PlayerElectron thePlayer, float distanceValue, float startingAngle)
    {
        center = centerPivot;
        player = thePlayer;
        orbitalRadius = distanceValue;
        revolutionAngle = startingAngle;
    }

    public void OnBodyInRange(Node2D body)
    {
        if (body == player)
        {
            player.AddElectromagneticCharge(this);
        }
        
    }

    public void OnBodyOutRange(Node2D body)
    {
        if (body == player)
        {
            player.RemoveElectromagneticCharge(this);
        }
    }
}
