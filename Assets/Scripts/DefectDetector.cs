using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 불량 판정 모듈: 이진화 이미지의 Black 픽셀 비율로 OK/NG 결정
/// </summary>
public class DefectDetector : MonoBehaviour
{
    [Header("판정 기준")]
    [Range(0f, 1f)]
    [Tooltip("Black 픽셀 비율이 이 값을 초과하면 NG")]
    public float defectThreshold = 0.3f;
    public Slider defectThreshold_Slider;
    public TextMeshProUGUI defectThreshold_Text;

    public void OnValueChanged_DefectThreshold(float _value)
    {
        defectThreshold = _value;
        defectThreshold_Text.text = _value.ToString("N2");
    }

    private void Start()
    {
        defectThreshold_Slider.value = defectThreshold;
        OnValueChanged_DefectThreshold(defectThreshold);
    }

    public DetectionData Check(Texture2D binaryTex)
    {
        if (binaryTex == null)
            return new DetectionData { isDefect = true, blackRatio = 1f };

        Color32[] pixels     = binaryTex.GetPixels32();
        int       totalCount = pixels.Length;
        int       blackCount = 0;

        foreach (Color32 p in pixels)
        {
            if (p.r < 128) blackCount++;
        }

        float ratio    = (float)blackCount / totalCount;
        bool  isDefect = ratio > defectThreshold;

        Debug.Log($"[DefectDetector] {blackCount}/{totalCount} ({ratio * 100f:F1}%) → {(isDefect ? "NG" : "OK")}");

        return new DetectionData
        {
            isDefect   = isDefect,
            blackRatio = ratio,
            blackCount = blackCount,
            totalCount = totalCount
        };
    }
}

[System.Serializable]
public class DetectionData
{
    public bool  isDefect;
    public float blackRatio;
    public int   blackCount;
    public int   totalCount;
}
