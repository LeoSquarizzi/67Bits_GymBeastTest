using TMPro;
using UnityEngine;

public class FPSCorner : MonoBehaviour
{
    float deltaTime = 0.0f;

    //public TextMeshProUGUI fpsText;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f; // Suavização do FPS

        //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //float fps = 1.0f / deltaTime;
        //fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = Color.white;

        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);
        GUI.Label(rect, text, style);
    }

    public void LogFPS()
    {
        float fps = 1.0f / deltaTime;
        Debug.Log("FPS atual: " + Mathf.RoundToInt(fps));
    }
}
