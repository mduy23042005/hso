using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armorRenderer;
    [SerializeField] private List<Armor> armorList;
    [SerializeField] private int currentArmor = 0;
    //Stand
    public void StandFront()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standFront;
        Debug.Log(currentArmor);
    }
    public void StandBack()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standBack;
        Debug.Log(currentArmor);
    }
    public void StandSide()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standSide;
        Debug.Log(currentArmor);
    }

    //Move
    //Move Front
    public void MoveFrontFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveFront[0];
        Debug.Log(currentArmor);
    }
    public void MoveFrontFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveFront[1];
        Debug.Log(currentArmor);
    }
    //Move Back
    public void MoveBackFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveBack[0];
        Debug.Log(currentArmor);
    }
    public void MoveBackFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveBack[1];
        Debug.Log(currentArmor);
    }
    //Move Side
    public void MoveSideFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveSide[0];
        Debug.Log(currentArmor);
    }
    public void MoveSideFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveSide[1];
        Debug.Log(currentArmor);
    }
}