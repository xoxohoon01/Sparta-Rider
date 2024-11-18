using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Vector3 moveDirection = Vector3.zero;
    private float itemSizeX;
    private float itemSizeY;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        itemSizeX = renderer.bounds.size.z;
        itemSizeY = renderer.bounds.size.y;
    }

    private void Update()
    {
        if(itemSO.speed > 0) transform.position += itemSO.speed * Time.deltaTime * moveDirection;    
    }

    public void Move(Vector3 forward)
    {
        // 아이템 위치 계산
        if (itemSO.speed > 0) transform.position += itemSizeX * 0.5f * transform.forward;
        else transform.position += itemSizeX * 0.5f * -transform.forward;
        transform.position += itemSizeY * 0.5f * Vector3.up;
        moveDirection = forward;
    }
}
