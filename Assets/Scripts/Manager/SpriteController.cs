using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer helmetRenderer;
    [SerializeField] private SpriteRenderer armorRenderer;
    [SerializeField] private SpriteRenderer legArmorRenderer;

    [Header("Sprite List")]
    [SerializeField] private List<Helmet> helmetList;
    [SerializeField] private List<Armor> armorList;
    [SerializeField] private List<LegArmor> legArmorList;

    private int currentHelmet = 0;
    private int currentArmor = 0;
    private int currentLegArmor = 0;

    private Armor setArmor;
    private LegArmor setLegArmor;
    private Helmet setHelmet;

    #region Change Helmet
    public void NextHelmet()
    {
        if (helmetList.Count == 0) return; // tránh lỗi nếu list rỗng
        currentHelmet = (currentHelmet + 1) % helmetList.Count;
    }

    public void PrevHelmet()
    {
        if (helmetList.Count == 0) return;
        currentHelmet = (currentHelmet - 1 + helmetList.Count) % helmetList.Count;
    }
    #endregion Change Helmet
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
        setHelmet = helmetList[currentHelmet];
    }

    #region Update Sprite
    #region Stand
    public void StandFront()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.front;
        armorRenderer.sprite = setArmor.standFront;
        legArmorRenderer.sprite = setLegArmor.standFront;
    }
    public void StandBack()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.back;
        armorRenderer.sprite = setArmor.standBack;
        legArmorRenderer.sprite = setLegArmor.standBack;
    }
    public void StandSide()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.side;
        armorRenderer.sprite = setArmor.standSide;
        legArmorRenderer.sprite = setLegArmor.standSide;
    }
    #endregion Stand
    #region Move
    //Move Front
    public void MoveFrontFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.front;
        armorRenderer.sprite = setArmor.moveFront[0];
        legArmorRenderer.sprite = setLegArmor.moveFront[0];
    }
    public void MoveFrontFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.front;
        armorRenderer.sprite = setArmor.moveFront[1];
        legArmorRenderer.sprite = setLegArmor.moveFront[1];
    }
    //Move Back
    public void MoveBackFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.back;
        armorRenderer.sprite = setArmor.moveBack[0];
        legArmorRenderer.sprite = setLegArmor.moveBack[0];
    }
    public void MoveBackFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.back;
        armorRenderer.sprite = setArmor.moveBack[1];
        legArmorRenderer.sprite = setLegArmor.moveBack[1];
    }
    //Move Side
    public void MoveSideFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.side;
        armorRenderer.sprite = setArmor.moveSide[0];
        legArmorRenderer.sprite = setLegArmor.moveSide[0];
    }
    public void MoveSideFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.side;
        armorRenderer.sprite = setArmor.moveSide[1];
        legArmorRenderer.sprite = setLegArmor.moveSide[1];
    }
    #endregion Move
    #region Attack
    //Attack Front
    public void AtkFrontFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.front;
        armorRenderer.sprite = setArmor.atkFront[0];
        legArmorRenderer.sprite = setLegArmor.atkFront[0];
    }
    public void AtkFrontFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.front;
        armorRenderer.sprite = setArmor.atkFront[1];
        legArmorRenderer.sprite = setLegArmor.atkFront[1];
    }
    //Attack Back
    public void AtkBackFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.back;
        armorRenderer.sprite = setArmor.atkBack[0];
        legArmorRenderer.sprite = setLegArmor.atkBack[0];
    }
    public void AtkBackFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.back;
        armorRenderer.sprite = setArmor.atkBack[1];
        legArmorRenderer.sprite = setLegArmor.atkBack[1];
    }
    //Attack Side
    public void AtkSideFrame0()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.side;
        armorRenderer.sprite = setArmor.atkSide[0];
        legArmorRenderer.sprite = setLegArmor.atkSide[0];
    }
    public void AtkSideFrame1()
    {
        UpdateSpriteID();
        helmetRenderer.sprite = setHelmet.side;
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