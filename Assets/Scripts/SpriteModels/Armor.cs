using UnityEngine;

[System.Serializable]
public class Armor
{
    // Stand & Injured sprites
    public Sprite standBack;
    public Sprite standFront;
    public Sprite standSide;
    // Move sprites
    public Sprite[] moveBack;
    public Sprite[] moveFront;
    public Sprite[] moveSide;
    // Attack sprites
    public Sprite[] atkBack;
    public Sprite[] atkFront;
    public Sprite[] atkSide;
}