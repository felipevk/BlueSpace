using System;
using System.Collections.Generic;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public enum PlayerWeaponType
	{
		Laser = 0
	}
	public class PlayerWeaponComponentData : ComponentData
	{
		public PlayerWeaponType type = PlayerWeaponType.Laser;
		public float currentRechargeTime = 0;
		public String input = "fire1";
		public int playerIndex = 0;
		public bool drawDebugProjectile = false;
	}

	public struct WeaponData
	{
		public int damage;
		public float rechargeTime;
		public int projectiles;
		public float projectileSpeed;
		public String projectileSprite;
		public String projectileHitSprite;
		public String shootSound;
		public String hitSound;
		public float projectileHitLifetime;
		public float shootSoundVolume;
		public float hitSoundVolume;
		public List<Vector3> positionOffset;
		public List<Quaternion> rotations;
	}

	public class PlayerWeaponComponentSystem : ComponentSystem
	{
		static List<WeaponData> weaponDataByType = new List<WeaponData>();
		public override void Start()
		{
			base.Start();
			WeaponData laser = new WeaponData();
			laser.damage = 5;
			laser.projectiles = 1;
			laser.rechargeTime = 0.1f;
			laser.projectileSpeed = 1000;
			laser.projectileSprite = "laserRed";
			laser.projectileHitSprite = "laserRedShot";
			laser.shootSound = "laserShoot";
			laser.hitSound = "";
			laser.shootSoundVolume = 0.2f;
			laser.hitSoundVolume = 0.2f;
			laser.positionOffset = new List<Vector3>();
			laser.positionOffset.Add( new Vector3( 0, -30, 0 ) );
			laser.rotations = new List<Quaternion>();
			laser.rotations.Add( Quaternion.Identity );

			weaponDataByType.Add( laser );
		}

		protected override void Update( String gameObjectId, ComponentData data )
		{
			PlayerWeaponComponentData playerWeaponData = data as PlayerWeaponComponentData;

			if ( playerWeaponData.currentRechargeTime > 0f )
			{
				playerWeaponData.currentRechargeTime -= Time.DeltaTime;
			}
			else
			{
				if ( Input.IsButtonPressed( playerWeaponData.input, playerWeaponData.playerIndex ) )
				{
					Vector3 gunPos = GetGameObject( gameObjectId ).GetGlobalPosition();
					Shoot( gameObjectId, gunPos, playerWeaponData );
				}
			}
		}

		private void Shoot( String gameObjectId, Vector3 gunPos, PlayerWeaponComponentData playerWeaponData )
		{
			WeaponData weaponData = weaponDataByType[(int)playerWeaponData.type];
			for ( int i = 0; i < weaponData.projectiles; i++ )
			{
				GameObject projectile = CreateGameObject( "Projectile" );
				projectile.Transform.Position = gunPos + weaponData.positionOffset[i];

				CreateComponentData<SpriteComponentData>( projectile.Id ).assetName = weaponData.projectileSprite;

				BoxCollision2DComponentData collider = CreateComponentData<BoxCollision2DComponentData>( projectile.Id );
				collider.drawDebug = playerWeaponData.drawDebugProjectile;
				collider.Width = 15;
				collider.Height = 40;

				ProjectileComponentData projectileInstanceData = CreateComponentData<ProjectileComponentData>( projectile.Id );
				projectileInstanceData.hitLifetime = weaponData.projectileHitLifetime;
				projectileInstanceData.damage = weaponData.damage;
				projectileInstanceData.speed = weaponData.projectileSpeed;
				projectileInstanceData.hitSound = weaponData.hitSound;
				projectileInstanceData.hitSprite = weaponData.projectileHitSprite;
			}
			playerWeaponData.currentRechargeTime = weaponData.rechargeTime;
			SoundComponentSystem.PlayOnce( weaponData.shootSound, weaponData.shootSoundVolume );
		}
	}
}
