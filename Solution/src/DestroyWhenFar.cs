using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	public class DestroyWhenFarComponentData : ComponentData
	{
		public Vector3 center;
		public float maxDistance = 2000;
	}


	public class DestroyWhenFarComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			DestroyWhenFarComponentData destroyData = data as DestroyWhenFarComponentData;
			destroyData.center = new Vector3
			{
				X = Blue.Game.Instance.WindowWidth * 0.5f,
				Y = Blue.Game.Instance.WindowHeight * 0.5f,
				Z = 0,
			};
			
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			DestroyWhenFarComponentData destroyData = data as DestroyWhenFarComponentData;
			Vector3 position = GetGameObject( gameObjectId ).GetGlobalPosition();

			if ( Vector3.Distance( position, destroyData.center ) > destroyData.maxDistance )
			{
				DestroyGameObject( gameObjectId );
			}
		}
	}
}
