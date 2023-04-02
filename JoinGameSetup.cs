using Godot;
using System;

public partial class JoinGameSetup : VBoxContainer
{
	[Export] public LineEdit PlayerName { get; set; }
	[Export] public LineEdit IPPortToJoin { get; set; }
	[Export] public Button JoinButton { get; set; }

	public void PlayerConnected(long id)
	{
		Globals.SinglePlayer = false;
		var error = GetTree().ChangeSceneToFile("res://Pong Game.tscn");
		GD.PrintErr(error);
	}

	public override void _Ready()
	{

		PlayerName.Text = System.Environment.UserName;
		JoinButton.Disabled = false;
		JoinButton.Text = "Join";

		Multiplayer.PeerConnected += PlayerConnected;
	}

	public override void _Process(double delta)
	{
	}

	private void _on_join_button_pressed()
	{
		JoinButton.Disabled = true;
		JoinButton.Text = "Joining...";

		// TODO add error handling
		var splitIpPort = IPPortToJoin.Text.Split(":");
		string ip = splitIpPort[0], port = splitIpPort[1];

		var peer = new ENetMultiplayerPeer();
		peer.CreateClient(ip, port.ToInt());
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;

		
	}
}



