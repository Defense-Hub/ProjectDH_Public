using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class PoolManager : MonoBehaviour
{
    
    [Header("# Pool Info")] 
    [SerializeField] public List<Pool> poolInfos;
    [SerializeField] public List<Transform> poolTransforms;
    private Dictionary<int, List<PoolObject>> poolDictionary;
    
    [Serializable]
    public class Pool
    {
        public int key; // key 값
        public PoolObject prefab; // 실제 생성될 오브젝트
        public int size; // 한번에 몇개를 생성할 것인지
        public Transform parentTransform; // 부모 오브젝트
    }
    
    private void Start()
    {
        GameManager.Instance.Pool = this;
        // 딕셔너리 초기화
        poolDictionary = new Dictionary<int, List<PoolObject>>();

        for (int i = 0; i < transform.childCount; i++)
        {
            poolTransforms.Add(transform.GetChild(i));
        }
    }
    
    public void SetPoolData<T>(List<PoolObject> poolObjList) where T : Enum
    {
        int idx = 0;
        foreach (var enumValue in Enum.GetValues(typeof(T)))
        {
            if (poolObjList[idx] == null)
            {
                Debug.LogWarning($"{enumValue}타입의 {idx} 번째 데이터가 존재하지 않습니다.");
                return;
            }
            int intEnumValue = (int)enumValue;
            PoolObject obj = poolObjList[idx++];
            Pool poolData = new Pool()
            {
                prefab = obj,
                key = intEnumValue,
                parentTransform = GetPoolParentTransform(enumValue),
                size = obj.SpawnCount
            };
            poolInfos.Add(poolData);
        }
    }
    
    public void StartPooling()
    {
        // pools에 있는 모든 오브젝트를 탐색하고 정해놓은 size만큼 프리팹을 미리 만들어 놓음
        foreach (Pool pool in poolInfos)
        {
            AddPoolObject(pool);
            // if(pool.size > 1) // pool size가 1보다 클 때만 미리 생성 (1은 Enemy이기 때문에 미리 생성 X)
            //     AddPoolObject(pool.type);
        }
    }

    private void AddPoolObject(Pool pool)
    {
        List<PoolObject> list = new List<PoolObject>();
            
        poolDictionary.TryAdd(pool.key, list);
            
        CreatePoolObject(pool.key);
    }
    
    private void CreatePoolObject(int key) // 프리팹 생성
    {
        Pool poolData = poolInfos.Find(obj => obj.key == key);
        
        // pool.size만큼 프리팹을 생성 -> 비활성화 -> 리스트에 넣어줌
        for (int i = 0; i < poolData.size; i++)
        {
            PoolObject poolObj = Instantiate(poolData.prefab, poolData.parentTransform); 
            poolObj.gameObject.SetActive(false);
            poolDictionary[key].Add(poolObj);
        }
    }

    // 이미 생성된 오브젝트 풀에서 프리팹을 가져옴
    public PoolObject SpawnFromPool(int type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError($"{type} 번째 생성되지 않았습니다.");
            return null;
        }
        
        PoolObject poolObject = null;
        
        for (int i = 0; i < poolDictionary[type].Count; i++)
        {
            if (!poolDictionary[type][i].gameObject.activeSelf) // 비활성화된 오브젝트를 찾았을 때
            {
                poolObject = poolDictionary[type][i];
                break;
            }

            if (i == poolDictionary[type].Count - 1) // 모든 오브젝트가 활성화 됐을 때 
            {
                CreatePoolObject(type);
                poolObject = poolDictionary[type][i + 1];
            }
        }
        
        poolObject.gameObject.SetActive(true); // 활성화
        
        return poolObject;
    }

    public void ChangePoolObject <T>(List<PoolObject> poolList) where T : Enum // PoolObject교체
    {
        int idx = 0;
        foreach (var enumValue in Enum.GetValues(typeof(T)))
        {
            int intEnumValue = (int)enumValue;

            Pool targetPool = poolInfos.Find(obj => obj.key == intEnumValue);

            targetPool.prefab = poolList[idx++];
            
            AddPoolObject(targetPool);
        }
    }
    public void DestroyPoolObject <T>() where T : Enum // PoolObject 삭제
    {
        var enumValues = Enum.GetValues(typeof(T));

        foreach (var enumValue in enumValues)
        {
            if (poolDictionary.TryGetValue((int)enumValue, out var poolList))
            {
                for (int i = poolList.Count-1; i >= 0; i--)
                {
                    GameObject obj = poolList[i].gameObject;
                    poolList.RemoveAt(i);
                    Destroy(obj);
                }
            }
        }
    }
    private Transform GetPoolParentTransform <T>(T type)
    {
        return type switch
        {
            EUnitRCode => poolTransforms[0],
            EEffectRcode => poolTransforms[1],
            EEnemyType => poolTransforms[2],
            EOhterRcode => poolTransforms[3],
        };
    }
}