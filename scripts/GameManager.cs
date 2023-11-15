using Godot;
using System;

public partial class GameManager : Node2D
{
    private AudioStreamPlayer2D musicPlayer;

    public override void _EnterTree()
    {
        base._EnterTree();
        musicPlayer = GetParent().GetChild<AudioStreamPlayer2D>(1);
    }

    private void DestroyCurrentLevel()
    {
        var children = GetChildren();
        for (int i = children.Count - 1; i >= 0; i--)
        {
            Node child = children[i];
            RemoveChild(child);
            child.QueueFree();
        }
        children.Clear();
    }

    private void LoadNextLevel(string path)
    {
        PackedScene pack = ResourceLoader.Load<PackedScene>(path);
        Node2D nd = pack.Instantiate<Node2D>();
        AddChild(nd);
    }

    public void PlayLevel(string levelName, string musicName)
    {
        DestroyCurrentLevel();
        LoadNextLevel($"res://scenes/level_{levelName}.tscn");
        PlayMusic(musicName);
    }

    private void PlayMusic(string musicName)
    {
        float pos = musicPlayer.GetPlaybackPosition();
        musicPlayer.Stream = ResourceLoader.Load<AudioStream>($"res://music/Conjugate {musicName}.wav");
        musicPlayer.Play(pos);
    }
}
