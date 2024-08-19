using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWindow : MonoBehaviour
{
    private bool state;
    public GameObject Target;

    [SerializeField] private Button settingButton;

    private void Awake()
    {
        settingButton.onClick.AddListener(OnClickDisplayButton);
    }
    public void OnClickDisplayButton()
    {
        Target.SetActive(true);
        Debug.Log("디스플레이 생겨남");

    }
}