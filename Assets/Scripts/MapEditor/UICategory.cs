using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICategory : MonoBehaviour
{
    [SerializeField] GameObject[] spawners = new GameObject[4];

    private int buttonIndex;

    public void OnLineTracksClick()
    {
        buttonIndex = 0;
        SetButtonActive(buttonIndex);
    }

    public void OnCurvedTracksClick()
    {
        buttonIndex = 1;
        SetButtonActive(buttonIndex);
    }
    public void OnArrowsClick()
    {
        buttonIndex = 2;
        SetButtonActive(buttonIndex);
    }
    public void OnFencesClick()
    {
        buttonIndex = 3;
        SetButtonActive(buttonIndex);
    }


    private void SetButtonActive(int index)
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (i == index)
            {
                spawners[i].SetActive(true);
            }
            else
            {
                spawners[i].SetActive(false);
            }
        }
    }
}
