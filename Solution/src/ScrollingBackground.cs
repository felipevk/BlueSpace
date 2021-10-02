using System;
using System.Collections.Generic;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlueSpace
{
	public class ScrollingBackgroundComponentData : ComponentData
	{
		public int entries;
		public Vector2[] positions;
		public float speed;
		public float initialYPos;
		public float threshold;
		public float backgroundHeight;
		public String assetName;
		public int zOrder = 0;
		public int currentLower = 0;
	}

	public class ScrollingBackgroundComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			ScrollingBackgroundComponentData backgroundData = data as ScrollingBackgroundComponentData;
			Texture2D backgroundTexture = Blue.Game.Instance.AssetManager.GetAsset<SpriteAsset>( backgroundData.assetName ).Texture2D;
			backgroundData.backgroundHeight = backgroundTexture.Height;
			float initialXPos = GetGameObject( gameObjectId ).GetGlobalPosition().X - ( backgroundTexture.Width * 0.5f );

			backgroundData.positions = new Vector2[backgroundData.entries];
			for ( int i = 0; i < backgroundData.positions.Length; i++ )
			{
				float yPos = backgroundData.initialYPos + ( backgroundData.backgroundHeight * i );
				backgroundData.positions[i] = new Vector2( initialXPos, yPos );
			}
			backgroundData.currentLower = backgroundData.entries - 1;
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			ScrollingBackgroundComponentData backgroundData = data as ScrollingBackgroundComponentData;

			for ( int i = 0; i < backgroundData.positions.Length; i++ )
			{
				backgroundData.positions[i].Y += Time.DeltaTime * backgroundData.speed;
			}

			if ( backgroundData.positions[backgroundData.currentLower].Y > backgroundData.threshold )
			{
				int nextIndex = ( backgroundData.currentLower + 1 ) % backgroundData.positions.Length;
				backgroundData.positions[backgroundData.currentLower].Y = backgroundData.positions[nextIndex].Y - backgroundData.backgroundHeight;
				backgroundData.currentLower --;
				if ( backgroundData.currentLower < 0 )
				{
					backgroundData.currentLower = backgroundData.positions.Length - 1;
				}
				Blue.Log.Message("Current lower: "+ backgroundData.currentLower );
			}
		}

		protected override void Render( string gameObjectId, ComponentData data )
		{
			ScrollingBackgroundComponentData backgroundData = data as ScrollingBackgroundComponentData;
			Texture2D backgroundTexture = Blue.Game.Instance.AssetManager.GetAsset<SpriteAsset>( backgroundData.assetName ).Texture2D;

			for ( int i = 0; i < backgroundData.positions.Length; i++ )
			{
				Blue.Game.Instance.GameRenderer.PrepareToDrawSprite( backgroundTexture, backgroundData.positions[i], Vector2.One, backgroundData.zOrder, Color.White );
			}
		}
	}
}
