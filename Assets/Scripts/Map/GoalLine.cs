using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalLine : MonoBehaviour
{
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

            Debug.Log(current.name);
        }
    }
}
