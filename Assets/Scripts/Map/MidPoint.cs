using UnityEngine;

public class MidPoint : MonoBehaviour
{
    private int id;

    public void SetId(int newId)
    {
        id = newId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MidPointManager.Instance.PassMidpoint(id, transform);
        }
    }
}
