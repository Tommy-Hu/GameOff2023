using Godot;

public partial class MainElectron : Node2D
{
    [Export]
    PackedScene MobScene;

    private void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Photon mob = MobScene.Instantiate<Photon>();

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
}