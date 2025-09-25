using UnityEngine;

[System.Serializable]
public class Armor
{
    // Stand & Injured sprites
    public Sprite standFront;
    public Sprite standBack;
    public Sprite standSide;
    // Move sprites
    public Sprite[] moveFront;
    public Sprite[] moveBack;
    public Sprite[] moveSide;
    // Attack sprites
    public Sprite[] atkFront;
    public Sprite[] atkBack;
    public Sprite[] atkSide;
    // Die sprites
    public Sprite die;
}