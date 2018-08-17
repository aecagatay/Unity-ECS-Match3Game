using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms2D;

namespace Match3GameECS
{
    public struct PlayerInput : IComponentData
    {
        public float2 click;
    }
}