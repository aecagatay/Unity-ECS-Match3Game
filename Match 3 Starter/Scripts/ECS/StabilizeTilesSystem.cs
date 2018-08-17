using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Match3GameECS
{
    public class StabilizeTilesSystem : ComponentSystem
    {     
        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<Position2D> Position;
        }
        [Inject] private PlayerData m_Players;

        protected override void OnUpdate()
        { 
            var Settings = Match3Bootstrap.Settings;

            int xSize = Settings.xSize;
            int ySize = Settings.ySize;

            var gameBoardGO = GameObject.Find("GameBoard");
            if(gameBoardGO != null)
            {
                float startX = gameBoardGO.transform.position.x;
                float startY = gameBoardGO.transform.position.y;
            }            

            Vector2 offset = Settings.tile.GetComponent<SpriteRenderer>().bounds.size;
            float xOffset = offset.x;
            float yOffset = offset.y;

            //if(Input.GetMouseButtonDown(0))
                //Debug.Log("Offset: " + xOffset + ", " + yOffset);

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            for (int i = 0; i < m_Players.Length; i++) {
                Position2D positionComponent = Match3Bootstrap.PositionsArr[i];                    
                entityManager.SetComponentData(Match3Bootstrap.Entities[i], positionComponent);

                //if(Input.GetMouseButtonDown(0))
                    //  Debug.Log("Position: " + positionComponent.Value.x + ", " + positionComponent.Value.y);                
            }
        }
    }

}