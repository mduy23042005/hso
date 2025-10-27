using UnityEngine.U2D.Animation;

[System.Serializable]
public class WeaponLibraries
{
    public int IDWeapon;
    public SpriteLibraryAsset weaponBackLibraries;
    public SpriteLibraryAsset weaponFrontLibraries;
}

[System.Serializable]
public class HelmetLibraries
{
    public int IDHelmet;
    public SpriteLibraryAsset helmetLibrariesAsset;
    public bool isHiddenHair = false;
}

[System.Serializable]
public class ArmorLibraries
{
    public int IDArmor;
    public SpriteLibraryAsset armorLibrariesAsset;
}

[System.Serializable]
public class HeadLibraries
{
    public int IDHead;
    public SpriteLibraryAsset headLibrariesAsset;
}

[System.Serializable]
public class LegArmorLibraries
{
    public int IDLegArmor;
    public SpriteLibraryAsset legArmorLibrariesAsset;
}

[System.Serializable]
public class HairLibraries
{
    public int IDHair;
    public SpriteLibraryAsset hairLibrariesAsset;
}