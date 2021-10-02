using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData( typeof( TextComponentData ) )]
	public class TextBlinkComponentData : ComponentData
	{
		public bool isBlink = false;
		public float blinkDuration = 0.5f;
		public float currentBlinkTime;
	}

	public class TextBlinkComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			TextBlinkComponentData blinkData = data as TextBlinkComponentData;

			blinkData.currentBlinkTime -= Time.DeltaTime;
			if ( blinkData.currentBlinkTime <= 0f )
			{
				blinkData.currentBlinkTime = blinkData.blinkDuration;
				TextComponentData textData = GetComponentData<TextComponentData>( gameObjectId );
				blinkData.isBlink = !blinkData.isBlink;
				textData.enabled = blinkData.isBlink;
			}
		}

		public void SetBlink( string gameObjectId, TextBlinkComponentData blinkData, bool isBlink )
		{
			GetComponentData<TextComponentData>( gameObjectId ).enabled = !isBlink;
		}
	}
}
