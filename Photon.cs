using Godot;
using System;

public partial class Photon : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	float speed;
	public override void _Ready()
    {
        base._Ready();
        speed = 1000f;
        SetRandomSpawn();
        SetRandomVelocity();    
    }

    private void SetRandomVelocity()
    {
        float velocityX = GD.Randf();
        float velocityY = GD.Randf();
        Vector2 velocity = new Vector2(velocityX, velocityY);
        velocity = velocity.Normalized();
        Velocity = velocity * speed;
    }

    private void SetRandomSpawn()
    {
        int positionX = ((int)GD.Randi()) % 16384 - 8192;
        int positionY = ((int)GD.Randi()) % 8192 - 4096;
        Position = new Vector2(positionX, positionY);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		base._Process(delta);
        Richochet(delta);
    }

    private void Richochet(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null)
        {
            if (collision.GetCollider().HasMethod("Hit"))
            {
                collision.GetCollider().Call("Hit");
                QueueFree();
                return;
            }
            else
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
            }
        }
    }


}
