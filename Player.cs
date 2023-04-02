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
		GD.Print("Player _Ready");
		var screenSize = GetViewportRect().Size;
		HalfPaddleHeight = PaddleHeight / 2.0f;
		MaxY = screenSize.Y - PaddleHeight;// - HalfPaddleHeight;
	}

	[Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	private void SetPosition(Vector2 newPosition)
	{
		Position = newPosition;
	}

	public override void _Input(InputEvent @event)
	{
		//GD.Print("Player _Input");
		//GD.Print(@event.ToString());
		if (IsMultiplayerAuthority() && @event is InputEventMouseMotion mouseMotion)
		{
			//GD.Print("mouseMotion.Position.Y: " + mouseMotion.Position.Y);
			//GD.Print("HalfPaddleHeight: " + HalfPaddleHeight);
			//GD.Print("MaxY: " + MaxY);
			Vector2 newPosition = new Vector2(
				x: Position.X,
				y: Mathf.Clamp(mouseMotion.Position.Y - HalfPaddleHeight, 0, MaxY)
			);
			//SetPosition(newPosition);
			Rpc("SetPosition", newPosition);
		}
		
	}
}



