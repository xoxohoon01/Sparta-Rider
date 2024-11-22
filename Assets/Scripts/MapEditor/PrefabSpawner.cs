using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject map;
    private Camera mainCam;
    private GameObject prefab;
    private GameObject currentPrefab;  // 생성된 프리팹을 저장할 변수

    [SerializeField] private GameObject UIConfirm;

    public Stack<GameObject> placedPrefab = new Stack<GameObject>();

    private float height = 0.5001f;

    private void Awake()
    {
        mainCam = Camera.main;
        UIConfirm.SetActive(false);
    }

    // 버튼 클릭 시 호출되는 메소드
    public void OnButtonClick()
    {
        prefab = Resources.Load<GameObject>($"Prefabs/{EventSystem.current.currentSelectedGameObject.name}");  // 리소스에서 프리팹 로드
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
            Vector3 targetPosition;

            if (currentPrefab.layer == LayerMask.NameToLayer("Track"))
            {
                float colliderHeight = hit.collider.bounds.size.y;
                targetPosition = hit.transform.position + colliderHeight * new Vector3(0, height, 0);
            }
            else
            {
                targetPosition = new Vector3(hit.point.x, 0, hit.point.z);
            }

            // 프리팹 목표 위치로 이동
            currentPrefab.transform.position = targetPosition;

            
        }
    }

    public void OnInstall(InputAction.CallbackContext context)
    {
        if (currentPrefab != null && context.performed)
        {
            // 이름 뒤에 Clone 제거
            currentPrefab.name = currentPrefab.name.Replace("(Clone)", "").Trim();
            placedPrefab.Push(currentPrefab);
            currentPrefab = null;
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (currentPrefab != null && context.performed)
        {
            Destroy(currentPrefab);
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (currentPrefab != null && context.performed)
        {
            // Y축 기준으로 90도 회전
            currentPrefab.transform.Rotate(0, 90, 0, Space.World);
        }
    }

    public void OnUndo(InputAction.CallbackContext context)
    {
        // z = Ctrl + z
        if (placedPrefab.Count > 0 && context.performed)
        {
            GameObject prefab = placedPrefab.Pop();
            Destroy(prefab);
        }
    }

    public void OnConfirm()
    {
        UIConfirm.SetActive(true);
    }

    public void OnDelete()
    {
        MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    }
}