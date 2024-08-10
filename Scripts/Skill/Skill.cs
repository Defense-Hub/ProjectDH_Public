using UnityEngine;

public class Skill : MonoBehaviour, ISkill
{
    public virtual void UseSkill() { }
    public virtual void EndSkill() { }
}
