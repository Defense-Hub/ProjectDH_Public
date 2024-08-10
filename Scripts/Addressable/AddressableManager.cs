using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.Serialization;

public class AddressableManager : Singleton<AddressableManager>
{
    // 어드레서블의 Label을 얻어올 수 있는 필드.
    public AssetLabelReference[] assetLabels;
    
    // IResourceLocation : 리소스의 경로를 저장한다
    public Dictionary<int, IList<IResourceLocation>> LocationDict { get;  private set; } = new Dictionary<int, IList<IResourceLocation>>();

    public bool IsLocationLoad { get; private set; }

    public async Task GetLocations()
    {
        if(IsLocationLoad )
            return;

        IsLocationLoad = true;
        // 딕셔너리에 리소스 경로 할당
        await LoadLocationsAsync();
        
        SortLocationDictionary();
        
        // ResourceManager.Instance.LoadUnitAsset(); // 유닛 로드
    }

    private async Task LoadLocationsAsync()
    {
        // 빌드타겟의 경로를 가져온다.
        for (int i = 0; i < assetLabels.Length; i++)
        {
            var handle = Addressables.LoadResourceLocationsAsync(assetLabels[i].labelString);
            
            // 비동기 작업 완료를 기다림
            await handle.Task;

            // 결과를 할당
            LocationDict[i] = handle.Result;
        }
    }

    private Type GetAddressableCode(EAddressableType type)
    {
        Type code = type switch
        {
            EAddressableType.Unit => typeof(EUnitRCode),
            EAddressableType.UI => typeof(EUIRCode),
            EAddressableType.FireEnemy => typeof(EFireEnemyRCode),
            EAddressableType.Effect => typeof(EEffectRcode),
            EAddressableType.Other => typeof(EOhterRcode),
            EAddressableType.WaterEnemy => typeof(EWaterEnemyRCode),
            EAddressableType.EarthEnemy => typeof(EEarthEnemyRCode),
            EAddressableType.IceEnemy => typeof(EIceEnemyRCode),
            EAddressableType.DarkEnemy => typeof(EDarkEnemyRCode),
            EAddressableType.Map => typeof(EMapRcode),
            EAddressableType.HuntEnemy => typeof(EHuntEnemyCode),
            EAddressableType.Thumbnail => typeof(EUnitRCode),
            _ => throw new NotImplementedException()
        };
        
        return code;
    }
    private void SortLocationDictionary() // 로케이션 정렬
    {
        // flag L 검색 조건
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        for (int i = 0; i < LocationDict.Count; i++)
        {
            Type enumType = GetAddressableCode((EAddressableType)i);
            MethodInfo method = typeof(AddressableManager).GetMethod(nameof(SortLocations),flag)?.MakeGenericMethod(enumType);
            LocationDict[i] = (IList<IResourceLocation>)method?.Invoke(this, new object[] { LocationDict[i]});
        }
        
        // LocationDict[(int)EAddressableType.Unit] = SortLocations<EUnitRCode>(LocationDict[(int)EAddressableType.Unit]);
        // LocationDict[(int)EAddressableType.UI] = SortLocations<EUIRCode>(LocationDict[(int)EAddressableType.UI]);
        // LocationDict[(int)EAddressableType.FireEnemy] = SortLocations<EFireEnemyRCode>(LocationDict[(int)EAddressableType.FireEnemy]);
    }

    private IList<IResourceLocation> SortLocations <T> (IList<IResourceLocation> locations ) where T : Enum // 어드레서블 자동 정렬
    {
        IList<IResourceLocation> temp = new List<IResourceLocation>();
        
        foreach (T enumValue in Enum.GetValues(typeof(T)))
        {
            string enumName = enumValue.ToString();
            IResourceLocation location = locations.FirstOrDefault(loc =>  loc.PrimaryKey.Contains(enumName));
            if (location != null)
            {
                temp.Add(location);

            }
            else
            {
                Debug.LogWarning($"{enumName}이 존재하지 않습니다.");
            }
        }
        return temp;
    }

    public async Task<GameObject> InstantiateAsync(EAddressableType type ,int idx)
    {
        // 해당 라벨의 idx번째 리소스를 생성한다.
        var handle = Addressables.InstantiateAsync(LocationDict[(int)type][idx]);
        await handle.Task;

         if (handle.Status == AsyncOperationStatus.Succeeded) // 성공
        {
            GameObject obj = handle.Result.gameObject;
            return obj; // 반환
        }
        else // 실패
        {
            Debug.LogError("Failed to instantiate the object.");
            return default;
        }
    }
    
    // MonoBehaviour를 상속받는 데이터 형태만 T에 넣을 수 있도록 강제함.
    public async Task<T> LoadMonoBehaviorAsset <T> (EAddressableType type ,int idx) where T : MonoBehaviour
    {
        // 해당 라벨의 idx번째 리소스의 게임 오브젝트 정보를 저장 (LoadAsset은 생성 X, 정보만 저장)
        var handle = Addressables.LoadAssetAsync<GameObject>(LocationDict[(int)type][idx]);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded) // 성공
        {
            GameObject loadedGameObject = handle.Result;
            // 게임 오브젝트 정보를 MonoBehaviour를 상속받는 T의 정보를 저장
            T component = loadedGameObject.GetComponent<T>(); 
            if (component != null)
            {
                return component;
            }
            else
            {
                Debug.LogError($"The loaded GameObject does not have a component of {typeof(T)}.");
                return default;
            }
        }
        else
        {
            Debug.LogError("Failed to LoadMonoAsset the object.");
            return default;
        }
    }
    
    public async Task<T> LoadAsset <T> (EAddressableType type ,int idx) 
    {
        // 해당 라벨의 idx번째 리소스의 게임 오브젝트 정보를 저장 (LoadAsset은 생성 X, 정보만 저장)
        var handle = Addressables.LoadAssetAsync<T>(LocationDict[(int)type][idx]);
        
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded) // 성공
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Failed to LoadAsset the object.");
            return default;
        }
    }
    
    public void ReleaseInstance(GameObject asset)// 생성된 오브젝트 제거
    {
        Addressables.ReleaseInstance(asset);
    }
}


