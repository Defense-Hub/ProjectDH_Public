using System;
using UnityEngine;
public class Meteor : PoolObject
{
    public Action<UnitTile> OnMeteorHitUnit;

    private float meteorSpeed;
    private UnitTile curTile;
    private Effect meteorDelayEffect;

    private void Update()
    {
        transform.Translate(meteorSpeed * Time.deltaTime * Vector3.down, Space.World);
        if (transform.position.y <= 0.2f)
        {
            gameObject.SetActive(false);
            meteorDelayEffect.gameObject.SetActive(false);
            CallMeteorHitUnit(curTile);
            OnMeteorHitUnit = null;
        }
    }
    
    public void SetMeteorInfo(float speed, UnitTile tile, Effect meteorEffect)
    {
        meteorSpeed = speed;
        curTile = tile;
        meteorDelayEffect = meteorEffect;
    }

    public void CallMeteorHitUnit(UnitTile tile)
    {
        OnMeteorHitUnit?.Invoke(tile);
    }
}
