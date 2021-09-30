using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class MeteorSpawnerComponentData : ComponentData
	{
		public Interval timeToSpawn;
		public float currentTimeToSpawn = 0;
		public float totalTime = 0;
		public float yPos = 0f;
		public Interval meteorsToSpawn;
		public Interval xPos;
		public Interval speed;
		public Interval senoidArc;
		public String playerId;
		public bool drawDebug;
	}

	public class MeteorSpawnerComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			MeteorSpawnerComponentData spawnerData = data as MeteorSpawnerComponentData;
			spawnerData.totalTime += Time.DeltaTime;

			spawnerData.currentTimeToSpawn -= Time.DeltaTime;
			if ( spawnerData.currentTimeToSpawn < 0f )
			{
				spawnerData.currentTimeToSpawn = spawnerData.timeToSpawn.Random();
				int meteorsToSpawn = (int)spawnerData.meteorsToSpawn.Random();
				for ( int i = 0; i < meteorsToSpawn; i++ )
				{
					SpawnMeteor( spawnerData );
				}
			}
		}

		private void SpawnMeteor( MeteorSpawnerComponentData spawnerData )
		{
			Random rand = new Random();
			GameObject meteor = CreateGameObject( "Meteor" );
			meteor.Transform.Position = new Vector3( spawnerData.xPos.Random(), spawnerData.yPos, 0 );
			SpriteComponentData meteorSprite = CreateComponentData<SpriteComponentData>( meteor.Id );
			MeteorComponentData meteorData = CreateComponentData<MeteorComponentData>( meteor.Id );
			meteorData.path = (MeteorPath) rand.Next(0, Enum.GetNames( typeof( MeteorPath ) ).Length);
			meteorData.size = (MeteorSize)rand.Next( 0, Enum.GetNames( typeof( MeteorSize ) ).Length );
			meteorData.speed = spawnerData.speed.Random();
			meteorData.senoidArc = spawnerData.senoidArc.Random();
			meteorData.hitSound = "meteorHit";
			meteorData.destroySound = "meteorDestroy";
			meteorData.hitSoundVolume = 1f;
			meteorData.destroySoundVolume = 1f;
			meteorData.playerId = spawnerData.playerId;
			BoxCollision2DComponentData meteorCollider = CreateComponentData<BoxCollision2DComponentData>( meteor.Id );
			meteorCollider.drawDebug = spawnerData.drawDebug;
			switch ( meteorData.size )
			{
				case MeteorSize.Small:
					meteorSprite.assetName = "meteorSmall";
					meteorCollider.Width = 50;
					meteorCollider.Height = 50;
					meteorData.health = 20;
					meteorData.damage = 1;
					meteorData.score = 100;
					break;
				case MeteorSize.Big:
					meteorSprite.assetName = "meteorBig";
					meteorCollider.Width = 150;
					meteorCollider.Height = 150;
					meteorData.health = 40;
					meteorData.damage = 1;
					meteorData.score = 200;
					break;
			}
		}
	}
}
