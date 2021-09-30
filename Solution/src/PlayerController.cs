using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData(typeof(SpriteComponentData))]
	public class PlayerControllerComponentData : ComponentData
	{
		public Vector2 direction = Vector2.Zero;
		public String regularSpriteName = "player";
		public String moveLeftSpriteName = "playerLeft";
		public String moveRightSpriteName = "playerRight";
	}

	public class PlayerControllerComponentSystem : ComponentSystem
	{
		public static float Speed = 500;
		protected override void Update( String gameObjectId, ComponentData data )
		{
			PlayerControllerComponentData playerControllerData = data as PlayerControllerComponentData;

			Vector2 direction = playerControllerData.direction;

			String selectedSprite = playerControllerData.regularSpriteName;

			if ( Input.IsButtonDown( "right", 0 ) )
			{
				direction.X = 1;
				selectedSprite = playerControllerData.moveRightSpriteName;
			}
			else if ( Input.IsButtonDown( "left", 0 ) )
			{
				direction.X = -1;
				selectedSprite = playerControllerData.moveLeftSpriteName;
			}
			else
			{
				direction.X = 0;
			}

			if ( Input.IsButtonDown( "up", 0 ) )
			{
				direction.Y = -1;
			}
			else if ( Input.IsButtonDown( "down", 0 ) )
			{
				direction.Y = 1;
			}
			else
			{
				direction.Y = 0;
			}

			playerControllerData.direction = direction;

			GameObject player = GetGameObject( gameObjectId );
			Vector3 playerPos = player.Transform.Position;
			playerPos.X += playerControllerData.direction.X * Speed * Time.DeltaTime;
			playerPos.Y += playerControllerData.direction.Y * Speed * Time.DeltaTime;

			player.Transform.Position = playerPos;

			SpriteComponentData playerSprite = GetComponentData<SpriteComponentData>( gameObjectId );
			if ( playerSprite.assetName != selectedSprite )
			{
				playerSprite.assetName = selectedSprite;
			}
		}
	} 
}
