using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public ItemGenerator generator;
    public Collider col;
    public Renderer ren;
    public ParticleSystem particle;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(5f);

    private void Awake()
    {
        generator = GetComponent<ItemGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Deactivate();
            generator.Generate(other.gameObject);
            StartCoroutine(CoActivation());
        }
    }

    private void Deactivate()
    {
        particle.Stop();
        col.enabled = false;
        ren.enabled = false;
    }

    private IEnumerator CoActivation()
    {
        yield return waitForSeconds;
        particle.Play();
        col.enabled = true;
        ren.enabled = true;
    }
}
