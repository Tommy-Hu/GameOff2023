using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CellsManager : Node2D
{
    public static CellsManager instance;

    [Export]
    public float badInterval = 10;
    private float badTimer = 10;

    private HashSet<Cell> cells = new HashSet<Cell>();
    private int totalAliveCells = 0;
    private int totalCancerCells = 0;

    public override void _Ready()
    {
        base._Ready();
        instance = this;
        void Recurse(Node2D nd)
        {
            for (int i = 0; i < nd.GetChildCount(); i++)
            {
                if (nd is Cell cell)
                    cells.Add(cell);
                Recurse(nd.GetChild(i) as Node2D);
            }
        }
        Recurse(this);
        totalAliveCells = cells.Count;
        totalCancerCells = cells.Aggregate(0, (a, c) => a + (c.good ? 0 : 1));
        CellLevelUIManager.instance.SetCancerCount(totalCancerCells);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        badTimer -= (float)delta;
        CellLevelUIManager.instance.SetCancerCountdown(badTimer);
        if (badTimer <= 0)
        {
            // turn a good cell into a bad cell
            Node ndParent = GetChild(0);
            Node batch = ndParent.GetChild((int)(GD.Randi() % (uint)ndParent.GetChildCount()));
            Cell cell = batch.GetChild<Cell>((int)(GD.Randi() % (uint)batch.GetChildCount()));
            if (!cell.good) return;
            totalCancerCells++;
            CellLevelUIManager.instance.SetCancerCount(totalCancerCells);
            cell.SetGood(false);
            badTimer = badInterval;
        }
    }

    public void CellDie(Cell cell)
    {
        if (!cell.good)
        {
            totalCancerCells--;
            CellLevelUIManager.instance.SetCancerCount(totalCancerCells);
            if (totalCancerCells <= 0)
            {
                GameManager.PlayLevel("frog", "Frog");
            }
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        instance = null;
    }
}
