using System;

using Blue;
using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

[RequiresComponentData( typeof( SpriteComponentData ) )]
public class LifetimeComponentData : ComponentData
{
	public int Life
	{ get; set; }
}
public class LifetimeComponentSystem : ComponentSystem
{
	protected override void Start( String gameObjectId, ComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			GameObject gameObj = GetGameObject( gameObjectId );
			Log.Message( "Entity " + gameObj.Name + " is alive" );
		}
	}

	protected override void Update( String gameObjectId, ComponentData data )
	{
		LifetimeComponentData lifetimeData = data as LifetimeComponentData;
		if ( IsAlive( lifetimeData ) )
		{
			lifetimeData.Life -= 1;
			GameObject gameObj = GetGameObject( gameObjectId );
			Vector3 position = gameObj.Transform.Position;
			position.X += 2;
			position.Y += 2;

			gameObj.Transform.Position = position;

			if ( !IsAlive( lifetimeData ) )
			{
				Log.Message( "Entity " + gameObj.Name + " is dead" );
				if ( HasComponentData<SpriteComponentData>( gameObjectId ) )
				{
					GetComponentData<SpriteComponentData>( gameObjectId ).drawDebugColor = Color.Yellow;
				}

				if ( HasComponentData<SoundComponentData>( gameObjectId ) )
				{
					SoundComponentData soundData = GetComponentData<SoundComponentData>( gameObjectId );
					SoundComponentSystem.Play( gameObjectId, soundData );
				}
			}
		}

		if ( !IsAlive( lifetimeData ) )
		{
			if ( Input.IsButtonPressed( "fire1", 0 ) )
			{
				lifetimeData.Life = 10;
			}
		}
	}

	public bool IsAlive( LifetimeComponentData data )
	{
		return data.Life > 0;
	}
}
