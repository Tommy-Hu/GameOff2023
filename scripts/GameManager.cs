using Godot;
using System;

[Flags]
public enum BeatType
{
    /// <summary>
    /// The super loud beat every phrase. (every two measures)
    /// </summary>
    PhraseBeat = 0,
    /// <summary>
    /// The loud beat every measure.
    /// </summary>
    MainBeat = 1 << 1,
    /// <summary>
    /// The lighter beat 4 times per measure.
    /// </summary>
    QuarterBeat = 1 << 2,
}

public partial class GameManager : Node2D
{
    /// <summary>
    /// Beats per minute of the music.
    /// </summary>
    public const int BPM = 90;
    /// <summary>
    /// The time between each (normalized) beat.
    /// </summary>
    public const float BEAT_TIME = 60.0f / BPM;
    /// <summary>
    /// Inverse of BEAT_TIME.
    /// </summary>
    public const float INV_BEAT_TIME = BPM / 60.0f;
    /// <summary>
    /// Un-delay this many seconds to the beats.
    /// </summary>
    public static float UNDELAY = 0.05f;

    private static GameManager singleton;

    private AudioStreamPlayer2D musicPlayer;

    public delegate void BeatEventHandler(BeatType beat);
    /// <summary>
    /// All observers of OnBeat will be called every time a beat just played in the BGM.
    /// </summary>
    public static event BeatEventHandler OnBeat;

    private float lastFramePos = 0f;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (singleton != null)
        {
            singleton.QueueFree();
        }
        singleton = this;
        musicPlayer = GetParent().GetChild<AudioStreamPlayer2D>(1);
    }

    /// <summary>
    /// THIS IS PRIVATE.
    /// </summary>
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
        OnBeat = null;// destroy observer links.
    }

    /// <summary>
    /// THIS IS PRIVATE.
    /// </summary>
    /// <param name="path"></param>
    private void LoadNextLevel(string path)
    {
        PackedScene pack = ResourceLoader.Load<PackedScene>(path);
        Node2D nd = pack.Instantiate<Node2D>();
        AddChild(nd);
    }

    /// <summary>
    /// Destroys the previous level, and transitions to the given level.
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="musicName"></param>
    public static void PlayLevel(string levelName, string musicName)
    {
        singleton.DestroyCurrentLevel();
        singleton.LoadNextLevel($"res://scenes/level_{levelName}.tscn");
        singleton.PlayMusic(musicName);
    }

    /// <summary>
    /// THIS IS PRIVATE.
    /// </summary>
    /// <param name="musicName"></param>
    private void PlayMusic(string musicName)
    {
        float pos = musicPlayer.GetPlaybackPosition();
        musicPlayer.Stream = ResourceLoader.Load<AudioStream>($"res://music/Conjugate {musicName}.wav");
        musicPlayer.Play(pos);
    }

    /// <summary>
    /// Returns the progress of the current beat.
    /// 0 means the current beat just started;
    /// 0.5 means it's half way between the current beat and the next beat;
    /// 1 means the next beat (the value should not be 1 because that would wrap to 0 of the next beat).
    /// </summary>
    public static float BeatProgress
    {
        get
        {
            return GetBeatProgressOffBeat(0);
        }
    }

    /// <summary>
    /// Returns the progress of the current beat, off by the given amount.
    /// </summary>
    /// <param name="offbeatAmount"></param>
    /// <returns></returns>
    public static float GetBeatProgressOffBeat(float offbeatAmount)
    {
        float pos = singleton.lastFramePos + offbeatAmount;
        return (pos - Mathf.FloorToInt(pos * INV_BEAT_TIME) * BEAT_TIME) * INV_BEAT_TIME;
    }

    public override void _Process(double delta)
    {
        UNDELAY = (float)delta * 2.0f;
        base._Process(delta);
        float pos = musicPlayer.GetPlaybackPosition() - UNDELAY;
        //GD.Print(pos);
        // the index of the next beat
        int nextBeat = Mathf.CeilToInt(lastFramePos * INV_BEAT_TIME);
        // the playback time of the next expected beat
        float nextBeatTime = nextBeat * BEAT_TIME;
        if (nextBeatTime <= pos)
        {
            // passed the next beat
            BeatType type = BeatType.QuarterBeat;
            if (nextBeat % 4 == 0) type |= BeatType.MainBeat;
            if (nextBeat % 8 == 0) type |= BeatType.PhraseBeat;
            GD.Print(nextBeat);
            OnBeat?.Invoke(type);
        }
        //GD.Print(BeatProgress);
        lastFramePos = pos;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (singleton == this)
        {
            OnBeat = null;// destroy all listeners
            singleton = null;
        }
    }
}
