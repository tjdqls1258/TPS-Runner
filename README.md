# TPS Runner

### 🧠 Core Architecture: 데이터 조립 기반 FSM

본 프로젝트의 핵심 AI 및 상태 제어 시스템은, 하드코딩된 일반적인 FSM의 한계(클래스 폭발, 강한 결합도)를 극복하기 위해 설계된 **'데이터 조립 기반의 제네릭 FSM'**입니다. 프로그래머가 코드를 수정하는 대신, 기획자가 인스펙터에서 레고 블록처럼 상태를 조립할 수 있는 환경을 구축했습니다.

#### 1. ScriptableObject를 활용한 로직 에셋화
* **기능 단위의 부품화**: 상태(State)를 하나의 거대한 클래스로 만들지 않고, 실행할 행동(`ActionSO`)과 상태 전환 조건(`ConditionSO`)으로 완전히 분리하여 ScriptableObject로 에셋화했습니다.
* **ScriptableObject 적용**: 수십 마리의 몬스터나 투사체가 스폰되더라도, 공통된 행동 로직(예: `AnimationActionSO_move`)은 **메모리에 단 하나의 인스턴스만 존재**하도록 공유하여 메모리 할당 및 가비지 컬렉션(GC)을 극단적으로 최적화했습니다.

#### 2. 제네릭 `<T>` Context 주입을 통한 의존성 역전(DIP)
* **로직과 데이터의 완벽한 분리**: 에셋(SO)은 런타임 데이터를 가질 수 없다는 한계를 극복하기 위해, 개별 객체의 데이터(속도, 현재 체력, 애니메이터 등)를 담는 `Context` 클래스를 별도로 설계했습니다.
* **범용성 확보**: `StateMachine<T, TState>` 제네릭 구조를 통해, 상태 로직은 어떤 객체인지 알 필요 없이 오직 주입받은 `Context`의 값만 연산합니다. 이 단일 FSM 엔진 하나로 플레이어 액션, 몬스터 AI, UI 상태 흐름까지 모두 처리합니다.

#### 3. SOLID 원칙과 실무적 확장성 (OCP)
* 새로운 형태의 보스 패턴이나 액션이 추가되어도, **기존의 FSM 매니저 코드는 단 한 줄도 수정할 필요가 없습니다.**
* 필요한 행동(`Action`)과 조건(`Condition`) 데이터 스크립트만 새로 작성한 뒤, 유니티 에디터 인스펙터 상에서 시각적으로 조립(Assembly)하여 새로운 상태 전이 흐름을 만들어낼 수 있는 완벽한 데이터 주도(Data-Driven) 설계를 완성했습니다.


```c#
﻿using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/MoveAnimation")]
public class AnimationActionSO_move : ActionSO<PlayerContext>
{
    [Tooltip("값이 목표치에 도달하는 데 걸리는 시간. (0.1 ~ 0.2 추천. 클수록 미끄러지듯 변함)")]
    [SerializeField] private float m_dampTime = 0.1f;

    public override void OnUpdate(PlayerContext context)
    {
        if (context.animator == null) return;

        Vector3 horizontalVelocity = new Vector3(context.Velocity.x, 0, context.Velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // 핵심: 매개변수가 4개인 SetFloat을 사용합니다!
        // (파라미터 해시, 목표 값, 댐핑 시간, 델타 타임)
        if(currentSpeed == 0f)
            context.animator.SetFloat(context.moveHash, currentSpeed);
        else
            context.animator.SetFloat(context.moveHash, currentSpeed, m_dampTime, Time.deltaTime);
    }
}
```
