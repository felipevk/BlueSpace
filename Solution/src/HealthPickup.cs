using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData(typeof(BoxCollision2DComponentData))]
	public class HealthPickupComponentData : ComponentData
	{
		public int points;
		public float speed;
	}


	public class HealthPickupComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			HealthPickupComponentData healthPickup = data as HealthPickupComponentData;

			GameObject gameObj = GetGameObject( gameObjectId );
			Vector3 position = gameObj.Transform.Position;
			position += Vector3.Up * Time.DeltaTime * healthPickup.speed;

			gameObj.Transform.Position = position;
		}

		protected override void OnCollision2DEnter( string gameObjectId, ComponentData data, string colliderId )
		{
			if ( HasComponentData<PlayerHealthComponentData>( colliderId ) )
			{
				HealthPickupComponentData healthPickup = data as HealthPickupComponentData;
				PlayerHealthComponentData playerHealth = GetComponentData<PlayerHealthComponentData>( colliderId );
				playerHealth.health += healthPickup.points;
				if ( playerHealth.health > playerHealth.maxHealth )
					playerHealth.health = playerHealth.maxHealth;
				// TODO sound effect
				DestroyGameObject( gameObjectId );
			}
		}
	}
}
