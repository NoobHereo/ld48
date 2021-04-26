using UnityEngine;
using game.Objects;

[CreateAssetMenu(fileName = "New Pickup Item", menuName = "Items/Pickups", order = 1)]
public class Pickup : ScriptableObject
{
    public string Name;
    public Sprite Texture;
    public PlayerStats ItemStat = PlayerStats.HP_REGEN;
}