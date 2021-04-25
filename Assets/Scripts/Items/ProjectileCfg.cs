using UnityEngine;
using game.Objects;

[CreateAssetMenu(fileName = "New Proj Cfg", menuName = "Items/Projectiles", order = 1)]
public class ProjectileCfg : ScriptableObject
{
    public float DPS = 1f;
    public int DMG = 1;
    public Sprite Texture;
}