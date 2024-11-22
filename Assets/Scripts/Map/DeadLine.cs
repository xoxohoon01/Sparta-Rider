using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Transform topParent = other.transform.root;
            MidPointManager.Instance.RespawnPlayer(topParent.gameObject);
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
