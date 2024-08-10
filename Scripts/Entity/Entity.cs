using UnityEngine;

public class Entity : PoolObject
{
    [field: SerializeField] public AnimationData AnimationData { get; private set; }
    [field: SerializeField] public Transform EffectWayPoint { get; private set; }
    public Animator Animator { get; private set; }
    public StatusHandler StatusHandler { get; private set;}

    protected virtual void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        StatusHandler = gameObject.AddComponent<StatusHandler>();
    }
}
