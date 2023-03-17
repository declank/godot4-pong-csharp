using Godot;
using System;

public partial class Game : Node2D
{
	public int Player1Score = 0;
	public int Player2Score = 0;

	private CharacterBody2D Ball;
	private RichTextLabel P1Label, P2Label;
	private AudioStreamPlayer2D P1DeathAudio, P2DeathAudio;

	public override void _Ready()
	{
		Ball = GetNode<CharacterBody2D>("Ball");
		P1Label = GetNode<RichTextLabel>("Player 1 Score");
		P2Label = GetNode<RichTextLabel>("Player 2 Score");
		P1DeathAudio = GetNode<AudioStreamPlayer2D>("Player Paddle/Death Audio");
		P2DeathAudio = GetNode<AudioStreamPlayer2D>("AI Paddle/Death Audio");
	}

	private void _on_left_boundary_body_entered(Node2D body)
	{
		if (body is Ball)
		{
			(Ball as Ball).ResetPosAndVelocity();

			Player2Score += 1;
			P2Label.Text = "[center]" + Player2Score.ToString() + "[/center]";

			P1DeathAudio.Play();
		}
	}

	private void _on_right_boundary_body_entered(Node2D body)
	{
		if (body is Ball)
		{
			(Ball as Ball).ResetPosAndVelocity();

			Player1Score += 1;
			P1Label.Text = "[center]" + Player1Score.ToString() + "[/center]";

			P2DeathAudio.Play();
		}
	}
}








