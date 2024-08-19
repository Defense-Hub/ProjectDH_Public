using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    [SerializeField] private Button prevSkillBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button curSkillBtn;
    [SerializeField] private Button postSkillBtn;
    [SerializeField] private Button postBtn;

    [SerializeField] private Image curSkillIcon;
    [SerializeField] private Image prevSkillIcon;
    [SerializeField] private Image postSkillIcon;

    [SerializeField] private Sprite nullSkillIcon;

    private Coroutine UICoroutine;

    public void Init()
    {
        prevSkillBtn.onClick.RemoveAllListeners();
        postSkillBtn.onClick.RemoveAllListeners();
        prevBtn.onClick.RemoveAllListeners();
        postBtn.onClick.RemoveAllListeners();

        // 반대 방향 입력을 통해 방향에 맞게 이동하도록 설정
        prevSkillBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PrevSkill);
        postSkillBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PostSkill);
        prevBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PostSkill);
        postBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PrevSkill);

    }

    public void UpdateUI()
    {
        if(UICoroutine != null)
        {
            StopCoroutine(UICoroutine);
        }
        UICoroutine = StartCoroutine(UpdateIcon());

        curSkillBtn.onClick.RemoveAllListeners();
        curSkillBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.ActiveSkill);
    }

    public IEnumerator UpdateIcon()
    {
        // 변경사항이 적용되고 한프레임 쉬었다가 UI 업데이트
        yield return null;
        if(GameManager.Instance.PlayerSkill.GetCurSkillIcon() == null)
            curSkillIcon.sprite = nullSkillIcon;
        else
            curSkillIcon.sprite = GameManager.Instance.PlayerSkill.GetCurSkillIcon();

        if (GameManager.Instance.PlayerSkill.GetPrevSkillIcon() == null)
        {
            prevSkillIcon.sprite = nullSkillIcon;
            postBtn.gameObject.SetActive(false);
        }
        else
        {
            prevSkillIcon.sprite = GameManager.Instance.PlayerSkill.GetPrevSkillIcon();
            postBtn.gameObject.SetActive(true);
        }

        if (GameManager.Instance.PlayerSkill.GetPostSkillIcon() == null)
        {
            postSkillIcon.sprite = nullSkillIcon;
            prevBtn.gameObject.SetActive(false);
        }
        else
        {
            postSkillIcon.sprite = GameManager.Instance.PlayerSkill.GetPostSkillIcon();
            prevBtn.gameObject.SetActive(true);
        }
            
    }
}
