using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MidPointManager.Instance.RespawnPlayer(other.gameObject);
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
