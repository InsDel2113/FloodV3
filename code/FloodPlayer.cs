using Sandbox;
using System;
using System.Linq;

namespace Flood
{
	partial class FloodPlayer : Player
	{
		private DamageInfo lastDamage;
		public FloodPlayer()
		{
			Inventory = new Inventory( this );
		}
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

            // Controller - movement controller code.
			Controller = new WalkController();

            // You don't need to change this.
			Animator = new StandardPlayerAnimator();

            // Camera - this is what the class/code the camera uses by default
			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Inventory.Add( new PhysGun(), true );
			Inventory.Add( new GravGun() );
			Inventory.Add( new Tool() );

			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
			Controller = null;

			EnableAllCollisions = false;
			EnableDrawing = false;

			Inventory.DropActive();
			Inventory.DeleteContents();
		}


		public override void TakeDamage( DamageInfo info )
		{
			//info.Damage = 0;

			lastDamage = info;

			base.TakeDamage( info );
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );
		}
	}
}
