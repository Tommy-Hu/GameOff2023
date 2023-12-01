using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerElectron : SubAtomicCharge
{
    int FORCE_ADJUSTMENT = 200;
    private AnimatedSprite2D daSprite;
    private Node2D arrowRotator;
    private Node2D movementArrowRotator;
    private Area2D goal;
    private GpuParticles2D sprintParticles;
    bool powered = false;
    
	List<SubAtomicCharge> electromagneticCharges = new() { };

    public override void _Ready()
    {
        base._Ready();
        Charge = -1;
        
        
        goal = GetParent().GetChild<Area2D>(6);

        daSprite = GetChild<AnimatedSprite2D>(1);
        arrowRotator = GetChild<Node2D>(3);
        movementArrowRotator = GetChild<Node2D>(4);
        sprintParticles = GetChild<GpuParticles2D>(5);

        GameManager.OnBeat += GameManager_OnBeat;
    }

    public override void _Process(double delta)
    {
        daSprite.Play();
        RotateArrow();
    }

    private void RotateArrow()
    {
        Vector2 vectorToGoal = goal.GlobalPosition - GlobalPosition;
        if (vectorToGoal.X > 0)
        {
            arrowRotator.GlobalRotation = Mathf.Atan(vectorToGoal.Y / vectorToGoal.X);
        }
        else
        {
            arrowRotator.GlobalRotation = Mathf.Pi + Mathf.Atan(vectorToGoal.Y / vectorToGoal.X);
        }
        arrowRotator.GlobalRotation += Mathf.Pi / 2;
    }

    private void RotateMovementArrow(Vector2 v)
    {
        if (v.X >= 0)
        {
            movementArrowRotator.GlobalRotation = Mathf.Atan(v.Y / v.X);
        }
        else 
        {
            movementArrowRotator.GlobalRotation = Mathf.Pi + Mathf.Atan(v.Y / v.X);
        }
        movementArrowRotator.GlobalRotation += Mathf.Pi / 2;
    }

    private void GameManager_OnBeat(BeatType beat)
    {
        if (beat.HasFlag(BeatType.PhraseBeat))
        {
            ApplyCentralImpulse(CalculateElectromagneticPull(8) / 16);
        }
        if (beat.HasFlag(BeatType.MainBeat))
        {
            //ApplyCentralImpulse(CalculateElectromagneticPull(1) / 16);
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
        if (movement == Vector2.Zero)
            movementArrowRotator.Visible = false;
        else
        {
            RotateMovementArrow(movement);
            movementArrowRotator.Visible = true;
        }
        if (powered)
        {
            ApplyForce(movement * FORCE_ADJUSTMENT / 20);
            sprintParticles.Emitting = true;
            movementArrowRotator.GetChild<Node2D>(0).Scale = new Vector2(0.5f, 0.3f);
            if (LinearVelocity.LengthSquared() > 250_000)
            {
                LinearVelocity *= 250_000 / LinearVelocity.LengthSquared();
            }
        }
        else
        {
            ApplyForce(movement * FORCE_ADJUSTMENT / 25);
            sprintParticles.Emitting = false;
            movementArrowRotator.GetChild<Node2D>(0).Scale = new Vector2(0.5f, 0.2f);
            if (LinearVelocity.LengthSquared() > 90_000)
            {
                LinearVelocity *= 90_000 / LinearVelocity.LengthSquared();
            }
        }
            
    }

    private Vector2 CalculateElectromagneticPull(int multiplier) 
	{
		Vector2 forceVector = Vector2.Zero;
		foreach (var e in electromagneticCharges)
		{
            float distance = GlobalPosition.DistanceTo(e.GlobalPosition);
			float forceMagnitude = e.Charge / distance;
			forceVector += FORCE_ADJUSTMENT * multiplier * forceMagnitude * (e.GlobalPosition - GlobalPosition).Normalized();
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
        GameManager.PlayRandomSFX("cell_bullet_splat.wav", "cell_bullet_splat1.wav");
        GetChild(1).QueueFree();
        GetChild(3).QueueFree();
        GetChild(4).QueueFree();
        GameManager.PlayLevelFade("electron", "Electron");
    }

    public void OnPowerUpAreaEntered(Node2D body)
    {
        if (body == this)
        {
            powered = !powered;
            if (powered)
                GameManager.PlaySFX("power_up.wav");
            else GameManager.PlaySFX("kill_bad_cell2.wav");
        }
    }

    public void OnGoalAreaEntered(Node2D body)
    {
        if (body == this)
        {
            GameManager.PlayRandomSFX("kill_bad_cell1.wav");
            GetParent<Node2D>().QueueFree();
            GD.Print("You win");
            GameManager.PlayLevelFade("cells", "Cells");
        }
        
    }
}
