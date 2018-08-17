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
    public class RemovingMatchSystem : ComponentSystem
    {
        public static Position2D pstn;
        public static Match3Settings Settings;

        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<Position2D> Position;
        }
        [Inject] private PlayerData m_Players;

        protected override void OnUpdate()
        {
            var settingsGO = GameObject.Find("Settings");
            Settings = settingsGO?.GetComponent<Match3Settings>();

            List<int> matchesHorizontal = new List<int>();
            List<int> matchesVertical = new List<int>();

            for(int i = 0; i < m_Players.Length; i++)
            {
                if(!Match3Bootstrap.isShifting)
                {
                    matchesHorizontal = findMatchesHorizontal(i);   
                    removeMatchesHorizontal(matchesHorizontal);

                    matchesVertical = findMatchesVertical(i);
                    removeMatchesVertical(matchesVertical);
                }                
            }
        }

        private List<int> findMatchesHorizontal(int a)
        {
            List<int> matchesHorizontal = new List<int>();
            matchesHorizontal.Add(a);
            for(int i = 0; i < m_Players.Length; i++)
            {
                if(Match3Bootstrap.PositionsArr[a].Value.x == Match3Bootstrap.PositionsArr[i].Value.x &&
                    Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.y - Match3Bootstrap.PositionsArr[i].Value.y) < 1.2 &&
                    Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.y - Match3Bootstrap.PositionsArr[i].Value.y) > 0.5 &&
                    Match3Bootstrap.TileSpritesArrPseudo[a] == Match3Bootstrap.TileSpritesArrPseudo[i])
                {                    
                    matchesHorizontal.Add(i);
                }
            }
            return matchesHorizontal;
        }

        private List<int> findMatchesVertical(int a)
        {
            List<int> matchesVertical = new List<int>();
            matchesVertical.Add(a);
            for(int i = 0; i < m_Players.Length; i++)
            {
                if(Match3Bootstrap.PositionsArr[a].Value.y == Match3Bootstrap.PositionsArr[i].Value.y &&
                    Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.x - Match3Bootstrap.PositionsArr[i].Value.x) < 1.2 &&
                    Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.x - Match3Bootstrap.PositionsArr[i].Value.x) > 0.5 &&
                    Match3Bootstrap.TileSpritesArrPseudo[a] == Match3Bootstrap.TileSpritesArrPseudo[i])
                {
                    matchesVertical.Add(i);                    
                }
            }
            return matchesVertical;
        }

        private void removeMatchesHorizontal(List<int> matchesHorizontal)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            float2 flt2 = new float2(-400f, -400f);

            if(matchesHorizontal.Count > 2)
            {
                for(int j = 0; j < matchesHorizontal.Count ; j++)
                {
                    entityManager.DestroyEntity(Match3Bootstrap.Entities[matchesHorizontal[j]]);

                    Match3Bootstrap.Entities[matchesHorizontal[j]] = entityManager.CreateEntity(Match3Bootstrap.TileArchetype);
                    entityManager.SetComponentData(Match3Bootstrap.Entities[matchesHorizontal[j]], Match3Bootstrap.PositionsArr[matchesHorizontal[j]]);
                    entityManager.SetComponentData(Match3Bootstrap.Entities[matchesHorizontal[j]], new PlayerInput{click = flt2});
                    Match3Bootstrap.TileSpritesArr[matchesHorizontal[j]] = null;
                    
                }
                GUIManager.instance.Score += matchesHorizontal.Count*25;
            }
        }

        private void removeMatchesVertical(List<int> matchesVertical)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            float2 flt2 = new float2(-400f, -400f);
            
            if(matchesVertical.Count > 2)
            {
                for(int j = 0; j < matchesVertical.Count ; j++)
                {
                    entityManager.DestroyEntity(Match3Bootstrap.Entities[matchesVertical[j]]);

                    Match3Bootstrap.Entities[matchesVertical[j]] = entityManager.CreateEntity(Match3Bootstrap.TileArchetype);
                    entityManager.SetComponentData(Match3Bootstrap.Entities[matchesVertical[j]], Match3Bootstrap.PositionsArr[matchesVertical[j]]);
                    entityManager.SetComponentData(Match3Bootstrap.Entities[matchesVertical[j]], new PlayerInput{click = flt2});
                    Match3Bootstrap.TileSpritesArr[matchesVertical[j]] = null;
                }
                GUIManager.instance.Score += matchesVertical.Count*25;
            }
        }        
    }
}
