using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private BuffSO buffSO;
    private void OnEnable()
    {
        AddBuff();
    }

    private void OnDisable()
    {
        RemoveBuff();
    }

    private void AddBuff()
    {
        GameManager.Instance.FieldManager.AddBuff(buffSO);
    }

    private void RemoveBuff()
    {
        GameManager.Instance.FieldManager.RemoveBuff(buffSO);
    }
}