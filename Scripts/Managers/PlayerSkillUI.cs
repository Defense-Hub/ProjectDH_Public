using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    [SerializeField] private Button prevSkillBtn;
    [SerializeField] private Button curSkillBtn;
    [SerializeField] private Button postSkillBtn;

    [SerializeField] private Image curSkillIcon;
    [SerializeField] private Image prevSkillIcon;
    [SerializeField] private Image postSkillIcon;

    [SerializeField] private Sprite nullSkillIcon;

    private Coroutine UICoroutine;

    public void Init()
    {
        prevSkillBtn.onClick.RemoveAllListeners();
        postSkillBtn.onClick.RemoveAllListeners();

        prevSkillBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PrevSkill);
        postSkillBtn.onClick.AddListener(GameManager.Instance.PlayerSkill.PostSkill);
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
            prevSkillIcon.sprite = nullSkillIcon;
        else
            prevSkillIcon.sprite = GameManager.Instance.PlayerSkill.GetPrevSkillIcon();

        if (GameManager.Instance.PlayerSkill.GetPostSkillIcon() == null)
            postSkillIcon.sprite = nullSkillIcon;
        else
            postSkillIcon.sprite = GameManager.Instance.PlayerSkill.GetPostSkillIcon();
    }
}
