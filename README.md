# Sparta-Rider
 
## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [팀소개](#팀-소개)
3. [개발기간](#개발기간)
4. [와이어프레임](#와이어프레임)
5. [주요기능](#주요기능)
6. [기술스택](#기술스택)

## 프로젝트 소개
### 프로젝트 제목 : 터보 레이서 (Turbo Racer)
![F7A3C859-326A-4EEE-9BA3-20897359F005](https://github.com/user-attachments/assets/5c99b403-88af-4b7d-a1e3-1a3d927b565c)
### 장르 : 레이싱
플레이어는 주행에 도움이 되는 아이템만을 먹어 누구보다 빠르게 완주하세요.
### 기본 조작법
![EED5807D-0A4D-4AFD-BA5E-53899385F471](https://github.com/user-attachments/assets/1ce8eda9-68f1-4844-b5e1-d4901398f2b9)

### 아이템 소개
#### 주행 방해 아이템
1. 토마토 : 시야 방해

![C8E42977-8A0E-4DB9-9B58-3D0CF804AB94_4_5005_c](https://github.com/user-attachments/assets/c0030f88-45d6-440b-b9b3-99419ed9173b)

2. 바나나 : 차량 3바퀴 회전

![72998A91-EDE3-4BA7-AA08-FA6F88E77E3E_4_5005_c](https://github.com/user-attachments/assets/f5d1cba1-82c8-4900-8508-0b46afc6d6d4)

3. 컵 케익 : 차량 속도 절반 감속

![ABF1B9FD-E525-47F8-A72D-2A894B5A3AFD_4_5005_c](https://github.com/user-attachments/assets/2ee024c6-59cc-414b-9555-1fdc2d2748f1)

4. 수박 : 차량 3초 일시 정지

![B79F7128-2C81-4169-B806-C43028FD8EDC_4_5005_c](https://github.com/user-attachments/assets/e8ebbe15-5ed2-49ba-80c2-022780c5f655)

5. 버섯 : 전진 후진 반전

![FD4B7B95-A014-490A-B9BC-1C831097C7D9_4_5005_c](https://github.com/user-attachments/assets/f109ef8c-04ab-47eb-a19d-c699945c0e23)

#### 주행 도움 아이템
1. 커피 : 차량 속도 2배 증가

![F2820B1C-7AE6-4451-97EC-1BF71500E50E_4_5005_c](https://github.com/user-attachments/assets/5ed3c736-3822-4770-ad76-80375592154a)

### 플레이어 맵 제작
#### 맵 제작 기본 조작법
![0376CC0C-5893-401E-8B03-5104B5CE5196](https://github.com/user-attachments/assets/95eaf234-137b-4d2c-809b-7cbcbcf26ff7)

## 팀 소개
<a href="https://github.com/xoxohoon01/Sparta-Rider/graphs/contributors">
<img src = "https://contrib.rocks/image?repo=xoxohoon01/Sparta-Rider">
</a>

## 개발기간
- 2024.11.15(금) ~ 2024.11.22(금)

## 와이어프레임
![F4810B09-FE3C-436F-90C6-BE3B6BE80A6F](https://github.com/user-attachments/assets/17ea84b4-0768-4e8e-9ab1-a90d8c517d0a)
![77E4DFE7-2DAB-40F1-A306-EF635C39B758](https://github.com/user-attachments/assets/7d1ae2a0-bc6d-41f3-bc69-79c84f5b3c82)

## 주요기능
|기능 이름|기능 설명|스크립트|
|:---:|:---:|:---:|
|자동차 움직임|- VehicleController라는 스크립트에서 차량의 움직임을 제어 - Update 에서 전진, 후진, 스티어링, 드리프트 등의 기능 구현 - Input System을 통해 InputValue를 받아와, 상태를 변경|[VehicleController.cs] (https://github.com/xoxohoon01/Sparta-Rider/blob/dev/Assets/Scripts/Vehicles/VehicleController.cs)|
|UI 동적 로딩 |동적으로 UI를 생성하려면 프리팹으로 준비한 뒤, Instantiate를 사용해 필요한 위치에 추가해주면 된다.|[UICreateMap.cs](https://github.com/xoxohoon01/Sparta-Rider/blob/dev/Assets/Scripts/UI/UICreateMap.cs)|
|아이템생성|현재 자식 객체가 있다면 객체를 지운다. ObjectPool에서 아이템을 가져온다. 현재 Point에 자식 객체로 설정한다|[ItemSpawnPoint.cs](https://github.com/xoxohoon01/Sparta-Rider/blob/dev/Assets/Scripts/Item/ItemSpawnPoint.cs)|
|아이템효과|아이템에 충돌이 발생할 때 Randerer 및 Collider를 비활성 시킵니다. 아이템 효과를 발동시킵니다.|[ItemContactController.cs](https://github.com/xoxohoon01/SpartaRider/blob/dev/Assets/Scripts/Item/Controller/ItemContactController.cs)
|맵  - 골인 지점, 중간 포인트|골인지점 - 중간 포인트를 거쳤는지 HashSet로 검사. 모든 중간 포인트를 지났다면 골인 지점을 통과했을 때 Lap++ 스폰포인트 - 마지막으로 거친 중간 포인트 or 골인 지점이 스폰 포인트가 된다.|[MidPointManager.cs](https://github.com/xoxohoon01/Sparta-Rider/blob/dev/Assets/Scripts/Managers/MidPointManager.cs)
|맵  - UI|현재 Lap - 통과한 골인 지점마다 ++, Lap Time - 골인 지점을 지난 이후로 얼마나 시간이 흘렀는지 Time.time - lapStartTime으로 측정, 현재 속도 - velocity의 크기를 받아와 소수점을 버린 숫자를 표시|[MidPointManager.cs](https://github.com/xoxohoon01/Sparta-Rider/blob/dev/Assets/Scripts/Managers/MidPointManager.cs)

## 기술 스택
- 언어
  - C#
- 게임 엔진
  - Unity - 2022.3.17f1
- 버전 관리
  - GitHub
- 개발 환경
  - IDE
    - VisualStudio
    - Rider: JetBrains
  - OS
    - Window 10
    - MacOS
