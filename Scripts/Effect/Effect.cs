using System;
using UnityEngine;

public class Effect : PoolObject
{
    public event Action OnEnd;

    public void SetEffect(Vector3 scale, Vector3 position)
    {
        transform.localScale = scale;
        transform.position = position;
    }

    private void OnParticleSystemStopped()
    {
        OnEnd?.Invoke();
    }
}
