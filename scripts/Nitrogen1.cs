using Godot;
using System;

public partial class Nitrogen1 : SubAtomicCharge
{
	// Called when the node enters the scene tree for the first time.
	PackedScene electronScene;
	PlayerElectron player;

    private const float RADIUS_MULTIPLIER = 500f;
	
	public override void _Ready()
	{
		base._Ready();
		electronScene = ResourceLoader.Load<PackedScene>("res://scenes/prefabs/electron.tscn");
		player = (PlayerElectron) GetParent().GetParent().GetChild(0);
        CollisionShape2D shape = GetChild<Nitrogen1Area2D>(1).GetChild<CollisionShape2D>(0);
        shape.Shape = (Shape2D)shape.Shape.Duplicate();
        ((CircleShape2D)shape.Shape).Radius = SpawnElectrons();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
    }

	private float SpawnElectrons()
	{
        float biggestRadius = 0f;
		for (int i = 0; i < Charge; i++)
        {
            ElectronNPC electron = electronScene.Instantiate<ElectronNPC>();

            float orbitRadius = DetermineRadius(i);
            float alfa = DetermineRotation(i);

            electron.Initialize(this, player, orbitRadius, alfa);
            GetChild(3).AddChild(electron);
            electron.Position = new Vector2(orbitRadius * Mathf.Cos(i*2*Mathf.Pi/Charge), orbitRadius * Mathf.Sin(i*2 * Mathf.Pi / Charge));
            biggestRadius = Mathf.Max(biggestRadius, orbitRadius);
        }
        return biggestRadius;
    }

    private static float DetermineRadius(int i)
    {
        float distanceValue;
        if (i == 0)
        {
            distanceValue = 1;
        }
        else if (i == 1)
        {
            distanceValue = 0.5f;
        }
        else if (i < 10)
        {
            float proportion = (float)(i - 1) / 8;
            distanceValue = 2 - proportion;
        }
        else
        {
            float proportion = (float)(i - 9) / 8;
            distanceValue = 3 - proportion;
        }

        return distanceValue * RADIUS_MULTIPLIER;
    }

    private static float DetermineRotation(int i)
    {
        float rotationValue;
        if (i<2) {
            rotationValue = Mathf.Pi * i;
        }
        else if (i < 10)
        {
            float proportion = (float)(i - 1) / 8;
            rotationValue = 2 * Mathf.Pi * proportion;
        }
        else
        {
            float proportion = (float)(i - 9) / 8;
            rotationValue = 2 * Mathf.Pi * proportion;
        }

        return rotationValue;
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

    public void OnHit(Node body)
    {
        if (body == player)
        {
            player.Hit();
        }
    }
}
