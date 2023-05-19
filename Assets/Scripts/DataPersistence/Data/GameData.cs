using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int rock;
    public int crystal;
    public int gold;
    public float hValue;
    public float vValue;
    public int speedBoostAmount;
    public int jumpBoostAmount;
    public float speedMult;
    public float jumpMult;
    public int minimap;
    public int flashlight;
    public Vector3 playerPosition;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        this.rock = 0;
        this.crystal = 0;
        this.gold = 0;
        this.hValue = 0;
        this.speedBoostAmount = 0;
        this.jumpBoostAmount = 0;
        this.speedMult = 5;
        this.jumpMult = 8;
        this.minimap = 0;
        this.flashlight = 0;
        playerPosition = new Vector3(0, 8, 0);
    }
}
