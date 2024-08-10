using GoogleSheet.Core.Type;

[UGS(typeof(EUnitRank))]
public enum EUnitRank
{
    Default = -1,
    Common, // 일반
    Rare, // 레어
    Unique, // 유니크
    Legendary, // 레전더리
    Epic, // 에픽
    Transcendent // 초월
}

[UGS(typeof(EAttackType))]
public enum EAttackType
{
    Default,
    Melee, // 근거리
    Ranged // 원거리
}

[UGS(typeof(EStatChangeType))]
public enum EStatChangeType
{
    Add, // 값 추가
    Multiple, // 값 비율로 곱셈
    Override, // 값 덮어쓰기
}
