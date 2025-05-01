## 설계 범위
### Usecase Diagram
```mermaid
graph LR
  %% Actor
  User((User))

  %% System Boundary

  subgraph Play 씬 활동
    direction LR
    UC_PlayStage([스테이지 플레이])
    UC_BuildManageTower([타워 건설/관리])
    UC_UpgradeUnitInGame([게임 중 유닛 업그레이드])
    UC_TriggerWave(["웨이브 시작 (수동/자동)"]) 
  end

  subgraph Main Lobby 씬 활동
    direction LR
    UC_ManageLobbyUpgrade([로비 업그레이드 관리])
    UC_StartGame([게임 시작/스테이지 선택])
  end

  %% User Interactions
  User -- "게임 시작" --> UC_StartGame
  User -- "영구 업그레이드" --> UC_ManageLobbyUpgrade
  User -- "타워 관련 조작" --> UC_BuildManageTower
  User -- "게임 중 업그레이드" --> UC_UpgradeUnitInGame
  User -- "웨이브 시작 명령" --> UC_TriggerWave

  %% Relationships between Use Cases (Optional: Use <<include>> or note for clarity)
  UC_StartGame -- "포함 (includes)" --> LoadingProcess(["리소스 로딩"])
  UC_StartGame -- "포함 (includes)" --> MapGeneration(["맵 생성"])
  UC_PlayStage -- "포함 (includes)" --> UC_BuildManageTower
  UC_PlayStage -- "포함 (includes)" --> UC_UpgradeUnitInGame
  UC_PlayStage -- "포함 (includes)" --> BattleProcess(["전투 진행 (자동)"])
  UC_TriggerWave -- "전투 시작/가속" --> BattleProcess


  %% Notes for clarification
  note_PlayStage["목표 달성, 전투 진행 등"]
  UC_PlayStage --> note_PlayStage

  note_StartGame["로비에서 Play 씬으로 이동<br/> 필요한 초기화 수행<br/> (로딩, 맵 생성 등)"]
  UC_StartGame --> note_StartGame

  note_LobbyUpgrade["영구적인 능력치/기능 업글"]
  UC_ManageLobbyUpgrade --> note_LobbyUpgrade

  note_BuildTower["타워 건설<br/> 업그레이드<br/> 판매 등"]
  UC_BuildManageTower --> note_BuildTower

  %% Internal Processes (Represented as notes or separate nodes for clarity, not primary use cases)
  style LoadingProcess fill:#lightgrey,stroke:#333,stroke-width:1px
  style MapGeneration fill:#lightgrey,stroke:#333,stroke-width:1px
  style BattleProcess fill:#lightgrey,stroke:#333,stroke-width:1px
  style NetworkMgmt fill:#lightgrey,stroke:#333,stroke-width:1px

  InternalProcesses["내부 처리 (자동)"] -- 네트워크 연결/ 데이터 관리 --> NetworkMgmt(["네트워크 연결/ 데이터 관리"])
```

## 네임스페이스 설계
**분리 목적:** 각 모듈의 책임을 명확히 하고, 의존성을 줄여 유지보수성과 재사용성을 높이기 위해 네임스페이스를 분리했습니다.
### Package Diagram
```mermaid
graph TD
    A[Project] --> Core;
    A --> Data;
    A --> GamePlay;
    A --> Network;
    A --> UI;
    A --> Audio;
    A --> Input;
    A --> Utility;
    A --> Test;
    A --> Editor;
  style Test fill:#f9d,stroke:#333,stroke-width:2px
  style Editor fill:#f9d,stroke:#333,stroke-width:2px
```
#### 1. Core
역할: 게임의 전체 적으로 필요한 초기화 관리.</br>
주요 내용: 게임 초기화 관리 

#### 2. Data
역할: 게임 설정, 초기값, 영구 데이터 정의 및 관리.</br>
주요 내용: ScriptableObject 정의 (스탯, 정보 등), 설정 값, 데이터 테이블 구조, 데이터 저장/로드 인터페이스/기본 핸들러.

#### 3. GamePlay
역할: 실제 인게임 플레이 로직의 대부분. 핵심 게임 메커니즘 구현.</br> 
주요 내용: 타워/적 로직 및 AI, 전투 시스템, 맵 시스템, 플레이어 상태(인게임), 로그라이크 요소, 아이템/스킬 로직. 게임 상태 관리, 씬 로딩, 핵심 게임 루프 관리.

#### 4. Network
역할: 네트워크 관련 기능.</br> 
주요 내용: Firebase 연동.

#### 5. UI
역할: 사용자 인터페이스 요소 표시, 상호작용, 로직. </br>
주요 내용: 화면(View/Screen) 관리, 재사용 위젯(Button, Popup 등), UI 애니메이션/효과, UI 이벤트/데이터 바인딩.

#### 6. Audio
역할: 게임 내 모든 사운드 및 음악 재생 관리. </br>
주요 내용: AudioManager, BGM/SFX 재생, 오디오 풀링, 믹서/볼륨 조절.

#### 7. Input
역할: 플레이어 입력(키보드, 마우스, 터치, 패드) 처리 및 해석.</br>
주요 내용: 입력 감지/이벤트 발행, 입력 매핑, 플랫폼별 입력 처리.

#### 8. Utility
역할: 범용 헬퍼 함수, 확장 메소드, 유틸리티 클래스.</br>
주요 내용: 유틸리티

#### 9. Test
역할: Unity 에디터 환경에서만 사용되는 스크립트. </br>
주요 내용: 테스트 코드.

#### 10. Editor
역할: Unity 에디터 환경에서만 사용되는 스크립트. </br>
주요 내용: 커스텀 에디터.

---

## UI
**설계:** UI는 처리해야 할 데이터가 많거나 향후 확장 가능성이 높은 경우, 재사용성과 유지보수성, 테스트 용이성을 높이기 위해 **MVVM(Model-View-ViewModel)** 구조를 채택했습니다.</br>
반면, 구조가 단순하고 변경 가능성이 낮은 UI는 MonoBehaviour 기반으로 간결하게 설계하였습니다 개발 효율성을 높였습니다.

MVVM 구조에서 Model은 Repository 패턴을 사용하여 데이터를 관리하고, Repository는 DI(Dependency Injection)을 통해 주입받는 방식으로 설계하였습니다.
### UI Class Diagram
```mermaid
classDiagram
    class MonoBehaviour {
        <<Unity Engine>>
    }

    class Test.UITest {
        + TestFunc()
    }
subgraph VVM
    class View {
        <<MonoBehaviour>>
        - UnityUIReference
        - ViewModel _viewModel // DI로 Inject

        - void Awake()
        - void OnDestroy()
        - void UpdateUI(Data )
    }

    class ViewModel {
        - IRepository _repo // DI로 Inject
        + event Action OnDataChanged // View가 구독

        + void SetData(data)  // 외부에서 호출
        - void NotifyViewDataChanged() // View에 알림
    }
end
subgraph DataAcess
    class Data.IRepository {
        <<interface>>
        + SetValue(Data)
        + GetValue() Data
    }
    class Data.Repository {
        - Model
        + SetValue(Data)
        + GetValue() Data
    }
    class Data.Model {
        + Data
        + Set(data) // 데이터 변경
    }
end


    MonoBehaviour <|-- View 
    View o--> ViewModel

    ViewModel o..> Data.IRepository
    Data.IRepository <|-- Data.Repository
    Data.Repository o--> Data.Model
    
    Test.UITest ..> ViewModel : tests
    

    %% Notes:
    %% o-- : Composition/Aggregation 
    %% --|> : Inheritance 상속
    %% ..> : Dependency / Interaction 
```

## DI(Dependency Injection)
의존 주입 분류 **(모든 Installer는 Core 단에서 초기화)**
#### (Project 단위)
- Manager Class: 전역으로 관리해야 되는것들을 AsSingle로 관리
- Repository Class: Data를 Repository 패턴으로 관리, 각 상황에 맞게 Bind
#### (Scene 단위)
- (SceneName)Scene Class: 각 Scene에서 사용되는 ViewModel을 AsTransient로 Bind (생성을 요청 받았을때만)
- Scene Class: 각 씬에 들어가면 새로 Bind하여 관리함 (예: MainCanvas 등)
