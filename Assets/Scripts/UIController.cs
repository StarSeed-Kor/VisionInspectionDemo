using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI 컨트롤러
/// 배치 위치: Canvas > Panel_Main 오브젝트에 Add Component
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("── 이미지 표시 ──────────────────")]
    [Tooltip("원본 이미지 (로드 직후 표시)")]
    public RawImage rawImageOriginal;
    [Tooltip("처리된 이미지 (Grayscale + Threshold 결과)")]
    public RawImage rawImageProcessed;

    [Header("── 결과 텍스트 ─────────────────")]
    public TMP_Text txtResult;          // "OK" / "NG"
    public TMP_Text txtFileName;        // 파일명
    public TMP_Text txtRatio;           // Black 비율
    public TMP_Text txtProcessingTime;  // 이미지 처리 소요 시간

    [Header("── 색상 설정 ──────────────────")]
    public Color colorOK = new Color(0.2f, 0.85f, 0.2f);
    public Color colorNG = new Color(0.9f, 0.2f, 0.2f);
    public Color colorError = Color.yellow;

    [Header("── 에러 패널 ─────────────────")]
    public GameObject panelError;
    public TMP_Text txtError;

    // ──────────────────────────────────────────
    void Start() => ResetUI();

    // ──────────────────────────────────────────
    /// <summary>검사 완료 후 결과 표시</summary>
    public void ShowResult(InspectionResult result)
    {
        HideError();

        if (rawImageOriginal != null) rawImageOriginal.texture = result.rawTex;
        if (rawImageProcessed != null) rawImageProcessed.texture = result.processedTex;

        if (txtResult != null)
        {
            txtResult.text = $"결과: {result.StatusText}";
            txtResult.color = result.isDefect ? colorNG : colorOK;
        }

        if (txtFileName != null) txtFileName.text = $"파일명: {result.fileName}";
        if (txtRatio != null) txtRatio.text = $"Black 비율: {result.RatioText}";
        if (txtProcessingTime != null) txtProcessingTime.text = $"처리 시간: {result.ProcessingTimeText}";
    }

    /// <summary>에러 메시지 표시</summary>
    public void ShowError(string message)
    {
        if (panelError != null) panelError.SetActive(true);
        if (txtError != null) { txtError.text = message; txtError.color = colorError; }
    }

    public void HideError()
    {
        if (panelError != null) panelError.SetActive(false);
    }

    public void ResetUI()
    {
        HideError();
        if (rawImageOriginal != null) rawImageOriginal.texture = null;
        if (rawImageProcessed != null) rawImageProcessed.texture = null;
        if (txtResult != null) { txtResult.text = "-"; txtResult.color = Color.white; }
        if (txtFileName != null) txtFileName.text = "-";
        if (txtRatio != null) txtRatio.text = "Black 비율: -";
        if (txtProcessingTime != null) txtProcessingTime.text = "처리 시간: -";
    }
}