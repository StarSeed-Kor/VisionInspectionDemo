# VisionInspectionDemo v3 — Unity 6 씬 설정 가이드

\---

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

\---

## 🎬 Hierarchy 구성

```
\[Scene: InspectionScene]
│
├── Managers                          (Empty GameObject)
│   ├── InspectionManager.cs
│   ├── ImageProcessor.cs
│   ├── DefectDetector.cs
│   └── ImageLoader.cs
│
└── Canvas  (Screen Space - Overlay)
    └── Panel\_Main                    ← UIController.cs
        │                               NativeFilePickerController.cs
        ├── RawImage\_Original         ← 원본 이미지
        ├── RawImage\_Processed        ← 처리 이미지
        ├── Txt\_Result   (TMP)        ← "OK" / "NG"
        ├── Txt\_FileName (TMP)
        ├── Txt\_Ratio    (TMP)
        ├── Txt\_Timestamp(TMP)
        ├── Panel\_Error               (기본 비활성)
        │   └── Txt\_Error (TMP)
        └── Btn\_SelectFile            → NativeFilePickerController.OpenFilePicker()
```

\---

## 🔗 컴포넌트 연결 체크리스트

### ① Managers 오브젝트

|컴포넌트|필드|연결 대상|
|-|-|-|
|InspectionManager|imageLoader|Managers/ImageLoader|
||imageProcessor|Managers/ImageProcessor|
||defectDetector|Managers/DefectDetector|
||uiController|Panel\_Main (UIController)|

### ② Panel\_Main → UIController

|필드|연결 대상|
|-|-|
|rawImageOriginal|RawImage\_Original|
|rawImageProcessed|RawImage\_Processed|
|txtResult|Txt\_Result|
|txtFileName|Txt\_FileName|
|txtRatio|Txt\_Ratio|
|txtTimestamp|Txt\_Timestamp|
|panelError|Panel\_Error|
|txtError|Panel\_Error/Txt\_Error|

### ③ Panel\_Main → NativeFilePickerController

|필드|연결 대상|
|-|-|
|inspectionManager|Managers/InspectionManager|

### ④ 버튼 OnClick

|버튼|함수|
|-|-|
|Btn\_SelectFile|NativeFilePickerController.OpenFilePicker()|

\---

