using Sandbox;
using Sandbox.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Flood.FloodRoundSystem;

namespace Flood
{
	public partial class FloodWaterSystem : BaseNetworkable
	{
		[Net]
		public Entity Water { get; set; }
		public Vector3 WaterBasePosition;
		public float WaterHeight = 375f; // how high the water goes up to
		public float WaterSpeed = 1f; // how fast the water rises up
		[Net]
		public List<Entity> HurtList { get; set; } = new List<Entity>();
		public float WaterDamage = 20f; // How much damage does the water do?

		// Run this each tick. Ticks down timers, checks round stuff, etc
		public void Tick()
		{
			if ( Water == null )
			{
				foreach ( var Entity in Entity.All )
				{
					if ( Entity.ClassInfo.Name == "env_sea" )
					{
						Log.Info( "Water found!" );
						Water = Entity;
						WaterBasePosition = Water.Position;
					}
				}
			}
			// if the water is still null after that...
			if ( Water == null )
			{
				Log.Info( "Water not found... Manually created - map not officially supported." );
				// Make it ourself!
				var water = new WaterSea();
				float lowestPoint = 0f;
				foreach ( var entity in Entity.All ) // find lowest entity
					if ( entity.Position.z < lowestPoint )
						lowestPoint = entity.Position.z;
				if ( lowestPoint < WaterHeight / 2 ) // don't want it too low now
					lowestPoint += WaterHeight / 2;
			}
			switch ( FloodGame.Instance.RoundSystem.CurrentRound )
			{
				case Round.PreGame:
					HideWater();
					break;
				case Round.Fight:
					RiseWater();
					break;
				case Round.PostGame:
					HideWater();
					break;
			}
		}

		public void SecondTick()
		{
			foreach ( var Player in HurtList )
			{
				DamageInfo info = new DamageInfo();
				info.Damage = WaterDamage;
				Player.TakeDamage( info );
			}
		}

		// Hurt list update
		public void WaterListUpdate(Entity Player, bool Add)
		{
			Log.Info( "Water list update: " + Player.Name );
			if ( Add )
				HurtList.Add( Player );
			else
				HurtList.Remove( Player );
		}

		public void RiseWater()
		{
			if ( Water.Position.z < WaterHeight )
				Water.Position += Vector3.Up * WaterSpeed;
		}
		public void HideWater()
		{
			if ( WaterBasePosition.z < Water.Position.z )
				Water.Position -= Vector3.Up * WaterSpeed;
		}
	}
}
