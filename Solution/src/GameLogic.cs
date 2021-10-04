using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public enum GameState
	{
		StartMenu,
		Game,
		GameOver
	}

	public class GameLogicComponentData : ComponentData
	{
		public int scoreToWin;
		public GameState state = GameState.StartMenu;
		public float currentTime = 0;

		public float timeToHideObjective;
		public float timeToShowGameOverSubtitle;

		public bool isWin = false;

		public TextComponentData startMenuTitle;
		public TextComponentData startMenuSubTitle;
		public TextComponentData gameObjective;
		public TextComponentData gameOverTitle;
		public TextComponentData gameOverSubTitle;

		public PlayerScoreComponentData playerScore;
		public MeteorSpawnerComponentData meteorSpawner;
		public PickupSpawnerComponentData pickupSpawner;
		public PlayerWeaponComponentData playerWeapon;
		public PlayerHealthComponentData playerHealth;
		public PlayerControllerComponentData playerController;

		public String playerId;
	}

	public class GameLogicComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			GameLogicComponentData gameLogicData = data as GameLogicComponentData;

			gameLogicData.playerScore.onScored +=
				( int newScore ) =>
				{
					if ( newScore >= gameLogicData.scoreToWin )
					{
						gameLogicData.isWin = true;
						SwitchState( gameLogicData, GameState.GameOver );
					}
				};

			gameLogicData.playerHealth.onDeath +=
				() =>
				{
					gameLogicData.isWin = false;
					SwitchState( gameLogicData, GameState.GameOver );
				};

			SwitchState( gameLogicData, GameState.StartMenu );
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			GameLogicComponentData gameLogicData = data as GameLogicComponentData;

			switch ( gameLogicData.state )
			{
				case GameState.StartMenu:
					UpdateStartMenu( gameLogicData );
					break;
				case GameState.Game:
					UpdateGame( gameLogicData );
					break;
				case GameState.GameOver:
					UpdateGameOver( gameLogicData );
					break;
				default:
					break;
			}
		}

		private void UpdateStartMenu( GameLogicComponentData gameLogicData )
		{
			if ( Input.IsButtonDown( "fire1", 0 ) )
			{
				SwitchState( gameLogicData, GameState.Game );
			}
		}

		private void UpdateGame( GameLogicComponentData gameLogicData )
		{
			if ( gameLogicData.currentTime > gameLogicData.timeToHideObjective )
			{
				gameLogicData.gameObjective.enabled = false;
			}
			else
			{
				gameLogicData.currentTime += Time.DeltaTime;
			}
		}

		private void UpdateGameOver( GameLogicComponentData gameLogicData )
		{
			if ( gameLogicData.currentTime > gameLogicData.timeToShowGameOverSubtitle )
			{
				gameLogicData.gameOverSubTitle.enabled = true;
				if ( Input.IsButtonDown( "fire1", 0 ) )
				{
					SwitchState( gameLogicData, GameState.Game );
				}
			}
			else
			{
				gameLogicData.currentTime += Time.DeltaTime;
			}
		}

		private void SwitchState( GameLogicComponentData gameLogicData, GameState newState )
		{
			gameLogicData.state = newState;
			gameLogicData.currentTime = 0;
			switch ( gameLogicData.state )
			{
				case GameState.StartMenu:
					gameLogicData.startMenuTitle.enabled = true;
					gameLogicData.startMenuSubTitle.enabled = true;
					gameLogicData.gameOverTitle.enabled = false;
					gameLogicData.gameOverSubTitle.enabled = false;
					gameLogicData.gameObjective.enabled = false;
					gameLogicData.playerController.enabled = false;
					gameLogicData.playerWeapon.enabled = true;

					gameLogicData.meteorSpawner.enabled = false;
					gameLogicData.pickupSpawner.enabled = false;
					break;
				case GameState.Game:
					gameLogicData.startMenuTitle.enabled = false;
					gameLogicData.startMenuSubTitle.enabled = false;
					gameLogicData.gameOverTitle.enabled = false;
					gameLogicData.gameOverSubTitle.enabled = false;
					gameLogicData.gameObjective.enabled = true;
					gameLogicData.playerController.enabled = true;
					gameLogicData.playerWeapon.enabled = true;

					gameLogicData.meteorSpawner.enabled = true;
					gameLogicData.pickupSpawner.enabled = true;
					gameLogicData.playerHealth.enabled = true;

					PlayerScoreComponentSystem.Reset( gameLogicData.playerScore );
					PlayerWeaponComponentSystem.Reset( gameLogicData.playerWeapon );
					PickupSpawnerComponentSystem.Reset( gameLogicData.pickupSpawner );
					GetComponentSystem<PlayerControllerComponentSystem>().Reset( gameLogicData.playerId );
					GetComponentSystem<PlayerHealthComponentSystem>().Reset( gameLogicData.playerId );
					break;
				case GameState.GameOver:
					gameLogicData.startMenuTitle.enabled = false;
					gameLogicData.startMenuSubTitle.enabled = false;
					gameLogicData.gameOverTitle.enabled = true;
					gameLogicData.gameOverSubTitle.enabled = false;
					gameLogicData.gameObjective.enabled = false;
					gameLogicData.playerController.enabled = false;
					gameLogicData.playerWeapon.enabled = false;

					gameLogicData.meteorSpawner.enabled = false;
					gameLogicData.pickupSpawner.enabled = false;
					gameLogicData.playerHealth.enabled = false;

					if ( gameLogicData.isWin )
					{
						gameLogicData.gameOverTitle.text = "VICTORY";
						gameLogicData.gameOverTitle.color = Color.Green;
					}
					else
					{
						gameLogicData.gameOverTitle.text = "DEFEAT";
						gameLogicData.gameOverTitle.color = Color.Red;
					}
					break;
				default:
					break;
			}
		}
	}
}
