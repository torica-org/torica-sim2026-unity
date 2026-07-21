using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameParameters game;
    public AerodynamicParameters aero;
    public AerodynamicCalculator calc;
    public PilotPositionResetter pilot;
    public CameraManager cm;
    private GameObject FlightSetting = null;

    private float timeInCurrentStatus = 0.0f;
    private readonly float IGNORE_INPUT_TIME = 1.0f;

    public static GameManager instance = null;
    public bool FirstLoad;//シミュ起動後最初のシーンロードか否か
    // ===== このインスタンスがロードされたときに実行される ==============
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            FirstLoad = true;
            DontDestroyOnLoad(this.gameObject);
            Log.Append("GameManager Awake.");
        }
        else
        {
            FirstLoad = false;
            Destroy(this.gameObject);
        }
        game = new();
        aero = new();
    }

    // ===== Updateの初回呼び出し前に実行される =============
    private void Start()
    {
        FlightSetting = GameObject.Find("FlightSetting");
        cm = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        calc = new(game, aero);
        pilot = new(aero);
        // GameObject aeroObj = GameObject.Find("");
        SerialHandler.OnHoldingPositive += PositiveHolding;
        SerialHandler.OnHoldingNegative += NegativeHolding;
        calc.Initialize();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        calc?.Initialize();
    }

    private void PositiveHolding()
    {
        if (timeInCurrentStatus >= IGNORE_INPUT_TIME)
        {
            if (game.status == GameParameters.Status.Splashdown) // 長押し＋着水
            {
                Debug.Log("CMD: Reset");
                game.EnterFlight = false;
                game.SettingMode = 2;
                SceneManager.LoadScene("FlightScene");
            }
            if (game.status == GameParameters.Status.Preparation) // 正の長押し＋準備
            {
                Debug.Log("CMD: Caribrate");
                if (cm == null)
                {
                    cm = GameObject.Find("CameraManager").GetComponent<CameraManager>();
                }
                if (cm != null)
                {
                    cm.CaribrateVR();
                }
                pilot.ResetPilotPosition();
            }
            timeInCurrentStatus = 0.0f;
        }
    }

    private void NegativeHolding()
    {
        if (timeInCurrentStatus >= IGNORE_INPUT_TIME)
        {
            if (game.status == GameParameters.Status.Splashdown) // 長押し＋着水
            {
                Debug.Log("CMD: Reset");
                game.EnterFlight = false;
                game.SettingMode = 2;
                SceneManager.LoadScene("FlightScene");
            }
            if (game.status == GameParameters.Status.Preparation)
            {
                Debug.Log("CMD: Start");
                game.EnterFlight = true;
                if (FlightSetting == null)
                {
                    FlightSetting = GameObject.Find("FlightSetting");
                }
                if (FlightSetting != null)
                {
                    game.FlightSettingActive = false;
                    FlightSetting.SetActive(game.FlightSettingActive);
                }
                Time.timeScale = (float)Convert.ToInt32(!game.FlightSettingActive & !game.Landing);
            }
            timeInCurrentStatus = 0.0f;
        }
    }

    // ===== 毎フレーム実行される ==============
    private void Update()
    {
        calc.DevicesUpdate();
        GameParameters.Status prev = game.status;

        bool Setting = game.SettingActive || game.FlightSettingActive;
        // ----- ゲームの状態を管理 -----
        if (Setting == true && game.EnterFlight == false && game.Landing == false)
        {
            game.status = GameParameters.Status.Preparation;
        }
        else if (Setting == false && game.EnterFlight == true && game.Landing == false)
        {
            game.status = GameParameters.Status.Flight;
        }
        else if (Setting == false && game.EnterFlight == true && game.Landing == true)
        {
            game.status = GameParameters.Status.Splashdown;
        }

        if (prev != game.status)
        {
            if (game.status == GameParameters.Status.Flight)
            {
                Log.Append("[Takeoff!]");
            }
            timeInCurrentStatus = 0f;
        }
        else
        {
            timeInCurrentStatus += Time.unscaledDeltaTime;
        }
        // Debug.Log(timeInCurrentStatus);
    }

    public void FixedUpdate()
    {
        // ----- シミュレーションの実行 -----
        calc.CalculatorFixedUpdate();
    }
}
