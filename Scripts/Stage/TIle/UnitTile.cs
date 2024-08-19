using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UnitTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [field: SerializeField] public Unit[] SpawnUnits { get; private set; } = new Unit[2];
    [field: SerializeField] public int UnitCount { get; set; } = 0;
    public int TileNum { get; set; } = 0;

    public void SetUnit(Unit spawnUnit)
    {
        SpawnUnits[UnitCount] = spawnUnit;
        SetUnitInitPosition();
        UnitCount++;
    }

    private void SetUnitInitPosition() // 초기 위치 세팅
    {
        Vector3 basePosition = transform.position;
        SummonEffect summonEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_SummonEffect).ReturnMyComponent<SummonEffect>();
        
        if (UnitCount == 0)
        {
            SpawnUnits[0].transform.position = basePosition + unitPositions[0];
            summonEffect.transform.position = basePosition + unitPositions[0];
            summonEffect.SetParticleColor(SpawnUnits[0].DataHandler.Data.UnitRank);
        }
        else
        {
            SpawnUnits[0].StateMachine.CallUnitMove(basePosition + unitPositions[1]);
            SpawnUnits[1].transform.position = basePosition + unitPositions[2];
            summonEffect.transform.position = basePosition + unitPositions[2];
            summonEffect.SetParticleColor(SpawnUnits[0].DataHandler.Data.UnitRank);
        }
    }

    // 상호작용 EventSystem 함수
    #region Interaction Cooe

    private LayerMask layerMask;
    private Camera mainCamera;
    private bool isDrag;

    // TIle Move Arrow
    private Vector3[] unitPositions = new Vector3[3];
    private LineRenderer arrowLine = null;
    private Vector3 drawStartPos;
    private Collider hitTileCol;
    private const float unitPostiontOffset = 0.25f;
    private const float raycastDistance = 100f;

    public void OnPointerDown(PointerEventData eventData) // 처음 눌렀으 때,
    {
        if (IsFreeze)
        {
            SoundManager.Instance.PlayInGameSfx(EInGameSfx.OnClickIce);
            DeActiveEffect(ECCType.Freeze);
        }
        else
        {
            if (UnitCount > 0 && !CantUnitMove())
            {
                ActivateClickUI();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UnitCount == 0 || CantUnitMove() || IsStatusEffect())
            return;

        isDrag = true;
        drawStartPos = transform.position + (Vector3.up * 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        // TODO : 드래그할 때, 오브젝트 표시 추가해야함.
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
        {
            arrowLine.widthMultiplier = 0.4f;
            if (hit.collider != hitTileCol)
            {
                if (UnitAttackRange.gameObject.activeSelf)
                {
                    DeActivateClickUI();
                }

                hitTileCol = hit.collider;
                DrawArrow(hitTileCol.transform.position);
            }
        }
        else
        {
            arrowLine.widthMultiplier = 0.0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        UnitAttackRange.gameObject.SetActive(false);

        arrowLine.widthMultiplier = 0f;
        hitTileCol = null;

        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
        {
            UnitTile targetTile = hit.collider.GetComponent<UnitTile>();
            if (targetTile == this || targetTile.IsStatusEffect() || targetTile.CantUnitMove() || IsStatusEffect() ||
                CantUnitMove())
            {
                isDrag = false;
                return;
            }

            SwapUnit(targetTile);
        }

        isDrag = false;

        if (UnitStatus != null && UnitStatus.gameObject.activeSelf)
        {
            UnitStatus.gameObject.SetActive(false);
        }
    }

    #endregion 

    // 상호작용에 발생하는 이벤트 
    #region Interaction Event

    public UI_UnitStatus UnitStatus { get; set; }
    public Unit_AttackRange UnitAttackRange { get; set; }
    public UI_Combination UICombination { get; set; }
    public UI_Sell UISell { get; set; }

    // Click Timer
    [SerializeField] private bool IsClick;
    private float timer;
    private readonly float pressTime = 0.5f;
    
    private async void ActivateClickUI()
    {
        if (UnitCount >= 2 && SpawnUnits[0].DataHandler.Data.UnitRank < EUnitRank.Legendary)
        {
            UICombination.SetCombinationUI(this);
        }

        UISell.OnSellBtn(SpawnUnits[0].DataHandler.Data.UnitRank, this);

        float Range = SpawnUnits[0].StatHandler.CurrentStat.AttackRange;

        UnitAttackRange.SetAttackRange(transform, Range);
        
        UnitStatus = await UIManager.Instance.ShowPopupUI<UI_UnitStatus>(EUIRCode.UI_UnitStatus);
        UnitStatus.UpdateUnitInfo(SpawnUnits[0]);
    }

    private void DeActivateClickUI()
    {
        UICombination.DisableCombinationUI();
        UISell.DisableCombinationUI();
        UnitAttackRange.gameObject.SetActive(false);
        
        if (UnitStatus != null && UnitStatus.gameObject.activeSelf)
        {
            UnitStatus.ClosePopupUI();
        }
    }

    private void DrawArrow(Vector3 pointer)
    {
        // CloseStatusUI();
        arrowLine.widthMultiplier = 0.25f; // 라인 너비 설정
        float arrowheadSize = 0.5f; // 화살축 크기 설정
        pointer.y = drawStartPos.y; // 시작 지점의 y값과 같도록 저장

        // 화살표 길이에 비례한 화살촉의 크기 계산
        float percentSize = (float)(arrowheadSize / Vector3.Distance(drawStartPos, pointer));

        // LineRenderer의 점 개수를 4개로 설정
        arrowLine.positionCount = 4;
        // 화살표 시작점을 drawStartPos로 설정
        arrowLine.SetPosition(0, drawStartPos);
        // 화살표의 끝에서 화살촉 부분을 제외한 점 설정
        arrowLine.SetPosition(1, Vector3.Lerp(drawStartPos, pointer, 0.999f - percentSize));
        // 화살표의 화살촉 부분 시작점을 설정
        arrowLine.SetPosition(2, Vector3.Lerp(drawStartPos, pointer, 1 - percentSize));
        // 화살표의 끝점을 pointer로 설정
        arrowLine.SetPosition(3, pointer);

        // 화살표의 너비 곡선을 설정
        arrowLine.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f), // 시작점의 너비
            new Keyframe(0.999f - percentSize, 0.4f), // 화살촉 시작 전의 너비
            new Keyframe(1 - percentSize, 1f), // 화살촉 시작 너비
            new Keyframe(1 - percentSize, 1f), // 화살촉 너비
            new Keyframe(1, 0f) // 끝점의 너비 (화살촉 끝)
        );
    }

    private bool CantUnitMove() // 유닛이 움직이는지 판단
    {
        return SpawnUnits[0] != null &&
               (SpawnUnits[0].StateMachine.IsEntityState(SpawnUnits[0].StateMachine.MoveState) ||
                SpawnUnits[0].StatusHandler.IsHardCC);
    }
    
    #endregion

    // 유닛 이동 이벤트
    #region Unit Move Event

    public void SwapUnit(UnitTile targetTile) //타일 유닛 교체
    {
        targetTile.UnitMove(this);
        UnitMove(targetTile);

        (SpawnUnits, targetTile.SpawnUnits) = (targetTile.SpawnUnits, SpawnUnits); // Swap (구조 분해 방식)
        (UnitCount, targetTile.UnitCount) = (targetTile.UnitCount, UnitCount); // Swap (구조 분해 방식)
    }

    public void MoveSingleUnitToOriginalTile(UnitTile targetTile) // 한마리 유닛 위치 재 구성
    {
        Vector3 basePosition = targetTile.transform.position;

        // 타겟 타일의 1 번째 유닛, 현재 타일 1 번째 유닛 위치 설정
        SpawnUnits[0].StateMachine.CallUnitMove(basePosition + unitPositions[2]);
        targetTile.SpawnUnits[0].StateMachine.CallUnitMove(basePosition + unitPositions[1]);
        (SpawnUnits[0], targetTile.SpawnUnits[1]) = (targetTile.SpawnUnits[1], SpawnUnits[0]); // Swap
        UnitCount--;
    }

    private void UnitMove(UnitTile targetTile) // 유닛 이동 함수 호출
    {
        if (UnitCount == 0)
            return;

        Vector3 basePosition = targetTile.transform.position;
        if (UnitCount == 1) // 유닛이 1개일 때는 타일 가운대로 이동
        {
            SpawnUnits[0].StateMachine.CallUnitMove(basePosition);
        }
        else // 2마리일 때는 지정된 위치로 이동
        {
            SpawnUnits[0].StateMachine.CallUnitMove(basePosition + unitPositions[1]);
            SpawnUnits[1].StateMachine.CallUnitMove(basePosition + unitPositions[2]);
        }
    }
    

    #endregion
    
    // 유닛 비활성화 이벤트
    #region DeActivate Unit

    public void DeActivateUnit()
    {
        if (GameManager.Instance.UnitSpawn.SpawnEnemyID.ContainsKey(SpawnUnits[0].Id))
        {
            GameManager.Instance.UnitSpawn.SpawnEnemyID[SpawnUnits[0].Id] =
                Mathf.Min(0, GameManager.Instance.UnitSpawn.SpawnEnemyID[SpawnUnits[0].Id]);
        }

        GameManager.Instance.UnitSpawn.CheckMaterialUnit(SpawnUnits[UnitCount - 1].Id, false);

        SpawnUnits[UnitCount - 1].DisableUnit();
        SpawnUnits[UnitCount - 1] = null;
        UnitCount--;

        // 유닛이 1마리 남아있으면 위치 재구성
        if (UnitCount == 1)
        {
            GameManager.Instance.UnitSpawn.Controller.TileEvents.ReconstructSpawnUnit(this);
        }
        else // 아예 없다면 소환 버튼 언블록 시도
        {
            CallUnBlockSummonBtn();
        }
    }

    public void CombinationUnit()
    {
        for (int i = 0; i < UnitCount; i++)
        {
            if (GameManager.Instance.UnitSpawn.SpawnEnemyID.ContainsKey(SpawnUnits[i].Id))
            {
                GameManager.Instance.UnitSpawn.SpawnEnemyID[SpawnUnits[i].Id]--;
            }

            GameManager.Instance.UnitSpawn.CheckMaterialUnit(SpawnUnits[i].Id, false);

            SpawnUnits[i].gameObject.SetActive(false);
            SpawnUnits[i] = null;
        }

        UnitCount = 0;
    }

    public void CallUnBlockSummonBtn()
    {
        if (GameManager.Instance.UnitSpawn.Controller.IsFullTile)
        {
            GameManager.Instance.UnitSpawn.Controller.CallUnBlockSummonBtn();
        }
    }

    #endregion
    
    // 타일 상태 이상 이벤트
    #region Tile Status Event Code
    
    // StatusEffect
    private Effect statusEffect;
    private Coroutine statusEffectCoroutine;
    private float statusEffectDuration;

    // Freeze 
    public bool IsFreeze { get; private set; }
    // Lava
    public bool IsLava { get; private set; }
    
    public void SetStatutEffectUnitTile(TileStatusEffectInfo effectInfo) // 타일 이벤트
    {
        statusEffectDuration = effectInfo.activeDuration;

        if (statusEffectCoroutine != null)
        {
            StopCoroutine(statusEffectCoroutine);
        }

        statusEffectCoroutine = StartCoroutine(StatusEffectCoroutine(effectInfo, EffectCallBack));
    }

    // 전조 이펙투 후 본 이펙트 효과 적용
    private IEnumerator StatusEffectCoroutine(TileStatusEffectInfo effectInfo, Action<ECCType> callback)
    {
        // 전조 이펙트
        ApplyEffect((int)effectInfo.eBeginEffect);

        float curTime = effectInfo.preparationTime;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }

        statusEffect.gameObject.SetActive(false);

        callback?.Invoke(effectInfo.eAfterEffect);
    }

    private void EffectCallBack(ECCType type) // 이펙트 적용 콜백 함수
    {
        SetStatusFlag(type, true);

        ApplyEffect((int)type);

        if (CantUnitMove()) // 이동 중이라면 다른 자리로 이동
        {
            UnitTile tile = GameManager.Instance.UnitSpawn.Controller.GetAvailableRandomUnitTile();

            if (tile != null)
            {
                SwapUnit(tile);
                return;
            }
        }

        CCInfo ccInfo = new CCInfo()
        {
            duration = statusEffectDuration,
            ccType = type,
            callBack = () =>
            {
                if (type == ECCType.Freeze)
                {
                    SetStatusFlag(type, false);
                }
            },
        };

        for (int i = 0; i < UnitCount; i++)
        {
            if (SpawnUnits[i] == null)
                return;

            SpawnUnits[i].StateMachine.ChangeHardCCState(ccInfo); // CC 적용
        }
    }

    private void SetStatusFlag(ECCType type, bool isStatus) // 상태에 따른 Bool 값 설정
    {
        if (IsStatusEffect() && !statusEffect.gameObject.activeSelf)
            return;

        if (type == ECCType.Freeze)
        {
            IsFreeze = isStatus;
        }
        else if (type == ECCType.Lava)
        {
            IsLava = isStatus;
        }

        if (!IsStatusEffect())
        {
            if (UnitCount == 0)
            {
                CallUnBlockSummonBtn();
            }
            else if(UnitCount == 1)
            {
                GameManager.Instance.UnitSpawn.Controller.TileEvents.ReconstructSpawnUnit(this);
            }
        }

        statusEffect.gameObject.SetActive(false);
    }

    private void SetUnitIdleState() // 유닛 상태 Idle로 변경
    {
        for (int i = UnitCount - 1; i >= 0; i--)
        {
            if (SpawnUnits[i] == null)
                return;

            SpawnUnits[i].StateMachine.ForceIdleChange();
        }
    }

    private void ApplyEffect(int effectType)
    {
        if (statusEffect != null && statusEffect.gameObject.activeSelf)
        {
            statusEffect.gameObject.SetActive(false);
        }

        statusEffect = GameManager.Instance.Pool.SpawnFromPool(effectType)
            .ReturnMyComponent<Effect>();
        statusEffect.transform.position = transform.position;
    }

    public void DeActiveEffect(ECCType type)
    {
        SetUnitIdleState();
        SetStatusFlag(type, false);
    }

    public bool IsStatusEffect()
    {
        return IsLava || IsFreeze;
    }

    #endregion


    private void Awake()
    {
        layerMask = LayerMask.GetMask("UnitTile");

        arrowLine = GetComponent<LineRenderer>();

        unitPositions[0] = Vector3.zero;
        unitPositions[1] = new Vector3(-unitPostiontOffset, 0, unitPostiontOffset);
        unitPositions[2] = new Vector3(unitPostiontOffset, 0, -unitPostiontOffset);

        mainCamera = Camera.main;
    }
}