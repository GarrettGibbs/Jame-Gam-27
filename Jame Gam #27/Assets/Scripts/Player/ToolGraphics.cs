using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGraphics : MonoBehaviour
{
    [SerializeField] GameObject[] Mower_Graphics;
    [SerializeField] GameObject[] Blower_Graphics;
    [SerializeField] GameObject[] Shovel_Graphics;

    private GameObject currentGraphic = null;

    public void ChangeToolGraphic(Tools tool, Vector2 direction) {
        if(tool == Tools.None) return;
        int toolIndex = 50;

        if (direction.y > .01) toolIndex = 0; //Up
        else if(direction.y < -.01) toolIndex = 1; //Down
        else if(direction.x > .01) toolIndex = 2; //Right
        else toolIndex = 3; //Left, and default
        
        if(currentGraphic != null) currentGraphic.SetActive(false);
        switch (tool) {
            case Tools.Mower:
                Mower_Graphics[toolIndex].SetActive(true);
                currentGraphic = Mower_Graphics[toolIndex];
                break;
            case Tools.LeafBlower:
                Blower_Graphics[toolIndex].SetActive(true);
                currentGraphic = Blower_Graphics[toolIndex];
                break;
            case Tools.Shovel:
                Shovel_Graphics[toolIndex].SetActive(true);
                currentGraphic = Shovel_Graphics[toolIndex];
                break;
        }
    }
}
