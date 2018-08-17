using UnityEngine;
using Unity.Entities;
using System.Collections;
using System.Collections.Generic;
/*----ECS Specific-----*/
using Unity.Transforms;
using Unity.Transforms2D;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine.SceneManagement;
using Unity.Collections;
using UnityEngine.UI;

namespace Match3GameECS
{
    public class Match3Bootstrap
    {
        public static EntityArchetype TileArchetype;
        public static Match3Settings Settings;

        public static bool isShifting;

        public static toinfiniityandbeyond.Rendering2D.SpriteInstanceRenderer TileLookSprite;       
        public static NativeArray<Entity> Entities;
        public static NativeArray<Position2D> PositionsArr;
        public static List<Texture2D> TileSpritesArr;
        public static List<Texture2D> TileSpritesArrPseudo;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            TileArchetype = entityManager.CreateArchetype(typeof(Position2D), typeof(PlayerInput), typeof(TransformMatrix));
        }

        public static void CreateBoard()
        {
            var gameBoardGO = GameObject.Find("GameBoard");
            if(gameBoardGO == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }

            GameObject NewGameButton = GameObject.Find("NewGameButton");
            //Debug.Log(NewGameButton.gameObject.transform.position);
            NewGameButton.gameObject.transform.Translate(-1000, -1000, -1000);

            GameObject coverbckgrnd = GameObject.Find("BG");
            coverbckgrnd.gameObject.transform.Translate(-1000, -1000, -1000);

            GameObject ExitButton = GameObject.Find("ExitButton");
            ExitButton.gameObject.transform.Translate(-1000, -1000, -1000);
            
		    GameObject gameOverPanel = GameObject.Find("GameOverPanel");
		    gameOverPanel.gameObject.transform.Translate(-1000, -1000, -1000);

            GUIManager.instance.MoveCounter = Settings.MoveCounterNum;
            GUIManager.instance.Score = 0;
            
            isShifting = false;

            int xSize = Settings.xSize;
            int ySize = Settings.ySize;
            
            Texture2D[] previousLeft = new Texture2D[ySize];
		    Texture2D previousBelow = null;

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            Entities = new NativeArray<Entity>(xSize*ySize, Allocator.Temp);
            PositionsArr = new NativeArray<Position2D>(xSize*ySize, Allocator.Temp);
            TileSpritesArr = new List<Texture2D>();
            TileSpritesArrPseudo = new List<Texture2D>();

            float2 flt2 = new float2(-400f, -400f);

            float startX = gameBoardGO.transform.position.x;
            float startY = gameBoardGO.transform.position.y;

            Vector2 offset = Settings.tile.GetComponent<SpriteRenderer>().bounds.size;
            float xOffset = offset.x;
            float yOffset = offset.y;

            int i = 0;
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    Entities[i] = entityManager.CreateEntity(TileArchetype);

                    Position2D positionComponent = new Position2D{Value = new float2(startX + (xOffset * x), startY + (yOffset * y))};                    
                    entityManager.SetComponentData(Match3Bootstrap.Entities[i], positionComponent);
                    PositionsArr[i] = positionComponent;

                    entityManager.SetComponentData(Entities[i], new PlayerInput{click = flt2});

                    List<Texture2D> possibleCharacters = new List<Texture2D>();
                    possibleCharacters.AddRange(Settings.characters);

                    possibleCharacters.Remove(previousLeft[y]);
				    possibleCharacters.Remove(previousBelow);

                    int rndmrange = Random.Range(0, possibleCharacters.Count);

                    Texture2D newSprite = possibleCharacters[rndmrange];
                    TileLookSprite = new toinfiniityandbeyond.Rendering2D.SpriteInstanceRenderer(newSprite, 350, new float2(0f, 0f));                    
				    previousLeft[y] = newSprite;
				    previousBelow = newSprite;
                    TileSpritesArr.Add(newSprite);
                    TileSpritesArrPseudo.Add(newSprite);
                    
                    entityManager.AddSharedComponentData(Entities[i], TileLookSprite);
                    i++;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterSceneLoad()
        {
            var settingsGO = GameObject.Find("Settings");
            if (settingsGO == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            Settings = settingsGO?.GetComponent<Match3Settings>();
            if (!Settings)
                return;
            
            InitializeWithScene();
        }

        public static void InitializeWithScene()
        {
            World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InitializeWithScene();
        }
    }
}