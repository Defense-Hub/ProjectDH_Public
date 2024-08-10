using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] public Unit[] SpawnUnits { get; private set; } = new Unit[2];
    [field: SerializeField] public int UnitCount { get; set; } = 0;
    public int TileNum { get; set; } = 0;
    private LayerMask layerMask;
    private Camera mainCamera;
    private bool isDrag;

    // Attack Range
    public Unit_AttackRange Unit_AttackRange { get; set; }
    public UI_Combination UI_Combination { get; set; }
    public UI_Sell UI_Sell { get; set; }

    // TIle Move Arrow
    private Vector3[] unitPositions = new Vector3[3];
    private LineRenderer arrowLine = null;
    private Vector3 drawStartPos;
    private Collider hitTileCol;
    private const float unitPostiontOffset = 0.25f;
    private const float raycastDistance = 100f;

    // StatusEffect
    private Effect statusEffect;
    private Coroutine statusEffectCoroutine;
    private float statusEffectDuration;

    // Freeze 
    public bool IsFreeze { get; private set; }
    // Lava
    public bool IsLava { get; private set; }

    [SerializeField]private bool IsClick;
    private float timer;
    private readonly float pressTime = 0.5f;
    private UI_UnitStatus unitStatus;
    private void Awake()
    {
        layerMask = LayerMask.GetMask("UnitTile");

        arrowLine = GetComponent<LineRenderer>();

        unitPositions[0] = Vector3.zero;
        unitPositions[1] = new Vector3(-unitPostiontOffset, 0, unitPostiontOffset);
        unitPositions[2] = new Vector3(unitPostiontOffset, 0, -unitPostiontOffset);

        mainCamera = Camera.main;
    }

    public void SetUnit(Unit spawnUnit)
    {
        SpawnUnits[UnitCount] = spawnUnit;
        SetUnitInitPosition();
        UnitCount++;
    }

    private void SetUnitInitPosition() // 초기 위치 세팅
    {
        Vector3 basePosition = transform.position;

        if (UnitCount == 0)
        {
            SpawnUnits[0].transform.position = basePosition + unitPositions[0];
        }
        else
        {
            SpawnUnits[0].StateMachine.CallUnitMove(basePosition + unitPositions[1]);
            SpawnUnits[1].transform.position = basePosition + unitPositions[2];
        }
    }
    
    
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
                IsClick = true;
            }
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsClick)
        {
            ResetClick();
            CloseStatusUI();
        }
    }

    private void CloseStatusUI()
    {
        if (unitStatus != null && unitStatus.gameObject.activeSelf)
        {
            unitStatus.ClosePopupUI();
        }
    }
    private void ResetClick()
    {
        IsClick = false;
        timer = 0f;
    }
    
    private async void Update()
    {
        if (IsClick && timer < pressTime)
        {
            timer += Time.deltaTime;
            
            if (timer > pressTime)
            {
                DeActivateClickUI();
                unitStatus = await UIManager.Instance.ShowPopupUI<UI_UnitStatus>(EUIRCode.UI_UnitStatus);
                unitStatus.UpdateUnitInfo(SpawnUnits[0]);
            }
        }
    }

    private void ActivateClickUI()
    {
        if (UnitCount >= 2 && SpawnUnits[0].DataHandler.Data.UnitRank < EUnitRank.Legendary)
        {
            UI_Combination.SetCombinationUI(this);                    
        }
        UI_Sell.OnSellBtn(SpawnUnits[0].DataHandler.Data.UnitRank, this);
                
        float Range = SpawnUnits[0].StatHandler.CurrentStat.AttackRange;

        Unit_AttackRange.SetAttackRange(transform, Range);
    }
    
    private void DeActivateClickUI()
    {
        UI_Combination.DisableCombinationUI();
        UI_Sell.DisableCombinationUI();
        Unit_AttackRange.gameObject.SetActive(false);
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
                if (IsClick)
                {
                    ResetClick();

                    if (Unit_AttackRange.gameObject.activeSelf)
                    {
                        DeActivateClickUI();
                    }
                    CloseStatusUI();
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        Unit_AttackRange.gameObject.SetActive(false);

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
    }

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

    public void DeActivateUnit()
    {
        if (GameManager.Instance.UnitSpawn.SpawnEnemyID.ContainsKey(SpawnUnits[0].Id))
        {
            GameManager.Instance.UnitSpawn.SpawnEnemyID[SpawnUnits[0].Id] = Mathf.Min(0, GameManager.Instance.UnitSpawn.SpawnEnemyID[SpawnUnits[0].Id]);
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

    private bool CantUnitMove() // 유닛이 움직이는지 판단
    {
        return SpawnUnits[0] != null &&
               (SpawnUnits[0].StateMachine.IsEntityState(SpawnUnits[0].StateMachine.MoveState) ||
                SpawnUnits[0].StatusHandler.IsHardCC);
    }


    #region TileEventCode

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

        if (!IsStatusEffect() && UnitCount == 1)
        {
            GameManager.Instance.UnitSpawn.Controller.TileEvents.ReconstructSpawnUnit(this);
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

    
}