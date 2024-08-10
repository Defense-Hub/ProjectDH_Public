using UnityEngine;

public class Unit_AttackRange : MonoBehaviour
{
    public void SetAttackRange(Transform tile, float range)
    {
        gameObject.SetActive(true);
        transform.position = tile.position + Vector3.up * 0.5f;

        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

        //공격 사거리에 따라 크기 조절
        float circleScale = range * 2;
        transform.localScale = new Vector3(circleScale, circleScale, circleScale);
    }
}