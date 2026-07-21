using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SettingCloseButton : MonoBehaviour
{
    private GameObject Setting;
    private bool firstPush = false;

    // Start is called before the first frame update
    void Start()
    {
        Setting = GameObject.Find("Setting");
    }

    public void OnClick()
    {
        if (!firstPush)
        {
            GameManager.instance.game.SettingActive = !GameManager.instance.game.SettingActive;
            Setting.SetActive(GameManager.instance.game.SettingActive);
            Time.timeScale=(float)Convert.ToInt32(!GameManager.instance.game.FlightSettingActive & !GameManager.instance.game.SettingActive & !GameManager.instance.game.Landing);
        }
    }
}
