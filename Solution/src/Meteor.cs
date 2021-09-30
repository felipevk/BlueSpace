using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public enum MeteorPath
	{
		Linear,
		Senoid
	}
	public enum MeteorSize
	{
		Small,
		Big
	}

	[RequiresComponentData( typeof( SpriteComponentData ) )]
	[RequiresComponentData( typeof( BoxCollision2DComponentData ) )]
	public class MeteorComponentData : ComponentData
	{
		public int health;
		public int damage;
		public int score;
		public MeteorPath path;
		public MeteorSize size;
		public String hitSound;
		public String destroySound;
		public String playerId;
		public float hitSoundVolume = 1f;
		public float destroySoundVolume = 1f;
		public float speed;
		public float senoidArc;
		public float totalTime = 0f;
	}

	public class MeteorComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			MeteorComponentData meteorData = data as MeteorComponentData;

			if ( meteorData.health <= 0 )
			{
				DestroyGameObject( gameObjectId );
				return;
			}

			GameObject gameObj = GetGameObject( gameObjectId );
			Vector3 position = gameObj.Transform.Position;

			meteorData.totalTime += Time.DeltaTime;
			
			switch ( meteorData.path )
			{
				case MeteorPath.Linear:
					position += Vector3.Up * Time.DeltaTime * meteorData.speed;
					break;
				case MeteorPath.Senoid:
					position.Y += Time.DeltaTime * meteorData.speed;
					position.X += (float)Math.Cos( meteorData.totalTime ) * Time.DeltaTime * meteorData.senoidArc;
					break;
				default:
					break;
			}

			gameObj.Transform.Position = position;
		}

		protected override void OnCollision2DEnter( string gameObjectId, ComponentData data, string colliderId )
		{
			if ( HasComponentData<ProjectileComponentData>( colliderId ) )
			{
				ProjectileComponentData projectileData = GetComponentData<ProjectileComponentData>( colliderId );
				MeteorComponentData meteorData = data as MeteorComponentData;

				 meteorData.health -= projectileData.damage;

				if ( meteorData.health > 0 )
				{
					SoundComponentSystem.PlayOnce( meteorData.hitSound, meteorData.hitSoundVolume );
				}
				else
				{
					SoundComponentSystem.PlayOnce( meteorData.destroySound, meteorData.destroySoundVolume );
					DestroyGameObject( gameObjectId );
					GetComponentSystem<PlayerScoreComponentSystem>().AddScore( meteorData.playerId, meteorData.score );
				}
			}
		}
	}
}
