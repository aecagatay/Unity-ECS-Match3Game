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
    public class FillingGapsSystem : ComponentSystem
    {
        public static Position2D pstn;
        public static Texture2D txtr2d;
        public static Match3Settings Settings;

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
                fillGaps(i);
            }
        }

        private void fillGaps(int i)
        {
            bool convertPseudo = false;

            if(Match3Bootstrap.TileSpritesArr[i] == null)
            {
                for(int j = 0; j < m_Players.Length; j++)
                {
                    if((Match3Bootstrap.PositionsArr[j].Value.x == (Match3Bootstrap.PositionsArr[i].Value.x)) &&
                        (Match3Bootstrap.PositionsArr[j].Value.y > (Match3Bootstrap.PositionsArr[i].Value.y)) && 
                        Match3Bootstrap.TileSpritesArr[j] != null)
                    {
                        pstn = Match3Bootstrap.PositionsArr[i];
                        Match3Bootstrap.PositionsArr[i] = Match3Bootstrap.PositionsArr[j];
                        Match3Bootstrap.PositionsArr[j] = pstn;

                        Match3Bootstrap.isShifting = true;
                    }
                    else if((Match3Bootstrap.PositionsArr[j].Value.x == (Match3Bootstrap.PositionsArr[i].Value.x)) &&
                            !(Match3Bootstrap.PositionsArr[j].Value.y > (Match3Bootstrap.PositionsArr[i].Value.y)) 
                            )
                    {
                        convertPseudo = true;
                    }
                }
                if(convertPseudo)
                {
                    Match3Bootstrap.TileSpritesArrPseudo[i] = null;
                }
            }
            Match3Bootstrap.isShifting = false;                      
        }            
    }
}
