using Data;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : Obstacle
{
   
    public override bool Init()
    {
        if (!base.Init())
            return false;

        obstacleType = EObstacleType.Door;


        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_GameScene gameSceneUI = Managers.UI.SceneUI.GetComponent<UI_GameScene>();
            IInventoryItem key = null;
            Inventory inventory = null;
            if (gameSceneUI != null)
            {

                inventory = gameSceneUI.uiInventory.inventory;
                key = inventory.FindItem("Key");
            }
            if(key != null)
            {
                inventory.RemoveItem(key);
                Managers.Game.ClearStage();
               
            }
            
            //Managers.Game.TestStage(nextStage);
        }
    }



}