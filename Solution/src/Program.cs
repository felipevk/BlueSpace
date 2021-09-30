﻿using Microsoft.Xna.Framework;
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
				AssetManager.AddAsset<SpriteAsset>( "meteorBig" );
				AssetManager.AddAsset<SpriteAsset>( "meteorSmall" );
				AssetManager.AddAsset<SpriteAsset>( "laserGreen" );
				AssetManager.AddAsset<SpriteAsset>( "laserGreenShot" );
				AssetManager.AddAsset<SpriteAsset>( "laserRed" );
				AssetManager.AddAsset<SpriteAsset>( "laserRedShot" );
				AssetManager.AddAsset<FontAsset>( "PixeloidSans" );
				AssetManager.AddAsset<SoundEffectAsset>( "pop" );
				AssetManager.AddAsset<SoundEffectAsset>( "laserShoot" );
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
				playerHealth.hitDuration = 3;
				SpriteBlinkComponentData playerBlink = CreateComponentData<SpriteBlinkComponentData>( player.Id );
				playerBlink.originalColor = Color.White;
				playerBlink.blinkColor = Color.Transparent;
				playerBlink.blinkDuration = 0.1f;
				BoxCollision2DComponentData playerCollider = GetComponentData<BoxCollision2DComponentData>( player.Id );
				playerCollider.drawDebug = false;
				playerCollider.Width = 35;
				playerCollider.Height = 55;

				GameObject gun = CreateGameObject( "Gun" );
				player.AddChild( gun );
				CreateComponentData<PlayerWeaponComponentData>( gun.Id ).drawDebugProjectile = false;

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

				GameObject hud = CreateGameObject( "HUD" );

				GameObject healthHud = CreateGameObject( "PlayerHealth" );
				hud.AddChild( healthHud );
				healthHud.Transform.Position = new Vector3( 10, 750, 0 );
				PlayerHUDComponentData healthDisplay = CreateComponentData<PlayerHUDComponentData>( healthHud.Id );
				healthDisplay.id = player.Id;
				healthDisplay.assetName = "player";
				healthDisplay.xOffset = 10.0f;
				healthDisplay.healthPointScale = new Vector2( 0.3f , 0.3f );

				GameObject scoreHud = CreateGameObject( "PlayerScore" );
				hud.AddChild( scoreHud );
				scoreHud.Transform.Position = new Vector3( 600, 740, 0 );
				PlayerScoreHUDComponentData scoreDisplay = CreateComponentData<PlayerScoreHUDComponentData>( scoreHud.Id );
				scoreDisplay.id = player.Id;
				TextComponentData scoreHudText = GetComponentData<TextComponentData>( scoreHud.Id );
				scoreHudText.assetName = "PixeloidSans";
				scoreHudText.scale = 2.0f;

				//GameObject stars = CreateGameObject( "Stars" );
				//stars.Transform.Position = new Vector3( 600, 200, 0 );
				//ParticleComponentData starsParticles = CreateComponentData<ParticleComponentData>( stars.Id );
				//starsParticles.emissorShape = ParticleSystemEmissorShape.Box;
				//starsParticles.squareShapeWidth = 100;
				//starsParticles.squareShapeHeight = 400;
				//starsParticles.lifetimeSeconds = 2f;
				//starsParticles.timeToEmit = new Interval( 0.1f, 0.5f );
				//starsParticles.lifetimeVariation = new Interval( 1f, 5f );
				//starsParticles.spriteAssetName = "ball";
				//starsParticles.directionVariationX = new Interval( -1f, -1f );
				//starsParticles.directionVariationY = new Interval( -0.2f, 0.2f );
				//starsParticles.particlesToEmitPerBurst = new Interval( 5f, 20f );
				//starsParticles.preloadParticles = 100;
				//starsParticles.speed = new Interval( 100, 500 );
				//starsParticles.drawDebug = true;
			}
		};
	} 
}