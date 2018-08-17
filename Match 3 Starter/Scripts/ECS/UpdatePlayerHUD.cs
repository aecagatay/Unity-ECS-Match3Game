using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Match3GameECS
{
    public class UpdatePlayerHUD : ComponentSystem
    {
        public Button NewGameButton;

        protected override void OnUpdate()
        {
            
        }
        public void SetupGameObjects()
        {
            NewGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
            NewGameButton.onClick.AddListener(Match3Bootstrap.CreateBoard);
        }
    }
}

