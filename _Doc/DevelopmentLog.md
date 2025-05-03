## 개발 일지
#### 개발 과정
#### 25.04
- [2025.04.19 / 시스템 구조 설계](#전체-시스템-구조-설계)
- [2025.04.19 / UI 설계 MVVM](#ui-설계)
- [2025.04.19 / DI 선택](#didependency-injection)
- [2025.04.24 / 확장성 고려 비지니스 로직](#확장성-고려)
- [2025.04.28 / Core System 설계](#game-play-system-설계)
#### 25.05

---
#### 2025.04.19
### 전체 시스템 구조 설계
초기 기획 후 최우선 과제는 '전체 시스템을 어떤 구조로 구성할 것인가'를 결정하는 것이었습니다.</br> 
먼저 코드의 역할 기준으로 네임스페이스를 나누고 각 네임스페이스가 서로 어떻게 의존할지(의존성 방향)을 설정했습니다.
```mermaid
graph LR
    Core;
    Data;
    GamePlay;
    Network;
    UI;
    Audio;
    Utility;

UI --> Data
UI --> Utility

GamePlay --> UI
GamePlay --> Data
GamePlay --> Audio
GamePlay --> Network
GamePlay --> Utility

Audio --> Data;

Core --> GamePlay
Core --> UI
Core --> Network
Core --> Data
```
```mermaid
graph LR
 EditorOnly --> Test;
    EditorOnly --> Editor;
  style Test fill:#f9d,stroke:#333,stroke-width:2px
  style Editor fill:#f9d,stroke:#333,stroke-width:2px
```

- **Core:** 게임의 초기화 담당
- **Data:** 사용되는 데이터, Addressable 로드
- **GamePlay:** 게임 핵심 로직
- **Network:** Firebase연동
- **UI:** User Interface
- **Audio:** 사운드 관리
- **Utility:** Helper, Utills, DesignPattern 등
- ${\textsf{\color{magenta}Editor}}$
  - **Test:** Test 코드 묶음
  - **Editor:** Editor 환경에서 사용 하는 코드

이렇게 정의된 기준과 구조를 바탕으로 코드 작성을 진행할 계획을 했습니다.

---
#### 2025.04.19
### UI 설계
UI(User Interface)를 MVVM(Model-View-ViewModel) 구조로 선택한 이유는 다음과 같습니다.
- **명확한 역할 분리**
    - **View:** 사용자에게 보여지는 UI 요소(버튼, 텍스트, 이미지 등)와 최소한의 View 관련 로직(애니메이션 등)만 담당합니다.
    - **ViewModel:** View를 위한 데이터, View로부터의 사용자 입력(커맨드)을 처리합니다. Model로부터 데이터를 가져와 View가 사용하기 쉬운 형태로 가공합니다.
    - **Model:** 애플리케이션의 데이터와 비즈니스 로직을 담당하며, 독립적 구조를 가지고 있습니다.
- **테스트 용이성:** ViewModel이 View에 직접 의존하지 않아 결합도가 낮습니다. 덕분에 UI 요소 없이도 ViewModel의 로직을 검증하는 테스트 코드를 작성하기가 훨씬 수월해서 이 패턴을 선택했습니다.

---
#### 2025.04.19
### DI(Dependency Injection)
DI(Dependency Injection) 도구로는 [Zenject](https://github.com/modesttree/Zenject)를 사용했습니다. </br>
DI를 사용한 핵심 이유는 다음과 같습니다.
- 중앙화된 초기화 관리: Singleton class 객체들의 생성 및 초기화 시점을 일관되게 관리하여, 모든 씬에서 누락 없이 안정적으로 사용할 수 있도록 하고 싶었습니다.
- 안정적인 의존성 관리: 특히 데이터 관련 객체들의 생성, 초기화, 그리고 필요한 곳에서의 참조 과정을 DI를 통해 명확하고 안정적으로 처리함으로써 의존성 누락, 잘못된 참조 같은 잠재적인 실수를 줄이고자 했습니다.
  

주로 DI로 관리한 항목은 다음과 같습니다.
- **Manager Class:** Singleton 항목을 관리. (예: DataManager, GameManager ...)
- **Repo/Data Class:** Repository 패턴, Data를 관리.
- **Scene Data:** Scene에 핵심적으로 사용되는 Main Canvas, UI 등을 관리

---
#### 2025.04.24
### 확장성 고려
나중에 변경, 확장될 부분 중 비지니스 로직 부분은 Strategy 패턴으로 분리했습니다. </br>
**ex)  인풋을 체크하는 로직**
```mermaid
classDiagram
graph TD
class IInputStrategy {
    <<interface>>
    + UpdateInput()
    + GetLogics()
}
class PcInputStrategy {
    + UpdateInput()
    + GetLogics()
}
class MobileInputStrategy {
    + UpdateInput()
    + GetLogics()
}

IInputStrategy <|-- PcInputStrategy
IInputStrategy <|-- MobileInputStrategy

%% Notes:
%% --|> : Inheritance 상속
```
Mobile 환경과 PC 환경에서 다른 Input 처리를 하기 위해 분리했습니다.
```C#
// 사용 예시
#if UNITY_EDITOR
    IInputStrategy inputStrategy = new PcInputStrategy();
#elif UNITY_ANDROID || UNITY_IOS
    IInputStrategy inputStrategy = new MobileInputStrategy();
#else // Mobile 환경이 아니면
    IInputStrategy inputStrategy = new PcInputStrategy();
#endif
    _inputSystem.SetInputStrategy(inputStrategy); // set 설정
```

---
#### 2025.04.28
### Game Play System 설계
Game Play에 필요한 핵심 System들을 설계 하였습니다.
```mermaid
classDiagram
class PlaySceneSystemManager {
    - InitializeSystem()
}
class EnemySystem {
     + ControllEnemys()
}
class WaveSystem {
    + SpawnEnemiesWave()
}
class StageSystem {
    + StartStage()
    + EndStage()
}
class MapSystem {
    - MapDatas
    - PathList
    + GenerateMap(x,y)
}
class ScreenClickInputSystem {
    + UpdateInput()
}
class CameraSystem {
    + HandleCameraMovement()
}
class TowerSystem {
    - TowerList
    + CreateTower(type)
    + RemoveTower(type)
    + SwapTower(p1 : float2, p2: float2)
}
class UpgradeSystem {
    + UpgradeTower()
}

PlaySceneSystemManager --> ScreenClickInputSystem : manages
PlaySceneSystemManager --> EnemySystem : manages
PlaySceneSystemManager --> WaveSystem : manages
PlaySceneSystemManager --> StageSystem : manages
PlaySceneSystemManager --> CameraSystem : manages
PlaySceneSystemManager --> TowerSystem : manages
PlaySceneSystemManager --> MapSystem : manages
```
PlaySceneSystemManager에서 GamePlay에 필요한 System 들을 설계 하였습니다.</br>
각 System의 역할을 다음과 같습니다.</br>
- **MapSystem:** 맵 데이터 생성, 맵 오브젝트 생성
- **ScreenClickInputSystem:** Input 관리
- **EnemySystem:** Enemy 행동 제어
- **StageSystem:** 스테이지 시작과 종료
- **WaveSystem:** 스테이지 Level에 맞는 Wave(enemy) 생성
- **CameraSystem:** 카메라 제어
- **TowerSystem:** 타워 생성, 제거, 위치 변경
- **UpgradeSystem:** 업그레이드

우선 Stage, Input-Camera System의 Flowchart를 구성했습니다.
```mermaid
flowchart TD

subgraph Play Scene
    A[Load Play Scene] --> B(Systems: Initialize)
end

subgraph Input-Camera
    IS_Input[InputSystem: UpdateInput] -- Input Data --> CS_Camera[CameraSystem: CameraHandle]
end

subgraph Stage Logic
    direction TB

     subgraph Stage End Flow
        direction TB
        eSS_End[StageSystem: EndStage] -- ClearEnemyData --> eES_Enemy[EnemySystem: ClearData]
    end
    subgraph Stage Start Flow
        direction TB
        SS_Start[StageSystem: StartStage] -- Level Data --> WS_Wave[WaveSystem: SpawnEnemiesWave]
        WS_Wave -- Enemies Data --> ES_Enemy(EnemySystem: SetEnemyData)
    end

   
end
```

---
