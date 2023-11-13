using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteMaskMaster : Sprite2D
{
    public static SpriteMaskMaster Singleton { get; private set; }

    private SubViewport view = null;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (Singleton != null) Singleton.QueueFree();
        Singleton = this;
        view = new SubViewport();
        view.Size = new Vector2I((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);
        view.RenderTargetClearMode = SubViewport.ClearMode.Always;
        view.TransparentBg = true;
        Texture = view.GetTexture();
        AddChild(view);
        GetViewport().SizeChanged += Viewport_SizeChanged;

        RenderingServer.FramePostDraw += RenderingServer_FramePostDraw;
    }

    private void RenderingServer_FramePostDraw()
    {
        //view.GetTexture().GetImage().SavePng("C:/Users/tommy/Desktop/testimg.png");
        //GD.Print(Texture.GetImage().GetSize());
        //DrawTexture(, /*GetViewport().GetCamera2D().GetScreenCenterPosition()*/ Vector2.Zero);
    }

    private void Viewport_SizeChanged()
    {
        Vector2 newSize = GetViewportRect().Size;
        view.Size = new Vector2I((int)newSize.X, (int)newSize.Y);
        QueueRedraw();
    }

    public static void AddMask(SpriteMask mask)
    {
        Singleton.view.AddChild(mask);
        mask.GlobalPosition += Singleton.GetViewportRect().Size * 0.5f;
        mask.QueueRedraw();
        Singleton.QueueRedraw();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        this.QueueRedraw();
    }
}
