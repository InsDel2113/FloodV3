using Sandbox;
using Sandbox.UI;

[Library]
public partial class FloodHud : HudEntity<RootPanel>
{
	public FloodHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet.Load( "/HUD/FloodHud.scss" );

		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<CrosshairCanvas>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		RootPanel.AddChild<Health>();
		RootPanel.AddChild<InventoryBar>();
		RootPanel.AddChild<CurrentTool>();
		RootPanel.AddChild<SpawnMenu>();
	}
}
