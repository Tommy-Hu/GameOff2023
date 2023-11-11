using Godot;
using System;

public partial class SelfDestroyingParticle : GpuParticles2D
{
    public float lifeLeft = 2f;
    public bool running = false;

    public override void _Process(double delta)
    {
        if (running)
        {
            lifeLeft -= (float)delta;
            if (Emitting && lifeLeft <= 0)
            {
                Emitting = false;
                var timer = GetTree().CreateTimer(Lifetime * 1.0f / SpeedScale, true, false, false);
                timer.Timeout += Timer_Timeout;
            }
        }
    }

    private void Timer_Timeout()
    {
        QueueFree();
    }

    public void StartSelfDestroy(float lifeLeft)
    {
        running = true;
        this.lifeLeft = lifeLeft;
    }

    public void Detatch(Node to)
    {
        this.Reparent(to);
    }
}
