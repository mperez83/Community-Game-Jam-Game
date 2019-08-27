﻿using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Pit, Pot, TurnUp, TurnDown, TurnLeft, TurnRight, Exit, Ladder, Sword };
    public TileType tileType;
    public bool blocksVision;
}