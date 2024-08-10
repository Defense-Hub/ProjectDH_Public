using System.Collections.Generic;

[System.Serializable]
public class Dialouge
{
    public string dialogueText;
    public bool isEvent;

    public Dialouge(string str, int isEvent)
    {
        dialogueText = str;
        this.isEvent = isEvent == 1;
    }
}

[System.Serializable]
public class DialogueData
{
    private int dialogueStep = 0; // 대화 단계 
    public List<Dialouge> dialogueList = new List<Dialouge>(); // 단계별 대화를 저장하는 문자열 배열

    public Dialouge GetDialogue() // 대화 반환
    {
        if(dialogueList.Count <= dialogueStep)
        {
            return null;
        }

        return dialogueList[dialogueStep++];
    }

    public void ResetDialogue() // 댜화 단계 초기화
    {
        dialogueStep = 0;
    }

    public bool DialogueComplete() // 대화가 끝났는 지 반환
    {
        return dialogueList.Count <= dialogueStep;
    }

}