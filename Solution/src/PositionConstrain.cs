using System;

using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class PositionConstrainComponentData : ComponentData
	{
		public bool useWindowBounds = true;
		public Vector2 min = Vector2.Zero;
		public Vector2 max = Vector2.Zero;
	}

	public class PositionConstrainComponentSystem : ComponentSystem
	{
		protected override void Update( String gameObjectId, ComponentData data )
		{
			PositionConstrainComponentData positionConstrainComponentData = data as PositionConstrainComponentData;

			GameObject player = GetGameObject( gameObjectId );
			Vector3 playerPos = player.Transform.Position;

			Vector2 min = positionConstrainComponentData.min;
			Vector2 max = positionConstrainComponentData.max;
			if ( positionConstrainComponentData.useWindowBounds )
			{
				min = Vector2.Zero;
				max = new Vector2( Blue.Game.Instance.WindowWidth, Blue.Game.Instance.WindowHeight );
			}

			playerPos.X = Math.Clamp( playerPos.X, min.X, max.X );
			playerPos.Y = Math.Clamp( playerPos.Y, min.Y, max.Y );

			player.Transform.Position = playerPos;
		}
	} 
}