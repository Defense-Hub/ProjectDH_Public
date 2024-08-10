using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : Effect
{
    [SerializeField] private float duration;

    private UnitTile targetTile;
    
    public void Init(UnitTile tile)
    {
        targetTile = tile;
        
    }
}
