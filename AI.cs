using Godot;
using System;

public partial class AI : CharacterBody2D
{
	[Export]
	public float AISpeed = 2000f;


	private Polygon2D BallPolygon2D;
	private CharacterBody2D Ball;

	public override void _Ready()
	{
		BallPolygon2D = GetNode<Polygon2D>("/root/Pong Game/Ball/Polygon2D");
		Ball = GetNode<CharacterBody2D>("/root/Pong Game/Ball");
	}

	private Vector2 GetMoveDirection()
	{
		float ballPaddleXDist = Position.X - Ball.Position.X;
		bool ballPositiveVelocity = (Ball as Ball).Velocity.X > 0;
		if (Mathf.Abs(Ball.Position.Y - Position.Y) > 15 && ballPaddleXDist < 600)
		{
			return (Ball.Position.Y < Position.Y) ? 
				Vector2.Up   * AISpeed * ((ballPositiveVelocity ? 1.0f : 0.5f)) : 
				Vector2.Down * AISpeed * ((ballPositiveVelocity ? 1.0f : 0.5f));
		}
		return Vector2.Zero;
	}

	

	public override void _Process(double delta)
	{
		Velocity = GetMoveDirection();
		MoveAndSlide();
		Velocity = Vector2.Zero;
	}
}
