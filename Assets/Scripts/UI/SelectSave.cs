using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSave : MonoBehaviour
{
    private bool isSelected;
    private Image img;
    private string fileName;

    void Start()
    {
        img = GetComponent<Image>();
        fileName = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    void Update()
    {
        if (isSelected && GameManager.Instance.SelectedSave != GetComponentInChildren<TextMeshProUGUI>().text)
        {
            isSelected = false;
            img.color = Color.white;
        }
    }

    public void SetSelectedSave()
    {
        GameManager.Instance.SelectedSave = fileName;
        isSelected = true;
        img.color = Color.grey;
    }
}
