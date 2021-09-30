using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData(typeof(SpriteComponentData))]
	[RequiresComponentData(typeof(BoxCollision2DComponentData))]
	public class ProjectileComponentData : ComponentData
	{
		public int damage;
		public String hitSprite;
		public String hitSound;
		public float speed;
		public bool isHit = false;
		public float hitLifetime;
		public float currentHitLifetime;
	}

	public class ProjectileComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			ProjectileComponentData projectileData = data as ProjectileComponentData;

			if ( projectileData.isHit )
			{
				if ( projectileData.currentHitLifetime < 0f )
				{
					DestroyGameObject( gameObjectId );
				}
				else
				{
					projectileData.currentHitLifetime -= Time.DeltaTime;
				}
			}
			else
			{
				GameObject gameObj = GetGameObject( gameObjectId );
				Vector3 position = gameObj.Transform.Position;
				position += Vector3.Down * Time.DeltaTime * projectileData.speed;

				gameObj.Transform.Position = position;
			}
		}

		protected override void OnCollision2DEnter( string gameObjectId, ComponentData data, string colliderId )
		{
			ProjectileComponentData projectileData = data as ProjectileComponentData;

			if ( projectileData.isHit )
				return;

			projectileData.isHit = true;
			projectileData.currentHitLifetime = projectileData.hitLifetime;
			GetComponentData<SpriteComponentData>( gameObjectId ).assetName = projectileData.hitSprite;
			GetComponentData<BoxCollision2DComponentData>( gameObjectId ).enabled = false;
		}
	}
}
