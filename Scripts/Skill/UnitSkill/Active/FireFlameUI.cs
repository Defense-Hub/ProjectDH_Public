using UnityEngine;
using UnityEngine.UI;

// TODO : 하위 객체로 묶는 것 생각
public class FireFlameUI : MonoBehaviour
{
    public Button[] targetBtns;

    public void Init(FireFlamethrower activeSkill)
    {
        for (int i = 0; i < targetBtns.Length; i++)
        {
            targetBtns[i].gameObject.SetActive(true);
            targetBtns[i].onClick.RemoveAllListeners();
            // 람다의 경우 미리 넣어만 두고 호출할 때 i값을 기준으로 함수를 불러오기 때문에 idx라는 새 변수에 값을 담아서 저장해둬야 한다.
            int idx = i;
            targetBtns[i].onClick.AddListener(() => activeSkill.ActiveSkill(idx));
        }
    }

    public void Disable()
    {
        Destroy(gameObject);
    }
}
