using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldingDetector{

    private GameParameters game;
    private CameraManager cm;
    private const float IGNORE_INPUT_TIME = 1.0f;
    private float calibrateTimePre = 0.0f;

    public HoldingDetector(GameParameters _game, CameraManager _cm)
    {
        game = _game;
        cm = _cm;
    }

    public void PositiveHolding()
    {
        if (game.timeInCurrentStatus >= IGNORE_INPUT_TIME)
        {
            if (game.status == GameParameters.Status.Splashdown) // 長押し＋着水
            {
                Debug.Log("CMD: Reset");
                game.status = GameParameters.Status.Preparation;
                game.SettingMode = 2;
                SceneManager.LoadScene("FlightScene");
            }
            if (game.status == GameParameters.Status.Preparation) // 正の長押し＋準備
            {
                if (game.timeInCurrentStatus - calibrateTimePre >= 0.5)
                {
                    Debug.Log("CMD: Calibrate");
                    if (cm == null)
                    {
                        cm = GameObject.Find("CameraManager").GetComponent<CameraManager>();
                    }
                    if (cm != null)
                    {
                        cm.CalibrateVR();
                    }
                    GameManager.instance.pilot.ResetPilotPosition();
                    calibrateTimePre = game.timeInCurrentStatus;
                }
            }
        }
    }

    public void NegativeHolding()
    {
        if (game.timeInCurrentStatus >= IGNORE_INPUT_TIME)
        {
            if (game.status == GameParameters.Status.Splashdown) // 長押し＋着水
            {
                Debug.Log("CMD: Reset");
                game.status = GameParameters.Status.Preparation;
                game.SettingMode = 2;
                SceneManager.LoadScene("FlightScene");
            }
            if (game.status == GameParameters.Status.Preparation)
            {
                Debug.Log("CMD: Start");
                game.status = GameParameters.Status.Flight;
           }
       }
   }

}
