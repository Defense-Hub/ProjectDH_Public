using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class MeshCombiner : MonoBehaviour
{
    private void Awake()
    {
        CombineNewMeshes();
    }

    public void CombineNewMeshes()
    {
        // 메쉬 필터와 메쉬 렌더러를 가진 자식 오브젝트들을 가져옴
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        Dictionary<Material, List<CombineInstance>> materialToMeshCombine = new Dictionary<Material, List<CombineInstance>>();

        // 각 메쉬 필터에 대해
        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer == null) continue;

            // 여러개의 머터리얼을 가지고 있는 경우 Combine 제외
            if (meshRenderer.sharedMaterials.Length > 1)
            {
                continue;
            }

            // Mesh를 읽을 수 없을 때도 Combine 제외 (읽을 수 없으면 CombineMesh가 그리질 못함)
            if (!meshFilter.sharedMesh.isReadable)
            {
                Debug.LogWarning($"{meshFilter.name} is not Readable");
                continue;
            }
            
            // 메쉬 렌더러의 머터리얼을 가져옴
            Material material = meshRenderer.sharedMaterial;

            // 머터리얼별로 메쉬를 그룹화
            if (!materialToMeshCombine.ContainsKey(material))
            {
                materialToMeshCombine.Add(material, new List<CombineInstance>());
            }

            CombineInstance combineInstance = new CombineInstance();
            combineInstance.mesh = meshFilter.sharedMesh;
            combineInstance.transform = meshFilter.transform.localToWorldMatrix;
            materialToMeshCombine[material].Add(combineInstance);

            // 결합된 메쉬가 생성되기 전에 기존에 존재하는 Mesh들은 비활성화
            meshFilter.gameObject.SetActive(false);
        }

        // 각 머터리얼 그룹에 대해
        foreach (var entry in materialToMeshCombine)
        {
            Material material = entry.Key;
            CombineInstance[] combineInstances = entry.Value.ToArray();

            // 새 메쉬를 생성하고 결합된 메쉬로 설정
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combineInstances, true, true);

            // 새 메쉬 오브젝트를 생성
            GameObject combinedObject = new GameObject("Combined Mesh");
            combinedObject.transform.SetParent(transform);

            MeshFilter meshFilter = combinedObject.AddComponent<MeshFilter>();
            meshFilter.mesh = combinedMesh;

            MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        }

        // 부모 오브젝트를 활성화
        gameObject.SetActive(true);
    }
}