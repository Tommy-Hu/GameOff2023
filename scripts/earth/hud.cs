using Godot;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;

public partial class hud : Control
{
	PackedScene plLifeIcon = (PackedScene)GD.Load("res://scenes/earth/HUD/life_icon.tscn");
	HBoxContainer lifeContainer = null;
	public override void _Ready()
	{
		HBoxContainer lifeContainer = GetNode<HBoxContainer>("LifeContainer");
		clearLives();
		setLives(3);
	}

	public void clearLives()
	{
		foreach (var c in lifeContainer.GetChildren())
		{
			lifeContainer.RemoveChild(c);
		}
	}
	public void setLives(int lives)
	{
		clearLives();
		foreach (int i in Enumerable.Range(0, lives))
		{
			lifeContainer.AddChild(plLifeIcon.Instantiate<TextureRect>());
		}
			
	}

	// public void clearOne()
	// {
	// 	if 
	// 	lifeContainer.GetChildren()
	// 	lifeContainer.RemoveChild();
	
	// }
	
}
