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
				AssetManager.AddAsset<SpriteAsset>( "background" );
				AssetManager.AddAsset<FontAsset>( "PixeloidSans" );
				AssetManager.AddAsset<SoundEffectAsset>( "laserShoot" );
				AssetManager.AddAsset<SoundEffectAsset>( "shotgunShoot" );
				AssetManager.AddAsset<SoundEffectAsset>( "meteorHit" );
				AssetManager.AddAsset<SoundEffectAsset>( "meteorDestroy" );
				AssetManager.AddAsset<SoundEffectAsset>( "soft-alert" );
			}
		}
		public class MainScene : Scene
		{
			public override void Start()
			{
				base.Start();
				BackgroundColor = Color.White;
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
				RegisterComponent<TextBlinkComponentSystem, TextBlinkComponentData>();
				RegisterComponent<PlayerHUDComponentSystem, PlayerHUDComponentData>();
				RegisterComponent<PlayerScoreComponentSystem, PlayerScoreComponentData>();
				RegisterComponent<PlayerScoreHUDComponentSystem, PlayerScoreHUDComponentData>();
				RegisterComponent<PickupSpawnerComponentSystem, PickupSpawnerComponentData>();
				RegisterComponent<HealthPickupComponentSystem, HealthPickupComponentData>();
				RegisterComponent<UpgradePickupComponentSystem, UpgradePickupComponentData>();
				RegisterComponent<DestroyWhenFarComponentSystem, DestroyWhenFarComponentData>();
				RegisterComponent<ScrollingBackgroundComponentSystem, ScrollingBackgroundComponentData>();
				RegisterComponent<OverheatHUDComponentSystem, OverheatHUDComponentData>();
				RegisterComponent<GameLogicComponentSystem, GameLogicComponentData>();
			}

			protected override void RegisterGameObjects()
			{
				base.RegisterGameObjects();

				GameObject background = CreateGameObject( "Background" );
				background.Transform.Position = new Vector3( Blue.Game.Instance.WindowWidth * 0.5f, Blue.Game.Instance.WindowHeight * 0.5f + 200, 0 );
				ScrollingBackgroundComponentData scrollingData = CreateComponentData<ScrollingBackgroundComponentData>( background.Id );
				scrollingData.assetName = "background";
				scrollingData.entries = 2;
				scrollingData.speed = 100;
				scrollingData.threshold = 800;
				scrollingData.initialYPos = -800;
				scrollingData.zOrder = -1;

				GameObject player = CreateGameObject( "Player" );
				CreateComponentData<PositionConstrainComponentData>( player.Id ).useWindowBounds = true;
				CreateComponentData<SpriteComponentData>( player.Id ).assetName = "player";
				CreateComponentData<PlayerControllerComponentData>( player.Id ).initialPos = new Vector3( 400, 600, 0 );
				CreateComponentData<PlayerScoreComponentData>( player.Id );
				PlayerHealthComponentData playerHealth = CreateComponentData<PlayerHealthComponentData>( player.Id );
				playerHealth.health = 5;
				playerHealth.initialHealth = 5;
				playerHealth.maxHealth = 8;
				playerHealth.hitDuration = 1.5f;
				SpriteBlinkComponentData playerBlink = CreateComponentData<SpriteBlinkComponentData>( player.Id );
				playerBlink.blinkDuration = 0.1f;
				BoxCollision2DComponentData playerCollider = GetComponentData<BoxCollision2DComponentData>( player.Id );
				playerCollider.drawDebug = false;
				playerCollider.Width = 35;
				playerCollider.Height = 55;

				PlayerWeaponComponentData playerWeapon = CreateComponentData<PlayerWeaponComponentData>( player.Id );
				playerWeapon.drawDebugProjectile = false;
				playerWeapon.playerSpriteId = player.Id;
				playerWeapon.initialType = PlayerWeaponType.Laser;

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
				healthHud.Transform.Position = new Vector3( 10, 720, 0 );
				PlayerHUDComponentData healthDisplay = CreateComponentData<PlayerHUDComponentData>( healthHud.Id );
				healthDisplay.id = player.Id;
				healthDisplay.assetName = "life";
				healthDisplay.xOffset = 10.0f;
				healthDisplay.zOrder = 3;
				healthDisplay.healthPointScale = new Vector2( 1f , 1f );

				GameObject scoreHud = CreateGameObject( "PlayerScore" );
				hud.AddChild( scoreHud );
				scoreHud.Transform.Position = new Vector3( 650, 740, 0 );
				PlayerScoreHUDComponentData scoreDisplay = CreateComponentData<PlayerScoreHUDComponentData>( scoreHud.Id );
				scoreDisplay.id = player.Id;
				TextComponentData scoreHudText = GetComponentData<TextComponentData>( scoreHud.Id );
				scoreHudText.assetName = "PixeloidSans";
				scoreHudText.scale = 2.0f;

				GameObject overheathHud = CreateGameObject( "Overheat" );
				hud.AddChild( overheathHud );
				overheathHud.Transform.Position = new Vector3( 10, 760, 0 );
				OverheatHUDComponentData overheatDisplay = CreateComponentData<OverheatHUDComponentData>( overheathHud.Id );
				overheatDisplay.playerWeaponId = player.Id;
				overheatDisplay.Width = 250;
				overheatDisplay.Height = 30;
				SoundComponentData overheatSound = CreateComponentData<SoundComponentData>( overheathHud.Id );
				overheatSound.assetName = "soft-alert";
				overheatSound.loop = true;

				GameObject overheathHudText = CreateGameObject( "OverheatText" );
				overheathHud.AddChild( overheathHudText );
				overheathHudText.Transform.Position = new Vector3( 255, 6, 0 );
				TextComponentData overheathHudTextData = CreateComponentData<TextComponentData>( overheathHudText.Id );
				overheathHudTextData.assetName = "PixeloidSans";
				overheathHudTextData.scale = 1.0f;
				overheathHudTextData.color = Color.Red;
				overheathHudTextData.text = "OVERHEAT";
				TextBlinkComponentData overheathHudTextBlink = CreateComponentData<TextBlinkComponentData>( overheathHudText.Id );
				overheathHudTextBlink.blinkDuration = 0.1f;
				overheathHudTextBlink.enabled = false;

				overheatDisplay.hudTextId = overheathHudText.Id;

				GameObject gameTitle = CreateGameObject( "GameTitle" );
				hud.AddChild( gameTitle );
				gameTitle.Transform.Position = new Vector3( 300, 250, 0 );
				TextComponentData gameTitleText = CreateComponentData<TextComponentData>( gameTitle.Id );
				gameTitleText.assetName = "PixeloidSans";
				gameTitleText.scale = 2.5f;
				gameTitleText.text = "Blue\n   Space";

				GameObject gameSubtitle = CreateGameObject( "GameSubtitle" );
				hud.AddChild( gameSubtitle );
				gameSubtitle.Transform.Position = new Vector3( 330, 400, 0 );
				TextComponentData gameSubtitleText = CreateComponentData<TextComponentData>( gameSubtitle.Id );
				gameSubtitleText.assetName = "PixeloidSans";
				gameSubtitleText.scale = 1.0f;
				gameSubtitleText.text = "Shoot to start";

				GameObject gameObjective = CreateGameObject( "GameObjective" );
				hud.AddChild( gameObjective );
				gameObjective.Transform.Position = new Vector3( 330, 300, 0 );
				TextComponentData gameObjectiveText = CreateComponentData<TextComponentData>( gameObjective.Id );
				gameObjectiveText.assetName = "PixeloidSans";
				gameObjectiveText.scale = 1.0f;

				GameObject gameOverTitle = CreateGameObject( "GameOverTitle" );
				hud.AddChild( gameOverTitle );
				gameOverTitle.Transform.Position = new Vector3( 330, 300, 0 );
				TextComponentData gameOverTitleText = CreateComponentData<TextComponentData>( gameOverTitle.Id );
				gameOverTitleText.assetName = "PixeloidSans";
				gameOverTitleText.scale = 2.0f;

				GameObject gameOverSubtitle = CreateGameObject( "GameOverSubtitle" );
				hud.AddChild( gameOverSubtitle );
				gameOverSubtitle.Transform.Position = new Vector3( 330, 350, 0 );
				TextComponentData gameOverSubtitleText = CreateComponentData<TextComponentData>( gameOverSubtitle.Id );
				gameOverSubtitleText.assetName = "PixeloidSans";
				gameOverSubtitleText.scale = 1.0f;
				gameOverSubtitleText.text = "Shoot to restart";

				GameObject gameManager = CreateGameObject( "GameManager" );
				GameLogicComponentData gameLogicData = CreateComponentData<GameLogicComponentData>( gameManager.Id );
				gameLogicData.playerId = player.Id; 
				gameLogicData.playerController = GetComponentData<PlayerControllerComponentData>( player.Id ); 
				gameLogicData.playerHealth = GetComponentData<PlayerHealthComponentData>( player.Id ); 
				gameLogicData.playerScore = GetComponentData<PlayerScoreComponentData>( player.Id ); 
				gameLogicData.playerWeapon = GetComponentData<PlayerWeaponComponentData>( player.Id ); 
				gameLogicData.pickupSpawner = GetComponentData<PickupSpawnerComponentData>( pickupSpawner.Id ); 
				gameLogicData.meteorSpawner = GetComponentData<MeteorSpawnerComponentData>( meteorSpawner.Id ); 

				gameLogicData.startMenuTitle = gameTitleText;
				gameLogicData.startMenuSubTitle = gameSubtitleText;
				gameLogicData.gameObjective = gameObjectiveText;
				gameLogicData.gameOverTitle = gameOverTitleText;
				gameLogicData.gameOverSubTitle = gameOverSubtitleText;

				gameLogicData.scoreToWin = 5000;
				gameLogicData.timeToHideObjective = 5;
				gameLogicData.timeToShowGameOverSubtitle = 1;

				gameObjectiveText.text = "Score " + gameLogicData.scoreToWin + " to win";
			}
		};
	} 
}
