using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Match3GameECS
{
    public class Match3Settings : MonoBehaviour
    {
        public List<Texture2D> characters = new List<Texture2D>();
        public GameObject tile;
        public int xSize, ySize;
        public int MoveCounterNum;

        public bool IsShifting { get; set; }
    }
}