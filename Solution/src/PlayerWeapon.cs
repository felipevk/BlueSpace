using System;
using System.Collections.Generic;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public enum PlayerWeaponType
	{
		Laser = 0,
		Shotgun = 1,
		GatlingGun = 2
	}

	public class PlayerWeaponComponentData : ComponentData
	{
		public PlayerWeaponType type;
		public PlayerWeaponType initialType = PlayerWeaponType.Laser;
		public float currentRechargeTime = 0;
		public float overheatDuration = 5;
		public float currentOverheat = 0;
		public float overheatCanShoot = 0.5f;
		public bool isOverheat = false;
		public bool isAutomatic = false;
		public String input = "fire1";
		public int playerIndex = 0;
		public bool drawDebugProjectile = false;
		public String playerSpriteId = "";
		public SpriteComponentData playerSprite;
	}

	public struct WeaponData
	{
		public bool isAutomatic;
		public float overheatAddPercentage;
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
		public List<Vector3> scale;
		public List<Tuple<int,int>> colliderSize;
		public List<Quaternion> rotations;
	}

	public class PlayerWeaponComponentSystem : ComponentSystem
	{
		static List<WeaponData> weaponDataByType = new List<WeaponData>();
		public override void Start()
		{
			base.Start();
			WeaponData laser = new WeaponData
			{
				isAutomatic = false,
				overheatAddPercentage = 0,
				damage = 5,
				projectiles = 1,
				rechargeTime = 0.1f,
				projectileSpeed = 1000,
				projectileSprite = "laserRed",
				projectileHitSprite = "laserRedShot",
				shootSound = "laserShoot",
				hitSound = "",
				shootSoundVolume = 0.2f,
				hitSoundVolume = 0.2f,
				scale = new List<Vector3>()
			};
			laser.scale.Add( Vector3.One );
			laser.positionOffset = new List<Vector3>
			{
				new Vector3( 0, -30, 0 )
			};
			laser.rotations = new List<Quaternion>
			{
				Quaternion.Identity
			};
			laser.colliderSize = new List<Tuple<int, int>>
			{
				new Tuple<int, int>( 15, 40 )
			};

			WeaponData shotgun = new WeaponData
			{
				isAutomatic = false,
				overheatAddPercentage = 0,
				damage = 8,
				projectiles = 2,
				rechargeTime = 0.05f,
				projectileSpeed = 1500,
				projectileSprite = "laserRed",
				projectileHitSprite = "laserRedShot",
				shootSound = "shotgunShoot",
				hitSound = "",
				shootSoundVolume = 0.2f,
				hitSoundVolume = 0.2f,
				scale = new List<Vector3>()
			};
			shotgun.scale.Add( new Vector3( 1.5f, 1, 1 ) );
			shotgun.scale.Add( new Vector3( 1.5f, 1, 1 ) );
			shotgun.positionOffset = new List<Vector3>
			{
				new Vector3( 15, -30, 0 ),
				new Vector3( -20, -30, 0 )
			};
			shotgun.rotations = new List<Quaternion>
			{
				Quaternion.Identity
			};
			shotgun.colliderSize = new List<Tuple<int, int>>
			{
				new Tuple<int, int>( 30, 40 ),
				new Tuple<int, int>( 30, 40 )
			};

			WeaponData gatlingGun = new WeaponData
			{
				isAutomatic = true,
				overheatAddPercentage = 0.05f,
				damage = 4,
				projectiles = 4,
				rechargeTime = 0.1f,
				projectileSpeed = 2000,
				projectileSprite = "laserGreen",
				projectileHitSprite = "laserGreenShot",
				shootSound = "shotgunShoot",
				hitSound = "",
				shootSoundVolume = 0.2f,
				hitSoundVolume = 0.2f,
				scale = new List<Vector3>()
			};
			gatlingGun.scale.Add( new Vector3( 1, 1, 1 ) );
			gatlingGun.scale.Add( new Vector3( 1, 1, 1 ) );
			gatlingGun.scale.Add( new Vector3( 1, 1, 1 ) );
			gatlingGun.scale.Add( new Vector3( 1, 1, 1 ) );
			gatlingGun.positionOffset = new List<Vector3>
			{
				new Vector3( 15, -30, 0 ),
				new Vector3( 30, -30, 0 ),
				new Vector3( -20, -30, 0 ),
				new Vector3( -40, -30, 0 )
			};
			gatlingGun.rotations = new List<Quaternion>
			{
				Quaternion.Identity,
				Quaternion.Identity,
				Quaternion.Identity,
				Quaternion.Identity
			};
			gatlingGun.colliderSize = new List<Tuple<int, int>>
			{
				new Tuple<int, int>( 15, 40 ),
				new Tuple<int, int>( 15, 40 ),
				new Tuple<int, int>( 15, 40 ),
				new Tuple<int, int>( 15, 40 )
			};

			weaponDataByType.Add( laser );
			weaponDataByType.Add( shotgun );
			weaponDataByType.Add( gatlingGun );
		}

		protected override void Start( string gameObjectId, ComponentData data )
		{
			PlayerWeaponComponentData playerWeaponData = data as PlayerWeaponComponentData;
			playerWeaponData.playerSprite = GetComponentData<SpriteComponentData>( playerWeaponData.playerSpriteId );
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
				if ( playerWeaponData.currentOverheat > 0f )
				{
					playerWeaponData.currentOverheat -= Time.DeltaTime;
					if ( playerWeaponData.isOverheat && 
						playerWeaponData.currentOverheat / playerWeaponData.overheatDuration < playerWeaponData.overheatCanShoot )
					{
						  playerWeaponData.isOverheat = false;
					}

					// Color.White (all 255) to Color.Red (r 255, g and b 0)
					float a = 255 * ( 1 - ( playerWeaponData.currentOverheat / playerWeaponData.overheatDuration ) );
					Color playerColor = new Color
					{
						R = 255,
						G = (byte)a,
						B = (byte)a,
						A = playerWeaponData.playerSprite.color.A
					};
					playerWeaponData.playerSprite.color = playerColor;
				}
				else
				{
					if ( playerWeaponData.isOverheat )
					{
						playerWeaponData.isOverheat = false;
						playerWeaponData.playerSprite.color = Color.White;
					}
				}
				bool isInputValid = playerWeaponData.isAutomatic ? 
					Input.IsButtonDown( playerWeaponData.input, playerWeaponData.playerIndex ) : 
					Input.IsButtonPressed( playerWeaponData.input, playerWeaponData.playerIndex );
				if ( isInputValid )
				{
					if ( playerWeaponData.isOverheat )
					{
						// TODO play gun clogged sound
					}
					else
					{
						Vector3 gunPos = GetGameObject( gameObjectId ).GetGlobalPosition();
						Shoot( gameObjectId, gunPos, playerWeaponData );
					}
				}
			}

		}

		public static void SwitchWeapon( PlayerWeaponComponentData playerWeapon, PlayerWeaponType weaponType )
		{
			WeaponData weaponData = weaponDataByType[(int)weaponType];

			playerWeapon.type = weaponType;
			playerWeapon.isAutomatic = weaponData.isAutomatic;
			playerWeapon.currentOverheat = 0;
		}

		public static void Reset( PlayerWeaponComponentData playerWeapon )
		{
			SwitchWeapon( playerWeapon, playerWeapon.initialType );
		}

		private void Shoot( String gameObjectId, Vector3 gunPos, PlayerWeaponComponentData playerWeaponData )
		{
			WeaponData weaponData = weaponDataByType[(int)playerWeaponData.type];
			for ( int i = 0; i < weaponData.projectiles; i++ )
			{
				GameObject projectile = CreateGameObject( "Projectile" );
				projectile.Transform.Position = gunPos + weaponData.positionOffset[i];
				projectile.Transform.Scale = weaponData.scale[i];

				CreateComponentData<SpriteComponentData>( projectile.Id ).assetName = weaponData.projectileSprite;

				BoxCollision2DComponentData collider = CreateComponentData<BoxCollision2DComponentData>( projectile.Id );
				collider.drawDebug = playerWeaponData.drawDebugProjectile;
				collider.Width = weaponData.colliderSize[i].Item1;
				collider.Height = weaponData.colliderSize[i].Item2;

				ProjectileComponentData projectileInstanceData = CreateComponentData<ProjectileComponentData>( projectile.Id );
				projectileInstanceData.hitLifetime = weaponData.projectileHitLifetime;
				projectileInstanceData.damage = weaponData.damage;
				projectileInstanceData.speed = weaponData.projectileSpeed;
				projectileInstanceData.hitSound = weaponData.hitSound;
				projectileInstanceData.hitSprite = weaponData.projectileHitSprite;

				CreateComponentData<DestroyWhenFarComponentData>( projectile.Id );
			}
			playerWeaponData.currentRechargeTime = weaponData.rechargeTime;
			SoundComponentSystem.PlayOnce( weaponData.shootSound, weaponData.shootSoundVolume );

			playerWeaponData.currentOverheat += playerWeaponData.overheatDuration * weaponData.overheatAddPercentage;
			if ( playerWeaponData.currentOverheat >= playerWeaponData.overheatDuration )
			{
				playerWeaponData.currentOverheat = playerWeaponData.overheatDuration;
				playerWeaponData.isOverheat = true;
				// TODO play overheat sound
			}
		}
	}
}
