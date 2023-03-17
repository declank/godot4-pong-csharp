using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed = 400;
	[Export]
	public int PaddleHeight = 102;

	public float MaxY, HalfPaddleHeight;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var screenSize = GetViewportRect().Size;
		HalfPaddleHeight = PaddleHeight / 2.0f;
		MaxY = screenSize.Y - PaddleHeight;// - HalfPaddleHeight;
		
	}

	public override void _Process(double delta)
	{
		/*var velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("move_up")) 
		{
			velocity.Y -= 200;
		}
		if (Input.IsActionPressed("move_down")) 
		{
			velocity.Y += 200;
		}
		
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Position.X,
			y: Mathf.Clamp(Position.Y, 0, MaxY)
		);*/
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		Position = new Vector2(
			x: Position.X,
			y: Mathf.Clamp(mouseMotion.Position.Y - HalfPaddleHeight, 0, MaxY)
		);
	}
}



