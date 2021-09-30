using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData(typeof(TextComponentData))]
	public class PlayerScoreHUDComponentData : ComponentData
	{
		public String id;
	}

	public class PlayerScoreHUDComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			PlayerScoreHUDComponentData playerHudData = data as PlayerScoreHUDComponentData;
			PlayerScoreComponentData playerScoreData = GetComponentData<PlayerScoreComponentData>( playerHudData.id );
			GetComponentData<TextComponentData>( gameObjectId ).text = playerScoreData.score.ToString( "D8" );

			playerScoreData.onScored +=
				( int newScore ) =>
				{
					GetComponentData<TextComponentData>( gameObjectId ).text = newScore.ToString("D8");
				};
		}
	}
}
