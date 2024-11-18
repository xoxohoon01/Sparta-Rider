using UnityEngine;
using UnityEngine.InputSystem;

public class MapEditorCameraController : MonoBehaviour
{
    private Camera mainCam;

    private bool isMoving = false;
    private Vector2 movementInput;
    private float moveSpeed = 50f;

    private float zoomSpeed = 5f;
    private float minZoom = 10f;
    private float maxZoom = 60f;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveCamera(movementInput);
        }
    }

    // Input System에서 호출
    public void OnCameraMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>(); // 입력값 저장
            isMoving = true; // 이동 시작
        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero; // 입력값 초기화
            isMoving = false; // 이동 중지
        }
    }

    private void MoveCamera(Vector2 input)
    {
        Vector3 move = new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;
        mainCam.transform.Translate(move, Space.World);
    }

    public void OnCameraZoomInOut(InputAction.CallbackContext context)
    {
        Vector2 scrollDelta = context.ReadValue<Vector2>();

        // Zoom 조정
        float newZoom = Mathf.Clamp(
            mainCam.fieldOfView - scrollDelta.y * zoomSpeed * Time.deltaTime,
            minZoom,
            maxZoom
        );

        if(context.performed)
            mainCam.fieldOfView = newZoom;
    }
}
