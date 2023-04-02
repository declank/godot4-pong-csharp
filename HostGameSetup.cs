using Godot;
using System;
using System.Text;
using System.Threading;

public partial class HostGameSetup : VBoxContainer
{
	private int ServerPort = 4567;
	private bool UPnpRan = false;
	private Thread UPnpThread;
	private ProgressBar ProgressBar;

	private Control WaitingOnUPnpScreen, HostGameDetailsScreen;
	private LineEdit IPAddressDisplay, PlayerNameLineEdit;
	private HttpRequest PublicIPHttpRequest;

	private String PublicIP;

	public void PlayerConnected(long id)
	{
		GD.Print("SOMEONE CONNECTED AAAAAH " + id);
		Globals.SinglePlayer = false;
		var error = GetTree().ChangeSceneToFile("res://Pong Game.tscn");
		GD.PrintErr(error);
	}

	public override void _Ready()
	{
		WaitingOnUPnpScreen = GetNode<Control>("Waiting On UPnP");
		HostGameDetailsScreen = GetNode<Control>("Host Game Details");
		ProgressBar = GetNode<ProgressBar>("Waiting On UPnP/ProgressBar");
		VisibilityChanged += () => UPnpSetup();

		WaitingOnUPnpScreen.Visible = true;
		HostGameDetailsScreen.Visible = false;

		PlayerNameLineEdit = GetNode<LineEdit>("Host Game Details/Player Name");
		IPAddressDisplay = GetNode<LineEdit>("Host Game Details/IP Address Display");
		PublicIPHttpRequest = GetNode<HttpRequest>("Host Game Details/HTTPRequest");
		PublicIPHttpRequest.RequestCompleted += OnRequestCompleted;

		Multiplayer.PeerConnected += PlayerConnected;
	}

	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode >= 200 && responseCode < 400)
		{
			PublicIP = Encoding.UTF8.GetString(body);
		}
		else
		{
			PublicIP = "0";
		}

		IPAddressDisplay.Text = PublicIP + ":" + ServerPort;
	}

	private void UPnpSetupThreadFunc()
	{
		ProgressBar.Value = 30;

		var upnp = new Upnp();
		var error = upnp.Discover();

		if (error != (int)Error.Ok)
		{
			GD.PrintErr("Could not set up UPnP");
			return;
		}

		if (upnp.GetGateway().IsValidGateway())
		{
			var status = upnp.AddPortMapping(ServerPort);

			GD.Print("Port:   " + ServerPort);
			GD.Print("Status: " + status);

			ProgressBar.Value = 100;

		}
	}

	private void UPnpSetup()
	{
		UPnpThread = new Thread(new ThreadStart(UPnpSetupThreadFunc)); // TODO does this need to be cleaned up?
		UPnpThread.Start();
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (UPnpThread is not null && !UPnpThread.IsAlive && WaitingOnUPnpScreen.Visible)
		{
			WaitingOnUPnpScreen.Visible = false;
			HostGameDetailsScreen.Visible = true;

			PlayerNameLineEdit.Text = System.Environment.UserName;

			PublicIPHttpRequest.Request("https://ifconfig.me", new string[] { "user-agent: curl" });

			var peer = new ENetMultiplayerPeer();
			var error = peer.CreateServer(ServerPort, 1);
			// TODO handle error
			peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
			Multiplayer.MultiplayerPeer = peer;

			// Waiting for player
		}
	}

	public override void _ExitTree()
	{
		if (UPnpThread != null)
			UPnpThread.Join();
	}
}
