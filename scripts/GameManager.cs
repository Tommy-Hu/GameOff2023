using Godot;
using System;
using System.Collections.Generic;

[Flags]
public enum BeatType
{
    None = 0,
    /// <summary>
    /// The super loud beat every phrase. (every two measures)
    /// </summary>
    PhraseBeat = 1,
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

    public const float ZOOM_TIME = 2f;
    /// <summary>
    /// Fade in or fade out time. NOT COMBINED TIME.
    /// </summary>
    public const float FADE_TIME = 1f;

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

    private static HashSet<AudioStreamPlayer2D> aliveSFX = new HashSet<AudioStreamPlayer2D>();

    private bool isLoadingNext = false;

    [Export]
    public NodePath faderPath;

    private TextureProgressBar fader;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (singleton != null)
        {
            singleton.QueueFree();
        }
        singleton = this;
        musicPlayer = GetParent().GetChild<AudioStreamPlayer2D>(1);
        aliveSFX.Clear();
    }

    public override void _Ready()
    {
        base._Ready();
        fader = GetNode<TextureProgressBar>(faderPath);
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
        aliveSFX.Clear();
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
        if (!singleton.isLoadingNext)
        {
            singleton.isLoadingNext = true;

            Camera2D cam = singleton.GetViewport().GetCamera2D();
            if (cam is not null)
            {
                Tween tween = singleton.CreateTween();
                tween.SetTrans(Tween.TransitionType.Quad);
                tween.TweenProperty(cam, "zoom", Vector2.One * 0.005f, ZOOM_TIME);
                tween.TweenCallback(Callable.From(() =>
                {
                    PlayLevelImmediate(levelName, musicName);
                }));
            }
            else
            {
                PlayLevelImmediate(levelName, musicName);
            }
        }
    }

    /// <summary>
    /// Fade out and back into `levelName`.
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="musicName"></param>
    public static void PlayLevelFade(string levelName, string musicName)
    {
        Tween tween = singleton.CreateTween();
        tween.SetTrans(Tween.TransitionType.Circ);
        tween.TweenProperty(singleton.fader, "value", 100, FADE_TIME);
        tween.TweenCallback(Callable.From(() =>
        {
            PlayLevelImmediate(levelName, musicName);
        }));
        tween.TweenProperty(singleton.fader, "value", 0, FADE_TIME);
    }

    /// <summary>
    /// DONT USE THIS IF U WANT TRANSITION.
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="musicName"></param>
    public static void PlayLevelImmediate(string levelName, string musicName)
    {
        singleton.DestroyCurrentLevel();
        singleton.LoadNextLevel($"res://scenes/level_{levelName}.tscn");
        singleton.PlayMusic(musicName);
        singleton.isLoadingNext = false;
    }

    /// <summary>
    /// This is the same as <see cref="PlaySFX(string)"/>, but will play a random sound in the array sfxChoices. To use this function,
    /// simply call this with comma separated string. For example, if you want to play a random footstep sound from 3 different sound files,
    /// you can call GameManager.PlayRandomSFX("footstep1.ogg", "footstep2.wav", "footstep3.mp3"); Note that file formats don't matter.
    /// </summary>
    /// <param name="sfxChoices"></param>
    public static void PlayRandomSFX(params string[] sfxChoices)
    {
        PlaySFX(sfxChoices[GD.Randi() % (uint)sfxChoices.Length]);
    }

    /// <summary>
    /// Plays the given sfx. For example, if the sfx == "shoot.wav", this function would play the audio file res://sfx/shoot.wav
    /// </summary>
    /// <param name="sfx"></param>
    public static void PlaySFX(string sfx)
    {
        AudioStreamPlayer2D audio = new AudioStreamPlayer2D();
        singleton.GetParent().AddChild(audio);
        audio.Stream = ResourceLoader.Load<AudioStream>(System.IO.Path.Combine("res://sfx/", sfx));
        audio.Finished += () =>
        {
            aliveSFX.Remove(audio);
            audio.QueueFree();
        };
        audio.Play();
        aliveSFX.Add(audio);
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
            //GD.Print(nextBeat);
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
