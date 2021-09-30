using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData( typeof( SpriteComponentData ) )]
	public class SpriteBlinkComponentData : ComponentData
	{
		public Color originalColor;
		public Color blinkColor;
		public bool isBlink = false;
		public float blinkDuration = 0.5f;
		public float currentBlinkTime;
	}

	public class SpriteBlinkComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			SpriteBlinkComponentData blinkData = data as SpriteBlinkComponentData;

			blinkData.currentBlinkTime -= Time.DeltaTime;
			if ( blinkData.currentBlinkTime <= 0f )
			{
				blinkData.currentBlinkTime = blinkData.blinkDuration;
				SpriteComponentData spriteData = GetComponentData<SpriteComponentData>( gameObjectId );
				blinkData.isBlink = !blinkData.isBlink;
				spriteData.color = blinkData.isBlink ? blinkData.blinkColor : blinkData.originalColor;
			}
		}

		public void SetBlink( string gameObjectId, SpriteBlinkComponentData blinkData, bool isBlink )
		{
			GetComponentData<SpriteComponentData>( gameObjectId ).color = isBlink ? blinkData.blinkColor : blinkData.originalColor;
		}
	}
}
