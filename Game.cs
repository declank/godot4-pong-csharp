using Godot;
using System;

public partial class Game : Node2D
{
	public int Player1Score = 0;
	public int Player2Score = 0;

	[Export] public CharacterBody2D Ball { get; set; }
	[Export] public RichTextLabel P1Label { get; set; }
	[Export] public RichTextLabel P2Label { get; set; }
	[Export] public AudioStreamPlayer2D P1DeathAudio { get; set; }
	[Export] public AudioStreamPlayer2D P2DeathAudio { get; set; }
	[Export] public CharacterBody2D P1Paddle { get; set; }
	[Export] public CharacterBody2D P2Paddle { get; set; }
	[Export] public AudioStreamPlayer2D CountdownSound { get; set; }
	
	private float InitialP1X, InitialP2X;

	private bool P1Updated, P2Updated;

	public override void _Ready()
	{
		InitialP1X = P1Paddle.Position.X;
		InitialP2X = P2Paddle.Position.X;

		Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;

		if (Globals.SinglePlayer)
		{
			P1Paddle = AttachPlayerScript(P1Paddle); // NB: You cannot pass property by ref so return value needed as original object disposed
			P2Paddle = AttachAIScript(P2Paddle);
		}
		else
		{
			P1Paddle = AttachPlayerScript(P1Paddle);
			P2Paddle = AttachPlayerScript(P2Paddle);
			if (Multiplayer.IsServer())
			{
				P2Paddle.SetMultiplayerAuthority(Multiplayer.GetPeers()[0]);
			}
			else
			{
				P2Paddle.SetMultiplayerAuthority(Multiplayer.GetUniqueId());
			}

			// Pause game until audio countdown timer is done
			GetTree().Paused = true;
			CountdownSound.Play();
		}


	}

	private void _on_countdown_timer_timeout()
	{
		if (!Globals.SinglePlayer)
		{
			GetTree().Paused = false;
		}
	}

	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void UpdateP1Paddle(CharacterBody2D paddle)
	{
		//P1Paddle.id
		P1Paddle = paddle;
		P1Updated = true;
		GD.Print("P1 Paddle Updated");
	}

	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void UpdateP2Paddle(CharacterBody2D paddle)
	{
		P2Paddle = paddle;
		P2Updated = true;
		GD.Print("P2 Paddle Updated");
	}

	private CharacterBody2D AttachPlayerScript(CharacterBody2D paddle)
	{
		var id = paddle.GetInstanceId();
		paddle.SetScript(ResourceLoader.Load("res://Player.cs"));
		paddle = InstanceFromId(id) as CharacterBody2D;
		paddle.SetProcessInput(true);
		paddle._Ready();
		return paddle;
	}

	private CharacterBody2D AttachAIScript(CharacterBody2D paddle)
	{
		var id = paddle.GetInstanceId();
		paddle.SetScript(ResourceLoader.Load("res://AI.cs"));
		paddle = InstanceFromId(id) as CharacterBody2D;
		paddle.SetProcess(true);
		paddle._Ready();
		return paddle;
	}

	
	private void ResetPaddleX()
	{
		P1Paddle.Position = new()
		{
			X = InitialP1X,
			Y = P1Paddle.Position.Y
		};

		P2Paddle.Position = new()
		{
			X = InitialP2X,
			Y = P2Paddle.Position.Y
		};
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void LeftBoundryRPC()
	{
		(Ball as Ball).ResetPosAndVelocity();

		Player2Score += 1;
		P2Label.Text = "[center]" + Player2Score.ToString() + "[/center]";

		P1DeathAudio.Play();

		// BUG some strange bug where the edge of paddle is hit and moves X position - this workaround may fix
		ResetPaddleX();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void RightBoundryRPC()
	{
		(Ball as Ball).ResetPosAndVelocity();

		Player1Score += 1;
		P1Label.Text = "[center]" + Player1Score.ToString() + "[/center]";

		P2DeathAudio.Play();

		// BUG some strange bug where the edge of paddle is hit and moves X position - this workaround may fix
		ResetPaddleX();
	}

	private void _on_left_boundary_body_entered(Node2D body)
	{
		//GD.Print("Authority: " + IsMultiplayerAuthority());
		if (body is Ball && IsMultiplayerAuthority())
		{
			Rpc("LeftBoundryRPC");
		}
	}

	
	private void _on_right_boundary_body_entered(Node2D body)
	{
		//GD.Print("Authority: " + IsMultiplayerAuthority());
		if (body is Ball && !IsMultiplayerAuthority())
		{
			Rpc("RightBoundryRPC");
		}
	}
}











