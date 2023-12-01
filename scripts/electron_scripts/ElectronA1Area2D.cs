using Godot;
using System;
using System.ComponentModel;

public partial class ElectronA1Area2D : Area2D
{
    ElectronNPC parentNode;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        parentNode = (ElectronNPC)GetParent();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
