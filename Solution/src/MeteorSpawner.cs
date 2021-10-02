using System;
using System.Collections.Generic;

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
		public bool drawDebug = false;
		public Dictionary<MeteorSize, List<String>> meteorAssetNames;
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
			CreateComponentData<DestroyWhenFarComponentData>( meteor.Id );
			SpriteComponentData meteorSprite = CreateComponentData<SpriteComponentData>( meteor.Id );
			MeteorComponentData meteorData = CreateComponentData<MeteorComponentData>( meteor.Id );
			meteorData.path = (MeteorPath) rand.Next(0, Enum.GetNames( typeof( MeteorPath ) ).Length);
			meteorData.size = (MeteorSize)rand.Next( 0, Enum.GetNames( typeof( MeteorSize ) ).Length );
			meteorData.senoidArc = spawnerData.senoidArc.Random();
			meteorData.hitSound = "meteorHit";
			meteorData.destroySound = "meteorDestroy";
			meteorData.playerId = spawnerData.playerId;
			BoxCollision2DComponentData meteorCollider = CreateComponentData<BoxCollision2DComponentData>( meteor.Id );
			meteorCollider.drawDebug = spawnerData.drawDebug;

			List<String> meteorSprites = spawnerData.meteorAssetNames[meteorData.size];
			meteorSprite.assetName = meteorSprites[rand.Next( 0, meteorSprites.Count )];

			switch ( meteorData.size )
			{
				case MeteorSize.Big:
					meteorCollider.Width = 70;
					meteorCollider.Height = 50;
					meteorData.health = 40;
					meteorData.damage = 1;
					meteorData.score = 200;
					meteorData.hitSoundVolume = 1f;
					meteorData.destroySoundVolume = 1f;
					meteorData.speed = spawnerData.speed.Random();
					break;
				case MeteorSize.Medium:
					meteorCollider.Width = 30;
					meteorCollider.Height = 30;
					meteorData.health = 30;
					meteorData.damage = 1;
					meteorData.score = 100;
					meteorData.hitSoundVolume = 1f;
					meteorData.destroySoundVolume = 1f;
					meteorData.speed = spawnerData.speed.Random();
					break;
				case MeteorSize.Small:
					meteorCollider.Width = 20;
					meteorCollider.Height = 20;
					meteorData.health = 20;
					meteorData.damage = 1;
					meteorData.score = 10;
					meteorData.hitSoundVolume = 0.5f;
					meteorData.destroySoundVolume = 0.5f;
					meteorData.speed = spawnerData.speed.Random();
					break;
				case MeteorSize.Tiny:
					meteorCollider.Width = 10;
					meteorCollider.Height = 10;
					meteorData.health = 1;
					meteorData.damage = 0;
					meteorData.score = 10;
					meteorData.hitSoundVolume = 0.5f;
					meteorData.destroySoundVolume = 0.5f;
					meteorData.speed = spawnerData.speed.min;
					break;
			}
		}
	}
}
