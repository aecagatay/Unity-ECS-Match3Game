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
    public class PlayerInputSystem : ComponentSystem
    {
        public static Position2D pstn;
        public static toinfiniityandbeyond.Rendering2D.SpriteInstanceRenderer TempTileSprite;
        public static Match3Settings Settings;

        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<PlayerInput> Input;
        }
        [Inject] private PlayerData m_Players;

        protected override void OnUpdate()
        {
            if(Input.GetMouseButtonDown(0))
            {
                SFXManager.instance.PlaySFX(Clip.Select);
                for (int i = 0; i < m_Players.Length; i++)
                {                                    
                    int index = UpdatePlayerInput(i);
                    if(index == -1)
                    {
                        //do nothing
                    }
                    else if(index >= 0)
                    {
                        SwapTiles(index);
                        break;
                    }
                    else if(index == -2)
                    {
                        break;
                    }
                }
            }
        }

        private int UpdatePlayerInput(int a)
        {
            PlayerInput pi = new PlayerInput();
            pi.click.x = -500f;
            pi.click.y = -500f;

            int index = -1;

            Camera c = Camera.main;
            Vector2 mousePos = new Vector2();                                              
            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;
            Vector3 vctr3 = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));
            pi.click.x = vctr3.x - 0.3f;
            pi.click.y = vctr3.z - 0.3f;             

            //Debug.Log("" + pi.click.y);

            if(pi.click.y < -4.35)
            {
                return -2;
            }
            if(m_Players.Input[a].click.x == -400f && m_Players.Input[a].click.y == -400f)
            {
                m_Players.Input[a] = pi;
                return -1;
            }
            else
            {
                if(Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.x - pi.click.x) < 0.4 && 
                    Mathf.Abs(Match3Bootstrap.PositionsArr[a].Value.y - pi.click.y) < 0.4)
                {
                    index = a;
                    return index;
                }
                else
                {
                    return -1;
                }
            }
        }

        private void SwapTiles(int index)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            int i = 0;

            for (i = 0; i < m_Players.Length; i++)
            {
                if(Mathf.Abs(Match3Bootstrap.PositionsArr[i].Value.x - m_Players.Input[i].click.x) < 0.4 && 
                    Mathf.Abs(Match3Bootstrap.PositionsArr[i].Value.y - m_Players.Input[i].click.y) < 0.4)
                {
                    break;
                }
            } 

            if((Mathf.Abs(Match3Bootstrap.PositionsArr[i].Value.x - Match3Bootstrap.PositionsArr[index].Value.x) < 1.2 && 
                    Match3Bootstrap.PositionsArr[i].Value.y == Match3Bootstrap.PositionsArr[index].Value.y) 
                ||
                (Mathf.Abs(Match3Bootstrap.PositionsArr[i].Value.y - Match3Bootstrap.PositionsArr[index].Value.y) < 1.2 &&
                    Match3Bootstrap.PositionsArr[i].Value.x == Match3Bootstrap.PositionsArr[index].Value.x)
                )
            {
                pstn = Match3Bootstrap.PositionsArr[i];
                Match3Bootstrap.PositionsArr[i] = Match3Bootstrap.PositionsArr[index];
                Match3Bootstrap.PositionsArr[index] = pstn;
                GUIManager.instance.MoveCounter--;

                PlayerInput pi = new PlayerInput{click = new float2(-400f, -400f)};
                for (int j = 0; j < m_Players.Length; j++)
                {
                    m_Players.Input[j] = pi;
                }
            }
            else
            {
                PlayerInput pi = new PlayerInput();

                Camera c = Camera.main;
                Vector2 mousePos = new Vector2();                                              
                mousePos.x = Input.mousePosition.x;
                mousePos.y = Input.mousePosition.y;
                Vector3 vctr3 = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));
                pi.click.x = vctr3.x - 0.3f;
                pi.click.y = vctr3.z - 0.3f;
                for (int j = 0; j < m_Players.Length; j++)
                {
                    m_Players.Input[j] = pi;
                }
            }
        }
    }
}
