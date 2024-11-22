using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelp : MonoBehaviour
{
    public GameObject helpText;

    public void OnClick()
    {
        helpText.SetActive(!helpText.activeSelf);
    }
}
