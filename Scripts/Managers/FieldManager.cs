using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffInfo
{
    public BuffInfo()
    {

    }

    public BuffInfo(int count, BuffSO buffSO)
    {
        Count = count;
        BuffSO = buffSO;
    }

    // Count가 -될 때, Count가 0일 경우 buff 삭제
    public int Count { get; set; }
    public BuffSO BuffSO { get; set; }
}

public class FieldManager : MonoBehaviour
{
    //<id, BuffInfo>
    public Dictionary<int, BuffInfo> BuffDict { get; private set; }
    public event Action OnBuffChange;

    private void Start()
    {
        GameManager.Instance.FieldManager = this;
        BuffDict = new Dictionary<int, BuffInfo>();
    }

    public void AddBuff(BuffSO buff)
    {
        // buff가 현재 Dictionary에 있는지 체크
        if (BuffDict.ContainsKey(buff.Id))
        {
            // 있다면 id에 해당하는 buffCount만 추가
            BuffDict[buff.Id].Count++;
        }
        else
        {
            // 없다면 Dictionary에 key,Value 추가
            BuffInfo newBuff = new BuffInfo(1, buff);
            BuffDict.Add(buff.Id, newBuff);
            CallUpdateBuff();
        }
    }

    public void RemoveBuff(BuffSO buff) 
    {
        // 혹시 모를 방어 코드
        if (!BuffDict.ContainsKey(buff.Id)) return;
        
        BuffDict[buff.Id].Count--;
        
        // 남아 있는 BuffUnit이 없다면
        if (BuffDict[buff.Id].Count == 0) 
        {
            // 전체 필드에 해당 버프 해제
            BuffDict.Remove(buff.Id);
            CallUpdateBuff();
        }
    }

    private void CallUpdateBuff()
    {
        OnBuffChange?.Invoke();
    }
}
