using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PowerUp : ScriptableObject
{
    public string boostName;
    public int maxAmount = 4;
    public int boostPrices;
}
