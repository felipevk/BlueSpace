using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Blue.Core;
using Blue.ECS;

namespace BlueSpace
{
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			MyGame.Run<MyGame>();
		}

		public class MyGame : Blue.Game
		{
			public static void Run<T>()
			where T : Game, new()
			{
				var factory = new MonoGame.Framework.GameFrameworkViewSource<T>();
				Windows.ApplicationModel.Core.CoreApplication.Run( factory );
			}

			protected override void Initialize()
			{
				CurrentScene = new MainScene();
				WindowWidth = 800;
				WindowHeight = 800;
				base.Initialize();

				Input.CreateInputGroup( "fire1" );
				Input.AddButtonToGroup( "fire1", Buttons.RightTrigger );
				Input.AddKeyToGroup( "fire1", Keys.Space );
				Input.AddKeyToGroup( "fire1", Keys.Enter );

				Input.CreateInputGroup( "left" );
				Input.AddKeyToGroup( "left", Keys.Left );
				Input.AddKeyToGroup( "left", Keys.A );
				Input.CreateInputGroup( "right" );
				Input.AddKeyToGroup( "right", Keys.Right );
				Input.AddKeyToGroup( "right", Keys.D );
				Input.CreateInputGroup( "up" );
				Input.AddKeyToGroup( "up", Keys.Up );
				Input.AddKeyToGroup( "up", Keys.W );
				Input.CreateInputGroup( "down" );
				Input.AddKeyToGroup( "down", Keys.Down );
				Input.AddKeyToGroup( "down", Keys.S );
			}

			protected override void LoadContent()
			{
				base.LoadContent();

				AssetManager.AddAsset<SpriteAsset>( "player" );
				AssetManager.AddAsset<SpriteAsset>( "playerLeft" );
				AssetManager.AddAsset<SpriteAsset>( "playerRight" );
				AssetManager.AddAsset<SpriteAsset>( "laserGreen" );
				AssetManager.AddAsset<SpriteAsset>( "laserGreenShot" );
				AssetManager.AddAsset<SpriteAsset>( "laserRed" );
				AssetManager.AddAsset<SpriteAsset>( "laserRedShot" );
				AssetManager.AddAsset<SpriteAsset>( "life" );
				AssetManager.AddAsset<SpriteAsset>( "pill_green" );
				AssetManager.AddAsset<SpriteAsset>( "powerupRed_bolt" );
				AssetManager.AddAsset<SpriteAsset>( "powerupYellow_bolt" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_big1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_big3" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_big4" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_med1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_med3" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_small1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_small2" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_tiny1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorBrown_tiny2" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_big1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_big3" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_big4" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_med1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_med2" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_small1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_small2" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_tiny1" );
				AssetManager.AddAsset<SpriteAsset>( "meteorGrey_tiny2" );
				AssetManager.AddAsset<SpriteAsset>( "star1" );
				AssetManager.AddAsset<SpriteAsset>( "star2" );
				AssetManager.AddAsset<SpriteAsset>( "star3" );
				AssetManager.AddAsset<SpriteAsset>( "star_4" );
				AssetManager.AddAsset<FontAsset>( "PixeloidSans" );
				AssetManager.AddAsset<SoundEffectAsset>( "laserShoot" );
				AssetManager.AddAsset<SoundEffectAsset>( "shotgunShoot" );
				AssetManager.AddAsset<SoundEffectAsset>( "meteorHit" );
				AssetManager.AddAsset<SoundEffectAsset>( "meteorDestroy" );
			}
		}
		public class MainScene : Scene
		{
			public override void Start()
			{
				base.Start();
				BackgroundColor = Color.Black;
			}

			protected override void RegisterComponents()
			{
				base.RegisterComponents();

				RegisterComponent<PositionConstrainComponentSystem, PositionConstrainComponentData>();
				RegisterComponent<PlayerControllerComponentSystem, PlayerControllerComponentData>();
				RegisterComponent<PlayerWeaponComponentSystem, PlayerWeaponComponentData>();
				RegisterComponent<ProjectileComponentSystem, ProjectileComponentData>();
				RegisterComponent<MeteorComponentSystem, MeteorComponentData>();
				RegisterComponent<MeteorSpawnerComponentSystem, MeteorSpawnerComponentData>();
				RegisterComponent<PlayerHealthComponentSystem, PlayerHealthComponentData>();
				RegisterComponent<SpriteBlinkComponentSystem, SpriteBlinkComponentData>();
				RegisterComponent<PlayerHUDComponentSystem, PlayerHUDComponentData>();
				RegisterComponent<PlayerScoreComponentSystem, PlayerScoreComponentData>();
				RegisterComponent<PlayerScoreHUDComponentSystem, PlayerScoreHUDComponentData>();
				RegisterComponent<PickupSpawnerComponentSystem, PickupSpawnerComponentData>();
				RegisterComponent<HealthPickupComponentSystem, HealthPickupComponentData>();
				RegisterComponent<UpgradePickupComponentSystem, UpgradePickupComponentData>();
				RegisterComponent<DestroyWhenFarComponentSystem, DestroyWhenFarComponentData>();
			}

			protected override void RegisterGameObjects()
			{
				base.RegisterGameObjects();

				GameObject player = CreateGameObject( "Player" );
				player.Transform.Position = new Vector3( 400, 600, 0 );
				CreateComponentData<PositionConstrainComponentData>( player.Id ).useWindowBounds = true;
				CreateComponentData<SpriteComponentData>( player.Id ).assetName = "player";
				CreateComponentData<PlayerControllerComponentData>( player.Id );
				CreateComponentData<PlayerScoreComponentData>( player.Id );
				PlayerHealthComponentData playerHealth = CreateComponentData<PlayerHealthComponentData>( player.Id );
				playerHealth.health = 5;
				playerHealth.maxHealth = 8;
				playerHealth.hitDuration = 3;
				// TODO fix blink w/ overheat
				SpriteBlinkComponentData playerBlink = CreateComponentData<SpriteBlinkComponentData>( player.Id );
				playerBlink.originalColor = Color.White;
				playerBlink.blinkColor = Color.Transparent;
				playerBlink.blinkDuration = 0.1f;
				BoxCollision2DComponentData playerCollider = GetComponentData<BoxCollision2DComponentData>( player.Id );
				playerCollider.drawDebug = false;
				playerCollider.Width = 35;
				playerCollider.Height = 55;

				PlayerWeaponComponentData playerWeapon = CreateComponentData<PlayerWeaponComponentData>( player.Id );
				playerWeapon.drawDebugProjectile = false;
				playerWeapon.playerSpriteId = player.Id;

				GameObject meteorSpawner = CreateGameObject( "MeteorSpawner" );
				MeteorSpawnerComponentData meteorSpawnerData = CreateComponentData<MeteorSpawnerComponentData>( meteorSpawner.Id );
				meteorSpawnerData.timeToSpawn = new Interval( 0f, 3f );
				meteorSpawnerData.xPos = new Interval( 0f, 800f );
				meteorSpawnerData.yPos = -20f;
				meteorSpawnerData.meteorsToSpawn = new Interval( 1f, 3f );
				meteorSpawnerData.speed = new Interval( 100f, 400f );
				meteorSpawnerData.senoidArc = new Interval( 50f, 300f );
				meteorSpawnerData.drawDebug = false;
				meteorSpawnerData.playerId = player.Id;
				meteorSpawnerData.meteorAssetNames = new Dictionary<MeteorSize, System.Collections.Generic.List<string>>
					{
						[MeteorSize.Big] = new List<string> {
						"meteorBrown_big1", "meteorBrown_big3", "meteorBrown_big4", "meteorGrey_big1", "meteorGrey_big3", "meteorGrey_big4"
					},
						[MeteorSize.Medium] = new List<string> {
						"meteorBrown_med1", "meteorBrown_med3", "meteorGrey_med1", "meteorGrey_med2"
					},
						[MeteorSize.Small] = new List<string> {
						"meteorBrown_small1", "meteorBrown_small2", "meteorGrey_small1", "meteorGrey_small2"
					},
						[MeteorSize.Tiny] = new List<string> {
						"meteorBrown_tiny1", "meteorBrown_tiny2", "meteorGrey_tiny1", "meteorGrey_tiny2"
					}
				};

				GameObject pickupSpawner = CreateGameObject( "PickupSpawner" );
				PickupSpawnerComponentData pickupSpawnerData = CreateComponentData<PickupSpawnerComponentData>( pickupSpawner.Id );
				pickupSpawnerData.timeToSpawn = new Interval( 0f, 3f );
				pickupSpawnerData.xPos = new Interval( 100f, 700f );
				pickupSpawnerData.yPos = -20f;
				pickupSpawnerData.timeToSpawn = new Interval( 2f, 4f );
				pickupSpawnerData.timeToUpgrade = new float[1];
				pickupSpawnerData.timeToUpgrade[0] = 30;
				pickupSpawnerData.timeWithoutPickups = 10f;

				GameObject hud = CreateGameObject( "HUD" );

				GameObject healthHud = CreateGameObject( "PlayerHealth" );
				hud.AddChild( healthHud );
				healthHud.Transform.Position = new Vector3( 10, 750, 0 );
				PlayerHUDComponentData healthDisplay = CreateComponentData<PlayerHUDComponentData>( healthHud.Id );
				healthDisplay.id = player.Id;
				healthDisplay.assetName = "life";
				healthDisplay.xOffset = 10.0f;
				healthDisplay.healthPointScale = new Vector2( 1f , 1f );

				GameObject scoreHud = CreateGameObject( "PlayerScore" );
				hud.AddChild( scoreHud );
				scoreHud.Transform.Position = new Vector3( 600, 740, 0 );
				PlayerScoreHUDComponentData scoreDisplay = CreateComponentData<PlayerScoreHUDComponentData>( scoreHud.Id );
				scoreDisplay.id = player.Id;
				TextComponentData scoreHudText = GetComponentData<TextComponentData>( scoreHud.Id );
				scoreHudText.assetName = "PixeloidSans";
				scoreHudText.scale = 2.0f;

				GameObject stars = CreateGameObject( "Stars" );
				stars.Transform.Position = new Vector3( 400, -40, 0 );
				ParticleComponentData starsParticles = CreateComponentData<ParticleComponentData>( stars.Id );
				starsParticles.emissorShape = ParticleSystemEmissorShape.Box;
				starsParticles.squareShapeWidth = 1000;
				starsParticles.squareShapeHeight = 20;
				starsParticles.lifetimeSeconds = 10f;
				starsParticles.timeToEmit = new Interval( 1f, 2f );
				starsParticles.lifetimeVariation = new Interval( 0f, 10f );
				starsParticles.spriteAssetName = "star3";
				starsParticles.directionVariationX = new Interval( -0.1f, 0.1f );
				starsParticles.directionVariationY = new Interval( 1f, 1f );
				starsParticles.particlesToEmitPerBurst = new Interval( 1f, 10f );
				starsParticles.preloadParticles = 10;
				starsParticles.speed = new Interval( 70, 80 );
				starsParticles.colorRVariation = new Interval( 0, 255 );
				starsParticles.colorGVariation = new Interval( 0, 255 );
				starsParticles.colorBVariation = new Interval( 0, 255 );
				starsParticles.drawDebug = true;

				GameObject stars2 = CreateGameObject( "Stars2" );
				stars2.Transform.Position = new Vector3( 400, -40, 0 );
				ParticleComponentData starsParticles2 = CreateComponentData<ParticleComponentData>( stars2.Id );
				starsParticles2.emissorShape = ParticleSystemEmissorShape.Box;
				starsParticles2.squareShapeWidth = 1000;
				starsParticles2.squareShapeHeight = 20;
				starsParticles2.lifetimeSeconds = 100f;
				starsParticles2.timeToEmit = new Interval( 1f, 5f );
				starsParticles2.lifetimeVariation = new Interval( 0f, 10f );
				starsParticles2.spriteAssetName = "star_4";
				starsParticles2.directionVariationX = new Interval( -0.1f, 0.1f );
				starsParticles2.directionVariationY = new Interval( 1f, 1f );
				starsParticles2.particlesToEmitPerBurst = new Interval( 10f, 30f );
				starsParticles2.preloadParticles = 10;
				starsParticles2.speed = new Interval( 30, 35 );
				starsParticles2.colorRVariation = new Interval( 0, 255 );
				starsParticles2.colorGVariation = new Interval( 0, 255 );
				starsParticles2.colorBVariation = new Interval( 0, 255 );
				starsParticles2.drawDebug = true;
			}
		};
	} 
}
