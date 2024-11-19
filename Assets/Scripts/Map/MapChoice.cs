using UnityEngine;

public class MapChoice : MonoBehaviour
{
    public void OnChooseMap(int number)
    {
        GameManager.Instance.mapNumber = number;
        GameManager.Instance.LoadCarChoiceScene();
    }
}
