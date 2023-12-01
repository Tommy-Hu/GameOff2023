using Godot;
using System;

public partial class Nitrogen1 : SubAtomicCharge
{
	// Called when the node enters the scene tree for the first time.
	PackedScene electronScene;
	PlayerElectron player;

    private const float RADIUS_MULTIPLIER = 500f;

    [Export]
    bool spawnElectron = true;
	
	public override void _Ready()
	{
		base._Ready();
		electronScene = ResourceLoader.Load<PackedScene>("res://scenes/prefabs/electron.tscn");
		player = (PlayerElectron) GetParent().GetParent().GetParent().GetChild(0);
        CollisionShape2D shape = GetChild<Nitrogen1Area2D>(1).GetChild<CollisionShape2D>(0);
        shape.Shape = (Shape2D)shape.Shape.Duplicate();
        ((CircleShape2D)shape.Shape).Radius = GetRadius();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
    }

	private float GetRadius()
	{
        float biggestRadius = 0f;
		for (int i = 1; i <= Charge; i++)
        {
            float orbitRadius = DetermineRadius(i);
            float alfa = DetermineRotation(i);
            biggestRadius = orbitRadius;
            if (spawnElectron == true)
                InstantiateElectrons(i, orbitRadius, alfa);
        }
        return biggestRadius;
    }

    private void InstantiateElectrons(int i, float orbitRadius, float alfa)
    {
        ElectronNPC electron = electronScene.Instantiate<ElectronNPC>();
        electron.Initialize(this, player, orbitRadius, alfa);
        GetChild(3).AddChild(electron);
    }

    public static float DetermineRadius(int i)
    {
        float distanceValue;
        if (i == 1 || i == 2)
        {
            distanceValue = 1;
        }
        else if (i < 10)
        {
            float proportion = (float)(i - 2) / 8;
            distanceValue = 1 + proportion;
        }
        else
        {
            float proportion = (float)(i - 10) / 8;
            distanceValue = 2 + proportion;
        }

        return distanceValue * RADIUS_MULTIPLIER;
    }

    public static float DetermineRotation(int i)
    {
        float rotationValue;
        if (i < 3) {
            rotationValue = Mathf.Pi * i;
        }
        else if (i < 11)
        {
            float proportion = (float)(i - 2) / 8;
            rotationValue = 2 * Mathf.Pi * proportion;
        }
        else
        {
            float proportion = (float)(i - 10) / 8;
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
