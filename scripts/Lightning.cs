using Godot;
using System;

public partial class Lightning : ColorRect
{
	public float angle = 0;
	private bool flash;

	public static Lightning instance;

	ShaderMaterial shader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		shader = this.Material as ShaderMaterial;
        GameManager.OnBeat += GameManager_OnBeat;
		shader.SetShaderParameter("duration", 1);
	}

    private void GameManager_OnBeat(BeatType beat)
    {
		if (beat.HasFlag(BeatType.MainBeat))
		{
			flash = true;
			angle = 0;
			shader.SetShaderParameter("thunder", true);
			shader.SetShaderParameter("halfthunder", true);
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

		shader.SetShaderParameter("center", Frog.instance.ToUV() );
		
		if (flash && !Frog.instance.win) {
			if (angle <= Math.PI)
			{
				angle = angle + 0.1f;
			}
			if (angle >= Math.PI)
            {
				shader.SetShaderParameter("halfthunder", false);
				angle += 0.025f;
            }
			shader.SetShaderParameter("duration", Math.Sin(angle));
			if (angle >=  (3f * Math.PI) / 2f) 
            {
				flash = false;
				shader.SetShaderParameter("thunder", false);
			}
        }

		if (Frog.instance.win)
        {
			
			if (angle <= Math.PI)
			{
				angle = angle + 0.1f;
			}
			if (angle >= Math.PI)
			{
				Frog.instance.win = false;
			}
			shader.SetShaderParameter("duration", Math.Sin(angle));

		}
		//GD.Print(angle);
		
	}


	public override void _EnterTree()
	{
		instance = this;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		instance = null;
	}
}
