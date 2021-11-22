
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Flood
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class FloodGame : Sandbox.Game
	{
		public FloodRoundSystem RoundSystem;
		public FloodWaterSystem WaterSystem;
		public FloodGame()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new FloodHud();
				// Init systems
				RoundSystem = new FloodRoundSystem();
				WaterSystem = new FloodWaterSystem();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}

		public static FloodGame Instance
		{
			get => Current as FloodGame;
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new FloodPlayer();
			client.Pawn = player;

			player.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( IsServer )
			{
				RoundSystem.Tick();
				WaterSystem.Tick();
			}
		}

		public override void PostLevelLoaded()
		{
			base.PostLevelLoaded();
			_ = StartSecondTimer();
		}
		public async Task StartSecondTimer()
		{
			while ( true )
			{
				await Task.DelaySeconds( 1 );
				RoundSystem.SecondTick();
				WaterSystem.SecondTick();
			}
		}

	}

}
