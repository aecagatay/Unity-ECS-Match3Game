﻿using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;

using toinfiniityandbeyond.Rendering2D;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace toinfiniityandbeyond.Examples
{
    public sealed class SpriteRendererSceneBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterScene()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name != "SpriteRendererScene")
                return;
            
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //Load all the sprites we need
            var animalSprites = new[]
            {
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scenes/Sprite Rendering Example/Sprites/elephant.png"),
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scenes/Sprite Rendering Example/Sprites/giraffe.png"),
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scenes/Sprite Rendering Example/Sprites/zebra.png"),
            };
            //Assign loaded sprites to sprite renderers
            var renderers = new[]
            {
                new SpriteInstanceRenderer(animalSprites[0], animalSprites[0].width, new float2(0.5f, 0.5f)),
                new SpriteInstanceRenderer(animalSprites[1], animalSprites[1].width, new float2(0.5f, 0.5f)),
                new SpriteInstanceRenderer(animalSprites[2], animalSprites[2].width, new float2(0.5f, 0.5f)),
            };
            //Spawn 10,000 entities with random positions/rotations
            for (int i = 0; i < 10000; i++)
            {
                var entity = entityManager.CreateEntity(ComponentType.Create<Position2D>(),
                                                        ComponentType.Create<Heading2D>(),
                                                        ComponentType.Create<TransformMatrix>());

                entityManager.SetComponentData(entity, new Position2D
                {
                    Value = new float2(Random.value * 50, Random.value * 25)
                });

                entityManager.SetComponentData(entity, new Heading2D
                {
                    Value = new float2(Random.value, Random.value)
                });

                entityManager.AddSharedComponentData(entity, renderers[i % 3]);
            }
        }
    }
}