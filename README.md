# VisionInspectionDemo v3 — Unity 6 씬 설정 가이드

---

## 📁 Scripts 폴더 구조

```
Assets/VisionInspectionDemo/Scripts/
├── Core/
│   └── InspectionManager.cs          ← 전체 파이프라인 컨트롤
├── Processing/
│   └── ImageProcessor.cs             ← Grayscale + Threshold 이진화
├── Inspection/
│   └── DefectDetector.cs             ← Black 픽셀 비율 → OK/NG 판정
├── UI/
│   ├── UIController.cs               ← 결과 표시
│   └── NativeFilePickerController.cs ← OS 네이티브 파일 선택 창
└── Utils/
    └── ImageLoader.cs                ← 파일 → Texture2D 로드
```

---

## 🎬 Hierarchy 구성

```
[Scene: InspectionScene]
│
├── Managers                          (Empty GameObject)
│   ├── InspectionManager.cs
│   ├── ImageProcessor.cs
│   ├── DefectDetector.cs
│   └── ImageLoader.cs
│
└── Canvas  (Screen Space - Overlay)
    └── Panel_Main                    ← UIController.cs
        │                               NativeFilePickerController.cs
        ├── RawImage_Original         ← 원본 이미지
        ├── RawImage_Processed        ← 처리 이미지
        ├── Txt_Result   (TMP)        ← "OK" / "NG"
        ├── Txt_FileName (TMP)
        ├── Txt_Ratio    (TMP)
        ├── Txt_Timestamp(TMP)
        ├── Panel_Error               (기본 비활성)
        │   └── Txt_Error (TMP)
        └── Btn_SelectFile            → NativeFilePickerController.OpenFilePicker()
```

---

## 🔗 컴포넌트 연결 체크리스트

### ① Managers 오브젝트

| 컴포넌트 | 필드 | 연결 대상 |
|----------|------|----------|
| InspectionManager | imageLoader | Managers/ImageLoader |
| | imageProcessor | Managers/ImageProcessor |
| | defectDetector | Managers/DefectDetector |
| | uiController | Panel_Main (UIController) |

### ② Panel_Main → UIController

| 필드 | 연결 대상 |
|------|----------|
| rawImageOriginal | RawImage_Original |
| rawImageProcessed | RawImage_Processed |
| txtResult | Txt_Result |
| txtFileName | Txt_FileName |
| txtRatio | Txt_Ratio |
| txtTimestamp | Txt_Timestamp |
| panelError | Panel_Error |
| txtError | Panel_Error/Txt_Error |

### ③ Panel_Main → NativeFilePickerController

| 필드 | 연결 대상 |
|------|----------|
| inspectionManager | Managers/InspectionManager |

### ④ 버튼 OnClick

| 버튼 | 함수 |
|------|------|
| Btn_SelectFile | NativeFilePickerController.OpenFilePicker() |

---

## 📦 파일 선택 창 설치 (빌드 배포 시)

> Unity Editor 플레이 테스트는 설치 없이 바로 동작합니다.

### 방법 A — SimpleFileBrowser (권장, 무료)
1. Asset Store → **"Runtime File Browser"** (Süleyman Yasir KULA) 임포트
2. `NativeFilePickerController.cs` → `① SimpleFileBrowser` 블록 주석 해제
3. `③ 에디터 전용` 블록 주석 처리

### 방법 B — StandaloneFileBrowser (GitHub 오픈소스)
1. https://github.com/gkngkc/UnityStandaloneFileBrowser → `.unitypackage` 다운로드 후 임포트
2. `NativeFilePickerController.cs` → `② SFB` 블록 주석 해제
3. `③ 에디터 전용` 블록 주석 처리


