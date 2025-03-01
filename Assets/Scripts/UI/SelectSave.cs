using TMPro;
using UnityEngine;

public class SelectSave : MonoBehaviour
{
    public void setSelectedSave()
    {
        GameManager.Instance.SelectedSave = GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
