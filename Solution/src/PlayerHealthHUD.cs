using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlueSpace
{
	public class PlayerHUDComponentData : ComponentData
	{
		public String assetName;
		public String id;
		public PlayerHealthComponentData playerHealthData;
		public float xOffset = 10f;
		public Vector2 healthPointScale;
	}

	public class PlayerHUDComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			PlayerHUDComponentData hudData = data as PlayerHUDComponentData;
			hudData.playerHealthData = GetComponentData<PlayerHealthComponentData>( hudData.id );
		}
		protected override void Render( string gameObjectId, ComponentData data )
		{
			PlayerHUDComponentData hudData = data as PlayerHUDComponentData;
			Texture2D healthTexture = Blue.Game.Instance.AssetManager.GetAsset<SpriteAsset>( hudData.assetName ).Texture2D;

			Vector3 globalPos = GetGameObject( gameObjectId ).GetGlobalPosition();
			Vector2 healthPointPos = new Vector2();
			healthPointPos.X = globalPos.X;
			healthPointPos.Y = globalPos.Y;

			for ( int i = 0; i < hudData.playerHealthData.health; i++ )
			{
				Blue.Game.Instance.GameRenderer.PrepareToDrawSprite( healthTexture, healthPointPos, hudData.healthPointScale, Color.Red );
				healthPointPos.X += (int)( healthTexture.Width * hudData.healthPointScale.X ) + hudData.xOffset;
			}
		}
	}
}
