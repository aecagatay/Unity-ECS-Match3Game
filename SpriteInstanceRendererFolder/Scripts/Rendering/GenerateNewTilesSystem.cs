using UnityEngine;
using Unity.Entities;
/*----ECS Specific-----*/
using Unity.Transforms;
using Unity.Transforms2D;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Match3GameECS
{
    public class GenerateNewTilesSystem : ComponentSystem
    {
        public static Position2D pstn;
        public static Texture2D txtr2d;
        public static Match3Settings Settings;
        public static toinfiniityandbeyond.Rendering2D.SpriteInstanceRenderer TileLookSprite;  

        struct PlayerData
        {
            public readonly int Length;
        }
        [Inject] private PlayerData m_Players;

        protected override void OnUpdate()
        {
            var settingsGO = GameObject.Find("Settings");
            Settings = settingsGO?.GetComponent<Match3Settings>();
            
            for(int i = 0; i < m_Players.Length; i++)
            {
                GenerateNewTiles(i);
            }
        }

        private void GenerateNewTiles(int i)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            if(Match3Bootstrap.TileSpritesArrPseudo[i] == null)
            {
                List<Texture2D> possibleCharacters = new List<Texture2D>();
                possibleCharacters.AddRange(Settings.characters);

                int rndmrange = Random.Range(0, possibleCharacters.Count);
                Texture2D newSprite = possibleCharacters[rndmrange];
                
                TileLookSprite = new toinfiniityandbeyond.Rendering2D.SpriteInstanceRenderer(newSprite, 350, new float2(0f, 0f));                    
				Match3Bootstrap.TileSpritesArr[i] = newSprite;
                Match3Bootstrap.TileSpritesArrPseudo[i] = newSprite;    
                
                entityManager.AddSharedComponentData(Match3Bootstrap.Entities[i], TileLookSprite);
            }                  
        }            
    }
}
