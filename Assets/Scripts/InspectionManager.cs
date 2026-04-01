using UnityEngine;
using System;
using System.Diagnostics;

/// <summary>
/// 비전검사 전체 흐름을 관리하는 핵심 컨트롤러
/// </summary>
public class InspectionManager : MonoBehaviour
{
    [Header("Pipeline Components")]
    public ImageLoader    imageLoader;
    public ImageProcessor imageProcessor;
    public DefectDetector defectDetector;

    [Header("UI")]
    public UIController uiController;

    public InspectionResult LastResult { get; private set; }

    public void ReRunInspection()
    {
        if (uiController.rawImageOriginal.texture == null)
        {
            uiController.ShowError("먼저 '파일 선택'을 해야 합니다.");
            return;
        }

        try
        {
            // 1. 이미지 로드
            Texture2D raw = uiController.rawImageOriginal.texture as Texture2D;
            if (raw == null)
            {
                uiController.ShowError("이미지를 불러올 수 없습니다.");
                return;
            }

            // 2. 처리 시간 측정 시작
            Stopwatch sw = Stopwatch.StartNew();

            // 3. 이미지 처리 (Grayscale + Threshold 이진화)
            Texture2D processed = imageProcessor.Process(raw);

            // 4. 불량 판정
            DetectionData detection = defectDetector.Check(processed);

            // 5. 처리 시간 측정 종료
            sw.Stop();

            string lastFilePath = LastResult.filePath;

            // 4. 결과 객체 생성
            LastResult = new InspectionResult
            {
                filePath = lastFilePath,
                fileName = System.IO.Path.GetFileName(lastFilePath),
                isDefect = detection.isDefect,
                blackRatio = detection.blackRatio,
                rawTex = raw,
                processedTex = processed,
                processingTimeMs = sw.ElapsedMilliseconds
            };

            // 5. UI 업데이트
            uiController.ShowResult(LastResult);

            UnityEngine.Debug.Log($"[InspectionManager] {LastResult.fileName} → {LastResult.StatusText}");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"[InspectionManager] 예외: {e.Message}");
            uiController.ShowError($"오류: {e.Message}");
        }
    }

    /// <summary>
    /// 단일 이미지 검사 실행 (NativeFilePickerController 콜백에서 호출)
    /// </summary>
    public void RunInspection(string path)
    {
        try
        {
            // 1. 이미지 로드
            Texture2D raw = imageLoader.Load(path);
            if (raw == null)
            {
                uiController.ShowError("이미지를 불러올 수 없습니다.");
                return;
            }

            // 2. 처리 시간 측정 시작
            Stopwatch sw = Stopwatch.StartNew();

            // 3. 이미지 처리 (Grayscale + Threshold 이진화)
            Texture2D processed = imageProcessor.Process(raw);

            // 4. 불량 판정
            DetectionData detection = defectDetector.Check(processed);

            // 5. 처리 시간 측정 종료
            sw.Stop();

            // 6. 결과 객체 생성
            LastResult = new InspectionResult
            {
                filePath = path,
                fileName = System.IO.Path.GetFileName(path),
                isDefect = detection.isDefect,
                blackRatio = detection.blackRatio,
                rawTex = raw,
                processedTex = processed,
                processingTimeMs = sw.ElapsedMilliseconds
            };

            // 7. UI 업데이트
            uiController.ShowResult(LastResult);

            UnityEngine.Debug.Log($"[InspectionManager] {LastResult.fileName} → {LastResult.StatusText} ({LastResult.processingTimeMs}ms)");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"[InspectionManager] 예외: {e.Message}");
            uiController.ShowError($"오류: {e.Message}");
        }
    }
}

/// <summary>검사 결과 데이터 모델</summary>
[Serializable]
public class InspectionResult
{
    public string filePath;
    public string fileName;
    public bool isDefect;
    public float blackRatio;
    public Texture2D rawTex;
    public Texture2D processedTex;
    public long processingTimeMs;   // 이미지 처리 소요 시간 (ms)

    public string StatusText => isDefect ? "NG" : "OK";
    public string RatioText => $"{blackRatio * 100f:F1}%";
    public string ProcessingTimeText => processingTimeMs < 1000
        ? $"{processingTimeMs}ms"
        : $"{processingTimeMs / 1000f:F2}s";
}