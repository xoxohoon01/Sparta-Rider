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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            particle.Stop();
            col.enabled = false;
            ren.enabled = false;
            // GameManager에 ItemGenerator를 넣으면 코드 바꿀 예정!!!
            generator.Generate();
        }
    }

    private IEnumerator CoItemBoxActivation()
    {
        yield return waitForSeconds;
        particle.Play();
        col.enabled = true;
        ren.enabled = true;
    }
}
