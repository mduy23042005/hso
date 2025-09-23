using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armorRenderer;
    [SerializeField] private List<Armor> armorList;
    private int currentArmor = 0;

    [SerializeField] private SpriteRenderer legArmorRenderer;
    [SerializeField] private List<LegArmor> legArmorList;
    private int currentLegArmor = 0;

    private Armor setArmor;
    private LegArmor setLegArmor;
    #region Change Armor
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
    #endregion Change Armor
    #region Change Leg Armor
    public void NextLegArmor()
    {
        if (legArmorList.Count == 0) return; // tránh lỗi nếu list rỗng
        currentLegArmor = (currentLegArmor + 1) % legArmorList.Count;
    }

    public void PrevLegArmor()
    {
        if (legArmorList.Count == 0) return;
        currentLegArmor = (currentLegArmor - 1 + legArmorList.Count) % legArmorList.Count;
    }
    #endregion Change Leg Armor

    private void UpdateSpriteID()
    {
        setArmor = armorList[currentArmor];
        setLegArmor = legArmorList[currentLegArmor];
    }

    #region Update Sprite
    #region Stand
    public void StandFront()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.standFront;
        legArmorRenderer.sprite = setLegArmor.standFront;
    }
    public void StandBack()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.standBack;
        legArmorRenderer.sprite = setLegArmor.standBack;
    }
    public void StandSide()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.standSide;
        legArmorRenderer.sprite = setLegArmor.standSide;
    }
    #endregion Stand
    #region Move
    //Move Front
    public void MoveFrontFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveFront[0];
        legArmorRenderer.sprite = setLegArmor.moveFront[0];
    }
    public void MoveFrontFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveFront[1];
        legArmorRenderer.sprite = setLegArmor.moveFront[1];
    }
    //Move Back
    public void MoveBackFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveBack[0];
        legArmorRenderer.sprite = setLegArmor.moveBack[0];
    }
    public void MoveBackFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveBack[1];
        legArmorRenderer.sprite = setLegArmor.moveBack[1];
    }
    //Move Side
    public void MoveSideFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveSide[0];
        legArmorRenderer.sprite = setLegArmor.moveSide[0];
    }
    public void MoveSideFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.moveSide[1];
        legArmorRenderer.sprite = setLegArmor.moveSide[1];
    }
    #endregion Move
    #region Attack
    //Attack Front
    public void AtkFrontFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkFront[0];
        legArmorRenderer.sprite = setLegArmor.atkFront[0];
    }
    public void AtkFrontFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkFront[1];
        legArmorRenderer.sprite = setLegArmor.atkFront[1];
    }
    //Attack Back
    public void AtkBackFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkBack[0];
        legArmorRenderer.sprite = setLegArmor.atkBack[0];
    }
    public void AtkBackFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkBack[1];
        legArmorRenderer.sprite = setLegArmor.atkBack[1];
    }
    //Attack Side
    public void AtkSideFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkSide[0];
        legArmorRenderer.sprite = setLegArmor.atkSide[0];
    }
    public void AtkSideFrame1()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.atkSide[1];
        legArmorRenderer.sprite = setLegArmor.atkSide[1];
    }
    #endregion Attack
    #region Die
    public void DieFrame0()
    {
        UpdateSpriteID();
        armorRenderer.sprite = setArmor.die;
        legArmorRenderer.sprite = null;
    }
    #endregion Die
    #endregion Update Sprite
}