using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.InputSystem;

public class ItemSpawnPoint : MonoBehaviour
{
    private WaitForSeconds waitForSeconds = new WaitForSeconds(5f);
    private BoxCollider currentCollider;

    public GameObject item;
    public ItemGenerator generator;

    private void Awake()
    {
        generator = GetComponent<ItemGenerator>();
        currentCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SetItem();
    }

    private void SetItem()
    {
        if(transform.childCount > 0)
        {
            transform.DetachChildren();
        }
        item = generator.Generate().item;
        item.transform.position = transform.position;
        item.SetActive(true);
        item.transform.SetParent(transform);
    }

    public void ResetItem()
    {
        Destroy(item);
        StartCoroutine(CoGenerator());
    }

    private IEnumerator CoGenerator()
    {
        yield return waitForSeconds;
        SetItem();
        Debug.Log("Item generator");
    }
}