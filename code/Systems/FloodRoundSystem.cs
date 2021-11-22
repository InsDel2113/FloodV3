using Sandbox;
using Sandbox.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Flood
{
	public partial class FloodRoundSystem : BaseNetworkable
	{
		public enum Round
		{
			PreGame = 0,
			Fight = 1,
			PostGame = 2
		}

		[Net]
		public Round CurrentRound { get; set; } = Round.PreGame;

		[Net]
		// Current round time. How far are we in the current round?
		public float CurrentRoundTime { get; set; } = 0f;

		// Round lengths. How long are the rounds?
		public float PreGameLength = 5f;
		public float FightLength = 5f;
		public float PostGameLength = 5f;

		// Run this each tick. Ticks down timers, checks round stuff, etc
		public void Tick()
		{
			// We aren't in fight round?
			if ( CurrentRound != Round.Fight )
			{
				// God everybody
				foreach ( var Player in Client.All )
				{
					Player.Pawn.Health = 100;
				}
			}
		}

		public bool CheckRoundOver()
		{
			switch ( CurrentRound )
			{
				case Round.PreGame:
					if ( CurrentRoundTime > PreGameLength )
					{
						CurrentRound = Round.Fight;
						CurrentRoundTime = 0;
						return true;
					}
					break;
				case Round.Fight:
					if ( CurrentRoundTime > FightLength )
					{
						CurrentRound = Round.PostGame;
						CurrentRoundTime = 0;
						return true;
					}
					break;
				case Round.PostGame:
					if ( CurrentRoundTime > PostGameLength )
					{
						CurrentRound = Round.PreGame;
						CurrentRoundTime = 0;
						return true;
					}
					break;
			}
			return false;
		}

		public void SecondTick()
		{
			Log.Info( CurrentRound );
			CurrentRoundTime += 1;
			// This changes us to new rounds and resets the timer
			CheckRoundOver();
		}
	}
}
