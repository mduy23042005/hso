using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armorRenderer;
    [SerializeField] private List<Armor> armorList;

    private int currentArmor = 0;

    public void NextArmor()
    {
        if (armorList.Count == 0) return; // tránh lỗi nếu list rỗng
        currentArmor = (currentArmor + 1) % armorList.Count;
    }

    public void PrevArmor()
    {
        if (armorList.Count == 0) return;
        currentArmor = (currentArmor - 1 + armorList.Count) % armorList.Count;
    }
    //Stand
    public void StandFront()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standFront;
    }
    public void StandBack()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standBack;
    }
    public void StandSide()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.standSide;
    }

    //Move
    //Move Front
    public void MoveFrontFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveFront[0];
    }
    public void MoveFrontFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveFront[1];
    }
    //Move Back
    public void MoveBackFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveBack[0];
    }
    public void MoveBackFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveBack[1];
    }
    //Move Side
    public void MoveSideFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveSide[0];
    }
    public void MoveSideFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.moveSide[1];
    }

    //Attack
    //Attack Front
    public void AtkFrontFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkFront[0];
    }
    public void AtkFrontFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkFront[1];
    }
    //Attack Back
    public void AtkBackFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkBack[0];
    }
    public void AtkBackFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkBack[1];
    }
    //Attack Side
    public void AtkSideFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkSide[0];
    }
    public void AtkSideFrame1()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.atkSide[1];
    }

    //Die
    public void DieFrame0()
    {
        Armor setArmor = armorList[currentArmor];
        armorRenderer.sprite = setArmor.die;
    }
}