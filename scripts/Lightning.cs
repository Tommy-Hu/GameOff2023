using Godot;
using System;

public partial class Lightning : ColorRect
{
	private float angle = 0;
	private bool flash;



	ShaderMaterial shader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shader = this.Material as ShaderMaterial;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		

		if (Input.IsActionJustReleased("FrogJump"))
		{
			flash = true;
			angle = 0;
			shader.SetShaderParameter("thunder", true);
		}
        if (flash) {
			angle = angle + 0.1f;
			shader.SetShaderParameter("duration", Math.Sin(angle));
			shader.GetShaderParameter("");
			if (angle >=  (3f*Math.PI)/2f) 
            {
				flash = false;
				shader.SetShaderParameter("thunder", false);
			}
        }
		//GD.Print(angle);
		GD.Print(Math.Sin(angle));
	}
}
