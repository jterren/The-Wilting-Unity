using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public GameObject[] Zones;

    public WorldData(WorldSpace world) =>
        Zones = world.zones;

}