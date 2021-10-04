using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class PlayerScoreComponentData : ComponentData
	{
		public int score;
		public Action<int> onScored;
	}

	public class PlayerScoreComponentSystem : ComponentSystem
	{
		public void AddScore( String gameobjectId, int scoreToAdd )
		{
			PlayerScoreComponentData playerScoreData = GetComponentData<PlayerScoreComponentData>( gameobjectId );
			playerScoreData.score += scoreToAdd;
			playerScoreData.onScored?.Invoke( playerScoreData.score );
		}
		public static void Reset( PlayerScoreComponentData playerScore )
		{
			playerScore.score = 0;
			playerScore.onScored?.Invoke( playerScore.score );
		}
	}
}
