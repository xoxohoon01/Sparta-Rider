using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [SerializeField] GameObject map;
    private Camera mainCam;
    private GameObject prefab;
    private GameObject currentPrefab;  // 생성된 프리팹을 저장할 변수

    private Stack<GameObject> placedPrefabs = new Stack<GameObject>();
    [SerializeField] float nearDistance = 30f;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // 버튼 클릭 시 호출되는 메소드
    public void OnButtonClick(string name)
    {
        prefab = Resources.Load<GameObject>(name);  // 리소스에서 프리팹 로드
        if (prefab != null)
        {
            // 프리팹 배치 중이었다면 제거
            Destroy(currentPrefab);
            // 프리팹 생성 (최초에는 카메라 위치에 배치)
            currentPrefab = Instantiate(prefab, mainCam.transform.position, Quaternion.identity, map.transform);
        }
    }

    private void Update()
    {
        if (currentPrefab != null)
        {
            MovePrefabToMousePosition();  // 마우스 위치로 프리팹 이동
        } else if (Input.GetMouseButtonDown(1))
        {
            GameObject prefab = placedPrefabs.Pop();
            Destroy(prefab);
        }
    }

    // 마우스 위치로 프리팹을 이동시키는 메소드
    private void MovePrefabToMousePosition()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 생성
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Ground");  // "Ground" 레이어만 검사

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);

            foreach (GameObject prefab in placedPrefabs)
            {
                // 기존 프리팹과의 거리
                float distance = (currentPrefab.transform.position - prefab.transform.position).magnitude;
                
                if (distance <= nearDistance)
                {
                    // TODO : 자석처럼 옆에 붙이기
                    
                }
            }

            float colliderHeight = hit.collider.bounds.size.y;
            targetPosition = hit.transform.position + colliderHeight * Vector3.up;

            // 프리팹 목표 위치로 이동
            currentPrefab.transform.position = targetPosition;

            if (Input.GetMouseButtonDown(0))
            {
                placedPrefabs.Push(currentPrefab);
                currentPrefab = null;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentPrefab); // 프리팹 제거
            }
        }
    }
}