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
    public float speed;
    public float jumpForce;
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
        this.speed = 24;
        this.jumpForce = 15;
        playerPosition = new Vector3(0, 8, 0);
    }
}
