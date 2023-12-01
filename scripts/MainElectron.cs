using Godot;

public partial class MainElectron : Node2D
{
    PackedScene mobScene;

    public override void _Ready()
    {
        mobScene = ResourceLoader.Load<PackedScene>("res://scenes/prefabs/photon.tscn");
    }
    private void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Photon mob = mobScene.Instantiate<Photon>();

        // Spawn the mob by adding it to the Main scene.
        GetChild(2).AddChild(mob);
    }
}