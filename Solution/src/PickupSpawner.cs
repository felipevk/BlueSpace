using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class PickupSpawnerComponentData : ComponentData
	{
		public float currentTimeToSpawn;
		public Interval timeToSpawn;
		public Interval xPos;
		public float yPos = 0f;
		public float totalTime;
		public float timeWithoutPickups;
		public float[] timeToUpgrade;
		public int currentUpgrade = 0;
	}

	public class PickupSpawnerComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			PickupSpawnerComponentData pickupSpawnerData = data as PickupSpawnerComponentData;
			pickupSpawnerData.totalTime += Time.DeltaTime;
			if ( pickupSpawnerData.totalTime < pickupSpawnerData.timeWithoutPickups )
				return;

			pickupSpawnerData.currentTimeToSpawn -= Time.DeltaTime;

			if ( pickupSpawnerData.currentTimeToSpawn <= 0 )
			{
				pickupSpawnerData.currentTimeToSpawn = pickupSpawnerData.timeToSpawn.Random();

				Random rand = new Random();
				bool spawnHealthPickup = rand.Next(0, 2) == 0;
				if ( spawnHealthPickup )
				{
					GameObject healthPickup = CreateGameObject( "Health" );
					healthPickup.Transform.Position = new Vector3( pickupSpawnerData.xPos.Random(), pickupSpawnerData.yPos, 0 );
					SpriteComponentData pickupSprite = CreateComponentData<SpriteComponentData>( healthPickup.Id );
					pickupSprite.assetName = "pill_green";
					HealthPickupComponentData healthPickupData = CreateComponentData<HealthPickupComponentData>( healthPickup.Id );
					healthPickupData.points = 1;
					healthPickupData.speed = 200;
					BoxCollision2DComponentData healthCollider = GetComponentData<BoxCollision2DComponentData>( healthPickup.Id );
					healthCollider.Width = 20;
					healthCollider.Height = 20;
				}
				else
				{
					PlayerWeaponType weaponType = (PlayerWeaponType)pickupSpawnerData.currentUpgrade+1;
					GameObject upgradePickup = CreateGameObject( "Upgrade" );
					upgradePickup.Transform.Position = new Vector3( pickupSpawnerData.xPos.Random(), pickupSpawnerData.yPos, 0 );
					SpriteComponentData pickupSprite = CreateComponentData<SpriteComponentData>( upgradePickup.Id );
					UpgradePickupComponentData upgradePickupData = CreateComponentData<UpgradePickupComponentData>( upgradePickup.Id );
					upgradePickupData.upgradeType = weaponType;
					upgradePickupData.speed = 200;
					BoxCollision2DComponentData upgradeCollider = GetComponentData<BoxCollision2DComponentData>( upgradePickup.Id );
					upgradeCollider.Width = 20;
					upgradeCollider.Height = 20;

					switch ( weaponType )
					{
						case PlayerWeaponType.Shotgun:
							pickupSprite.assetName = "powerupYellow_bolt";
							break;
						case PlayerWeaponType.GatlingGun:
							pickupSprite.assetName = "powerupRed_bolt";
							break;
					}
				}
			}

			if ( pickupSpawnerData.currentUpgrade < pickupSpawnerData.timeToUpgrade.Length &&
				pickupSpawnerData.totalTime > pickupSpawnerData.timeToUpgrade[pickupSpawnerData.currentUpgrade] )
				pickupSpawnerData.currentUpgrade++;
		}
	}
}
