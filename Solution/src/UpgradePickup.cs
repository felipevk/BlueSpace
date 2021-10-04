using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData( typeof( BoxCollision2DComponentData ) )]
	public class UpgradePickupComponentData : ComponentData
	{
		public PlayerWeaponType upgradeType;
		public float speed;
	}

	public class UpgradePickupComponentSystem : ComponentSystem
	{
		protected override void Update( string gameObjectId, ComponentData data )
		{
			UpgradePickupComponentData upgradePickup = data as UpgradePickupComponentData;

			GameObject gameObj = GetGameObject( gameObjectId );
			Vector3 position = gameObj.Transform.Position;
			position += Vector3.Up * Time.DeltaTime * upgradePickup.speed;

			gameObj.Transform.Position = position;
		}

		protected override void OnCollision2DEnter( string gameObjectId, ComponentData data, string colliderId )
		{
			if ( GetGameObject( colliderId ).Name.Equals("Player") )
			{
				UpgradePickupComponentData upgradePickup = data as UpgradePickupComponentData;
			}
			if ( HasComponentData<PlayerWeaponComponentData>( colliderId ) )
			{
				UpgradePickupComponentData upgradePickup = data as UpgradePickupComponentData;
				PlayerWeaponComponentData weaponData = GetComponentData<PlayerWeaponComponentData>( colliderId );
				PlayerWeaponComponentSystem.SwitchWeapon( weaponData, upgradePickup.upgradeType );

				// TODO sound effect
				DestroyGameObject( gameObjectId );
			}
		}
	}
}
