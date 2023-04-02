using Godot;
using System;

public partial class AI : CharacterBody2D
{
	[Export]
	public float AISpeed = 800f;

	private Polygon2D BallPolygon2D;
	private CharacterBody2D Ball;

	public override void _Ready()
	{
		BallPolygon2D = GetNode<Polygon2D>("/root/Pong Game/Ball/Polygon2D");
		Ball = GetNode<CharacterBody2D>("/root/Pong Game/Ball");
	}

	private Vector2 GetMoveDirection()
	{
		//float ballPaddleXDist = Position.X - Ball.Position.X;
		float centrePaddleY = Position.Y + 51;

		if (Mathf.Abs(Ball.Position.Y - centrePaddleY) > 40)
		{
			bool ballPositiveVelocity = (Ball as Ball).Velocity.X > 0;
			return (Ball.Position.Y < centrePaddleY) ? 
				Vector2.Up   * AISpeed * ((ballPositiveVelocity ? 1.0f : 0.5f)) : 
				Vector2.Down * AISpeed * ((ballPositiveVelocity ? 1.0f : 0.5f));
		}
		return Vector2.Zero;
	}

	public override void _Process(double delta)
	{
		Velocity = GetMoveDirection();
		MoveAndSlide();
		//Velocity = Vector2.Zero;
	}
}
