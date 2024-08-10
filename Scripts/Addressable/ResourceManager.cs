using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager> // 어드레서블에서 Load하는 데이터
{
    [field:Header("# Entity Resources")]
    [field: SerializeField] public List<PoolObject> UnitDataList { get; private set; }
    [field: SerializeField] public List<PoolObject> EnemyDataList { get; private set; }

    [field: Header("# Effect Resources")]
    [field: SerializeField] public List<PoolObject> EffectDataList { get; private set; }

    [field: Header("# Other Resources")]
    [field: SerializeField] public List<PoolObject> OtherDataList { get; private set; }

    [field: Header("# Map Info")]
    public GameObject[ ] LoadMaps;
    public int curMapNum; // 현재 맵 번호
    public int loadMapNum; // 로드된 맵 번호
    public int currentEnemyType; // 현재 테마의 Enemy 타입

    private Dictionary<int, GameObject> uiDictionary = new Dictionary<int, GameObject>();

    public void ResetUI()
    {
        uiDictionary.Clear();
    }

    public async Task<List<PoolObject>> LoadPoolObjectData(EAddressableType type ) // 로드 에셋
    {
        List<PoolObject> poolObjList = new List<PoolObject>();
        // Load
        for (int i = 0; i < AddressableManager.Instance.LocationDict[(int)type].Count; i++)
        {
            var obj = await AddressableManager.Instance.LoadMonoBehaviorAsset<PoolObject>(type, i);
            if (obj != null)
            {
                poolObjList.Add(obj);
            }
            else
            {
                Debug.LogError($"{type} => {i} number of Unit Load Error");
            }
        }

        return poolObjList;
    }
    
    public async Task<GameObject> GetUIGameObject(EUIRCode uiType) // 로드 UI
    {
        GameObject obj;
        int idx = (int)uiType;
        if (uiDictionary.ContainsKey(idx))
        {
            obj = uiDictionary[idx];
        }
        else
        {
            obj = await AddressableManager.Instance.InstantiateAsync(EAddressableType.UI, idx);
            uiDictionary.Add(idx, obj);
        }

        obj.SetActive(true);
        return obj;
    }
    
    [ContextMenu("Set")]
    public async Task SetPoolData()
    {
        GameManager.Instance.Pool.SetPoolData<EUnitRCode>(UnitDataList);
        GameManager.Instance.Pool.SetPoolData<EEnemyType>(EnemyDataList);
        GameManager.Instance.Pool.SetPoolData<EEffectRcode>(EffectDataList);
        GameManager.Instance.Pool.SetPoolData<EOhterRcode>(OtherDataList);

        await MapDataInit();
        
        GameManager.Instance.Pool.StartPooling();
        ResetUI();
    }
    
    [ContextMenu("Rel")]
    public async Task ChangeMapTheme() // 테마 체인지 (Map, Enemy)
    {
        if (currentEnemyType >= AddressableManager.Instance.LocationDict[(int)EAddressableType.Map].Count + (int)EAddressableType.WaterEnemy)
        {
            return;
        }

        foreach (var enemy in EnemyDataList)
        {
            AddressableManager.Instance.ReleaseInstance(enemy.gameObject);
        }
        EnemyDataList.Clear();


        GameManager.Instance.Pool.DestroyPoolObject<EEnemyType>();

        await Task.WhenAll( LoadEnemyAsset(), ChangeMap());

        GameManager.Instance.Pool.ChangePoolObject<EEnemyType>(EnemyDataList);
    }

    public async Task LoadAddressableDatas() // 모든 어드레서블 데이터 로드
    {
        currentEnemyType = (int)EAddressableType.WaterEnemy;

        await Task.WhenAll(LoadEnemyAsset(), LoadUnitAsset(), LoadEffectAsset(), LoadOtherAsset());

    }
    private async Task LoadEnemyAsset()
    { 
        EnemyDataList = await LoadPoolObjectData((EAddressableType)currentEnemyType++);
    }
    private async Task LoadUnitAsset()
    {
        UnitDataList = await LoadPoolObjectData(EAddressableType.Unit);
    }
    private async Task LoadEffectAsset()
    {
        EffectDataList = await LoadPoolObjectData(EAddressableType.Effect);
    }
    private async Task LoadOtherAsset()
    {
        OtherDataList = await LoadPoolObjectData(EAddressableType.Other);
    }

    private async Task ChangeMap() // 현재 활성화된 맵을 언로드 및 다음 맵을 활성화하고 새로운 맵을 로드함
    {
        // Fade Out
        await GameManager.Instance.MapChangeEffect.FadeOut();
        // Fade Out 되었을 때 활성화된 맵 언로드 & 새로운 맵 키기
        AddressableManager.Instance.ReleaseInstance(LoadMaps[curMapNum]);
        LoadMaps[(curMapNum + 1) % LoadMaps.Length].SetActive(true);
        await LoadMapAsset();
        // Fade In
        await GameManager.Instance.MapChangeEffect.FadeIn();
    }

    private async Task LoadMapAsset() // 맵 에셋을 불러옴
    {
        if (loadMapNum == AddressableManager.Instance.LocationDict[(int)EAddressableType.Map].Count)
            return;

        GameObject map = await AddressableManager.Instance.InstantiateAsync(EAddressableType.Map, loadMapNum);
        map.transform.parent = GameManager.Instance.MapTransform;
        map.SetActive(false);

        LoadMaps[curMapNum] = map;

        loadMapNum++;
        curMapNum = (curMapNum + 1) % LoadMaps.Length;
    }

    public async Task MapDataInit()
    {
        LoadMaps = new GameObject[2];
        curMapNum = loadMapNum = 0;

        await LoadMapAsset();
        await LoadMapAsset();

        LoadMaps[curMapNum].SetActive(true);
    }

}