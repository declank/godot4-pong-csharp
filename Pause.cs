using Godot;
using System;

public partial class Pause : Panel
{
	private Button InitialFocus;

	public override void _Ready()
	{
		Visible = false;
		InitialFocus = GetNode<Button>("VBoxContainer/Continue");

		var returnToMainMenuButton = GetNode<MenuButton>("VBoxContainer/Return To Main Menu");
		var returnToMainPopupMenu = returnToMainMenuButton.GetPopup();
		returnToMainPopupMenu.IndexPressed += (index) => OnReturnMMPopupMenu(index);

		var quitGameMenuButton = GetNode<MenuButton>("VBoxContainer/Quit Game");
		var quitGamePopupMenu = quitGameMenuButton.GetPopup();
		quitGamePopupMenu.IndexPressed += (index) => OnQuitPopupMenu(index);
	}

	private void OnReturnMMPopupMenu(long index)
	{
		switch (index)
		{
			case 0:
				GetTree().ChangeSceneToFile("res://Main Menu.tscn");
				GetTree().Paused = false;
				break;
			case 1:
				// Continue game
				// BUG Unpausing using the popup signifcantly increases ball velocity?
				UnpauseGame();
				break;
		}
	}

	private void OnQuitPopupMenu(long index)
	{
		switch (index)
		{
			case 0:
				GetTree().Quit();
				break;
			case 1:
				// Continue game
				UnpauseGame();
				break;
		}
	}

	private void UnpauseGame()
	{
		Hide();
		Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;
		GetTree().Paused = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (Globals.SinglePlayer && Input.IsActionJustPressed("pause"))
		{
			if (GetTree().Paused == false)
			{
				GetTree().Paused = true;
				Show();
				InitialFocus.GrabFocus();
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			else
			{
				UnpauseGame();
			}
		}
	}

	private void _on_continue_pressed()
	{
		UnpauseGame();
	}

	private void _on_quit_game_about_to_popup()
	{
		// Replace with function body.
	}

}
