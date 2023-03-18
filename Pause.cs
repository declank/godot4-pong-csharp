using Godot;
using System;

public partial class Pause : Panel
{
	private Button InitialFocus;

	public override void _Ready()
	{
		Visible = false;
		InitialFocus = GetNode<Button>("VBoxContainer/Continue");
		var quitGameMenuButton = GetNode<MenuButton>("VBoxContainer/Quit Game");
		var quitGamePopupMenu = quitGameMenuButton.GetPopup();
		quitGamePopupMenu.IndexPressed += (index) => OnQuitPopupMenu(index);

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
		if (Input.IsActionJustPressed("pause"))
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
