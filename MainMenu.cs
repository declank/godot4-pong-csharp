using Godot;
using System;

public partial class MainMenu : Control
{
	private VBoxContainer HostGameScreen;
	private VBoxContainer JoinGameScreen;
	private VBoxContainer MainMenuVBox;
	
	public override void _Ready()
	{
		var hostJoinPopup = GetNode<MenuButton>("Main Menu/Multiplayer").GetPopup();
		hostJoinPopup.IndexPressed += (index) => OnHostJoinPopupPressed(index);
		HostGameScreen = GetNode<VBoxContainer>("Host Game UI");
		JoinGameScreen = GetNode<VBoxContainer>("Join Game UI");
		MainMenuVBox = GetNode<VBoxContainer>("Main Menu");

		MainMenuVBox.Visible = true; // Do I need to hide the others for sake of consistency
		HostGameScreen.Visible = false;
		JoinGameScreen.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnHostJoinPopupPressed(long index)
	{
		switch (index)
		{
			case 0:
				// Host game
				MainMenuVBox.Visible = false;
				HostGameScreen.Visible = true;
				break;
			case 1:
				// Join game
				MainMenuVBox.Visible = false;
				JoinGameScreen.Visible = true;
				break;
		}
	}

	private void _on_single_player_pressed()
	{
		Globals.SinglePlayer = true;
		var error = GetTree().ChangeSceneToFile("res://Pong Game.tscn");
		GD.PrintErr(error);
	}

	private void _on_quit_game_pressed()
	{
		GetTree().Quit();
	}
}

