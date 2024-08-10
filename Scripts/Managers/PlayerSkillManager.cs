using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    // 좌측
    public Stack<ActiveSkill> PrevSkillStack { get; private set; }

    // 우측
    public Stack<ActiveSkill> PostSkillStack { get; private set; }

    // 가운데 ( 현재 스킬 )
    public ActiveSkill CurActiveSkill { get; private set; }

    [SerializeField] private PlayerSkillUI SkillUI;

    [Header("# DEV ONlY")]
    public int PrevCnt;
    public int PostCnt;

    private void Start()
    {
        GameManager.Instance.PlayerSkill = this;
        CurActiveSkill = new ActiveSkill();
        PrevSkillStack = new Stack<ActiveSkill>();
        PostSkillStack = new Stack<ActiveSkill>();
    }

    private void Update()
    {
        PrevCnt = PrevSkillStack.Count;
        PostCnt = PostSkillStack.Count;
    }

    public void ActiveSkill()
    {
        if (CurActiveSkill == null) return;
        CurActiveSkill.UseSkill();

        // 스킬 사용 이후 현재 slot에 있는 ActiveSkill를 CurSkill로 전환
        if(PrevSkillStack.Count != 0)
            CurActiveSkill = PrevSkillStack.Pop();
        else if(PostSkillStack.Count != 0)
            CurActiveSkill = PostSkillStack.Pop();
        else CurActiveSkill = null;

        // UI 업데이트
        SkillUI.UpdateUI();
    }

    // Unit이 생성될 때 ActiveSkill이 있다면 전부 AddSkill
    public void AddSkill(ActiveSkill newSkill)
    {
        Debug.Log("AddSkill");
        // 기존 Skill을 Prev로 옮기고
        if (CurActiveSkill != null)
            PrevSkillStack.Push(CurActiveSkill);
        // CurActiveSkill이 비어있는 최초 상태인 경우
        else
        {
            // 최초 생성
            SkillUI.gameObject.SetActive(true);
            SkillUI.Init();
        }

        // 기존 Skill 자리에 신규 스킬 등록
        CurActiveSkill = newSkill;

        // UI 업데이트
        SkillUI.UpdateUI();
    }

    // 다음 스킬 (우측에 있는 Skill 불러오기)
    public void PostSkill()
    {
        // UI 단계에서도 위 조건이 만족 안된다면 비활성화
        if(!CanPostSkill()) return;

        PrevSkillStack.Push(CurActiveSkill);
        CurActiveSkill = PostSkillStack.Pop();

        // UI 업데이트
        SkillUI.UpdateUI();
    }


    public void PrevSkill()
    {
        if (!CanPrevSkill()) return;

        PostSkillStack.Push(CurActiveSkill);
        CurActiveSkill = PrevSkillStack.Pop();

        // UI 업데이트
        SkillUI.UpdateUI();
    }

    public bool CanPostSkill()
    {
        return CurActiveSkill != null && PostSkillStack.Count !=0;
    }

    public bool CanPrevSkill()
    {
        return CurActiveSkill != null && PrevSkillStack.Count != 0;
    }

    // UI //
    public Sprite GetCurSkillIcon()
    {
        if (CurActiveSkill == null) return null;
        else return CurActiveSkill.Icon;
    }

    public Sprite GetPostSkillIcon() 
    {
        if (PostSkillStack.Count == 0) return null;
        else return PostSkillStack.Peek().Icon;
    }

    public Sprite GetPrevSkillIcon()
    {
        if (PrevSkillStack.Count == 0) return null;
        else return PrevSkillStack.Peek().Icon;
    }
}
