using Godot;
using System;

public partial class FPSCounter : RichTextLabel
{
	public override void _Process(double delta)
	{
		Text = Engine.GetFramesPerSecond() + " FPS";
	}
}
