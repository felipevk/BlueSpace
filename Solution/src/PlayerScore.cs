using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class PlayerScoreComponentData : ComponentData
	{
		public int score;
		public int best;
		public Action<int> onScored;
	}

	public class PlayerScoreComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			PlayerScoreComponentData playerScoreData = GetComponentData<PlayerScoreComponentData>( gameObjectId );
			GetComponentData<PlayerHealthComponentData>( gameObjectId ).onDeath += 
				() =>
				{
					if ( playerScoreData.score > playerScoreData.best )
					{
						playerScoreData.best = playerScoreData.score;
						playerScoreData.onScored?.Invoke( playerScoreData.score );
					}
				};
		}

		public void AddScore( String gameobjectId, int scoreToAdd )
		{
			PlayerScoreComponentData playerScoreData = GetComponentData<PlayerScoreComponentData>( gameobjectId );
			playerScoreData.score += scoreToAdd;
			playerScoreData.onScored?.Invoke( playerScoreData.score );
		}
	}
}
