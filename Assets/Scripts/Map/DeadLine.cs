using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Transform current = other.transform;

            // 최상위 부모를 찾을 때까지 반복
            while (current.parent != null)
            {
                current = current.parent;
            }

            Instantiate(current.gameObject, spawnPoint, Quaternion.identity);
            Destroy(current.gameObject);
        }
    }
}
