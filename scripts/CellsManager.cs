using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CellsManager : Node2D
{
    public static CellsManager instance;
    public static bool lost = false;
    public static bool won = false;

    public const string STAT_HEALTHY_CELLS_ATE = "HEALTHY_CELLS_ATE";
    public const string STAT_CANCER_CELLS_ATE = "CANCER_CELLS_ATE";

    [Export]
    public float badInterval = 10;
    [Export]
    public int loseBadCount = 10;

    private float badTimer = 10;

	private HashSet<Cell> cells = new HashSet<Cell>();
	private int totalAliveCells = 0;
	private int totalCancerCells = 0;

    public override void _Ready()
    {
        base._Ready();
        instance = this;
        lost = won = false;
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
        CellLevelUIManager.instance.SetCancerCount(totalCancerCells, Mathf.CeilToInt((totalCancerCells * 100f) / loseBadCount));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (lost || won) return;
        badTimer -= (float)delta;
        CellLevelUIManager.instance.SetCancerCountdown((int)(100 - badTimer * 100f / badInterval));
        if (badTimer <= 0)
        {
            // turn a good cell into a bad cell
            Node ndParent = GetChild(0);
            Node batch = ndParent.GetChild((int)(GD.Randi() % (uint)ndParent.GetChildCount()));
            Cell cell = batch.GetChild<Cell>((int)(GD.Randi() % (uint)batch.GetChildCount()));
            if (!cell.good) return;
            totalCancerCells++;

            CellLevelUIManager.instance.SetCancerCount(totalCancerCells, Mathf.CeilToInt((totalCancerCells * 100f) / loseBadCount));
            cell.SetGood(false);
            badTimer = badInterval;
            if (totalCancerCells >= loseBadCount)
            {
                CellLevelUIManager.instance.SetDeathUIVisible(true);
                lost = true;
                won = false;
            }
        }
    }

    public void CellDie(Cell cell)
    {
        if (lost || won) return;

        if (!cell.good)
        {
            totalCancerCells--;
            GameManager.ChangeStat(STAT_CANCER_CELLS_ATE, 1);
            CellLevelUIManager.instance.SetCancerCount(totalCancerCells, Mathf.CeilToInt((totalCancerCells * 100f) / loseBadCount));
            if (totalCancerCells <= 0)
            {
                CellsManager.won = true;
                GameManager.PlayLevel("frog", "Frog");
            }
        }
        else
        {
            GameManager.ChangeStat(STAT_HEALTHY_CELLS_ATE, 1);
        }
    }

	public override void _ExitTree()
	{
		base._ExitTree();
		instance = null;
	}
}
