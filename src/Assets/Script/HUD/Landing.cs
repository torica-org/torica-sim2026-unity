using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Landing : MonoBehaviour
{
    private GameObject Result;
    private GameObject SimpleResult;
    //[SerializeField] private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        // Result = GameObject.Find("Result");
        // SimpleResult = GameObject.Find("SimpleResult");
        //canvas.enabled = false;

        // Result.SetActive(false);
        // SimpleResult.SetActive(false);
        GameManager.instance.game.Landing = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.game.Landing = true;
        //GameManager.instance.game.SettingMode = 0; // ?
        //canvas.enabled = true;
        ResultScreen.terminationReason = "着水";
        Time.timeScale=(float)Convert.ToInt32(!GameManager.instance.game.SettingActive & !GameManager.instance.game.Landing);
        Log.Append($"[Splashdown!] Result: {GameManager.instance.game.Distance:0.00} m");
    }

    // Update is called once per frame
    void Update()
    {
        // Result.SetActive(!GameManager.instance.game.SettingActive & GameManager.instance.game.Landing);
        // SimpleResult.SetActive(!GameManager.instance.game.SettingActive & GameManager.instance.game.Landing);
    }
}
