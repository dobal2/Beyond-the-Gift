# Beyond the Gift - 2024 게임잼 2nd

> 2024년 12월 게임잼 작품 | 개발 기간: 5일 (2024.12.15 ~ 12.19) | Unity 2D

주문서를 확인하며 선물을 포장해 썰매에 실어 보내는 2D 캐주얼 게임으로, 드래그앤드롭 인터랙션과 주문 검증 시스템 설계에 집중했습니다.

---

## 게임 플레이

1. 오른쪽 주문서에서 아이가 원하는 **선물 종류 / 사탕 / 포장지** 확인
2. 선물 선택 → 사탕 데코 → 포장지 선택 → 스티커 / 편지 추가 (선택)
3. 완성된 선물을 썰매로 드래그해서 배달
4. 정답이면 쿠키 획득, 오답이면 페널티
5. 스테이지가 올라갈수록 제한 시간 압박과 쿠키 차감 패널티 증가

### 스테이지 구성

| 스테이지 | 시작 쿠키 | 제한 시간 | 페널티 |
|--------|---------|---------|------|
| 1      | 1,000   | 없음    | 없음 |
| 2      | 1,500   | 100초   | 10초마다 쿠키 -100 |
| 3      | 1,500   | 150초   | 5초마다 쿠키 -150 |

---

## 팀 구성

| 이름 | 역할 |
|------|------|
| 서재민 (dobalman) | 게임 매니저, 주문 UI, 드래그앤드롭 인터랙션, 쇼핑 시스템, 편지 그리기, UI 그래픽, 비주얼 이펙트, 아웃라인 효과 |
| GaeZZang | 선물 시스템 (Decorator 패턴), 주문 생성/검증, MVP 구조, 썰매 이동 |

---

## 기술 스택

- **Unity** 2022.3 LTS / C#
- **TextMeshPro** — UI 텍스트
- **DOTween** — UI 애니메이션
- **ScriptableObject** — 선물·상점 아이템 데이터

---

## 아키텍처

### 디자인 패턴

**Decorator 패턴 — 선물 조합**
```
Gift (abstract)
├── Book / Car / Game / Money / Doll
└── CondimentDecorator (abstract)
    ├── CandyDecorator   — 사탕 추가
    └── WrapperDecorator — 포장지 추가
```
선물 본체에 사탕과 포장지를 런타임에 조합해 최종 GiftData 목록을 생성합니다.

**MVP 패턴 — 제품 UI**
```
ProductModel     — 선물 상태 (isGiftReady / isCandyReady / isWrapperFinished)
ProductPresenter — Model ↔ View 연결, 버튼 액션 처리
ProductView      — 버튼 입력 수신, 텍스트 피드백 표시
IProductView     — View 인터페이스
```

**Singleton 패턴 — GameManager**
```
GameManager.Instance
├── 스테이지 진행 및 쿠키 경제
├── 시간 트래킹 / 페널티 계산
└── UI 패널 열기/닫기 통합 관리
```

### 스크립트 구조

```
Assets/
├── Scripts/                        # GaeZZang 담당
│   ├── Gift/
│   │   ├── Gift.cs                 # 선물 추상 베이스
│   │   ├── {Book,Car,Game,Money,Doll}.cs
│   │   └── Condiments/
│   │       ├── GiftData.cs         # 선물 종류 열거형
│   │       ├── CondimentDecorator.cs
│   │       ├── CandyDecorator.cs
│   │       └── WrapperDecorator.cs
│   ├── Order/
│   │   ├── Order.cs                # 주문 생성 및 정답 검증
│   │   ├── ChildData.cs            # 단일 주문 데이터
│   │   └── MovingSled.cs           # 썰매 SmoothDamp 애니메이션
│   └── Product/
│       ├── GUI/
│       │   ├── IProductView.cs
│       │   ├── ProductModel.cs
│       │   ├── ProductPresenter.cs
│       │   └── ProductView.cs
│       └── RollingUI.cs            # 제품 패널 슬라이드 애니메이션
│
└── Name/Jaemin/Scripts/            # 서재민 담당
    ├── GameManager.cs              # 게임 전체 흐름 (Singleton)
    ├── OrderDisplay.cs             # 주문 카드 UI 갱신 및 정답 처리
    ├── GiftDragger.cs              # 선물 드래그앤드롭 → 썰매 투하
    ├── GiftDoubleClick.cs          # 더블클릭으로 선물 초기화
    ├── GiftReinitializer.cs        # 포장 완성 후 선물 오브젝트 복제
    ├── Sticker.cs                  # 스티커 드래그앤드롭 배치
    ├── DrawMesh.cs                 # 편지 자유 그리기 (영역 클리핑 포함)
    ├── CookieDisplay.cs            # 쿠키 잔액 실시간 표시
    ├── Shop/
    │   ├── ShopManager.cs          # 쿠키 소비 구매 + PlayerPrefs 영속 저장
    │   └── ShopSlot.cs
    └── ScriptableObjectScript/
        ├── OrderGiftScriptable.cs  # 선물 이름/설명/아이콘
        └── ShopItem.cs             # 상점 아이템 데이터
```

---

## 주요 기술

- `IDragHandler` / `IPointerUpHandler`와 `GraphicRaycaster`를 활용한 드래그앤드롭 및 썰매 투하 판정 구현
- Decorator 패턴을 적용해 선물·사탕·포장지를 런타임에 조합하는 주문 검증 시스템 설계
- `GraphicRaycaster` 기반 프레임별 영역 체크로 편지 UI 밖 그리기 차단 구현
- `Instantiate`를 활용한 선물 오브젝트 복제 및 스티커·편지 상태 이전 흐름 구현
- `PlayerPrefs`를 활용한 상점 구매 데이터 영속 저장 구현

---

## 빌드 & 실행

Unity 2022.3 이상에서 프로젝트를 열고 `Assets/Scenes/`의 메인 씬을 실행합니다.
