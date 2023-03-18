using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export]
	public int InitialSpeed = 500;
	[Export]
	public float SpeedIncreasePerPaddleHit = 1.09f;
	[Export]
	public CollisionShape2D Top;
	[Export]
	public CollisionShape2D Bottom;
	
	private AudioStreamPlayer2D AudioPlayer;

	public override void _Ready()
	{
		GD.Randomize(); // Move this out if we have a scene script
		ResetPosAndVelocity();

		AudioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
	}

	public void ResetPosAndVelocity()
	{
		Position = new Vector2(570, 324);
		Velocity = new()
		{
			X = (GD.Randi() % 2) * 2.0f - 1.0f,
			Y = (GD.Randi() % 2) * 2.0f - 1.0f
		};
	}

	public override void _Process(double delta)
	{
		var collisionObject = MoveAndCollide(Velocity * InitialSpeed * (float)delta);
		if (collisionObject is not null)
		{
			Velocity = Velocity.Bounce(collisionObject.GetNormal());
		}
		if (collisionObject?.GetCollider() is CharacterBody2D)
		{
			// If ball hits a player or AI paddle, play sound and increase speed
			Velocity *= SpeedIncreasePerPaddleHit;
			AudioPlayer.Play();
		}	
	}
}
