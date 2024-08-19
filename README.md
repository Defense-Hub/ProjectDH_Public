<div align="center">
<img src="https://github.com/user-attachments/assets/dbcfeed1-418b-4926-81db-ecbcd2a21faf" alt="coding" width="450px" />  
<br/>    
<br/>    
    
### Help Me Guardians Project
<br/>    
<br/> 

</div>

## 🙋‍♂️ 팀 소개

<p>
<a href="https://github.com/LuBly">
  <img src="https://github.com/LuBly.png" width="150">
</a>
<a href="https://github.com/mwomwo1">
  <img src="https://github.com/mwomwo1.png" width="150">
</a>
<a href="https://github.com/JeongEunJi1127">
  <img src="https://github.com/JeongEunJi1127.png" width="150">
</a>
<a href="https://github.com/seungdo1234">
  <img src="https://github.com/seungdo1234.png" width="150">
</a>
</p>

 `Team`  **ProjectDH** 
 
 `Members` **박현호 윤세나 정은지 지승도**

 `Stack` **Unity, C#**   

 `Develop`  **2024.06 ~ 2024.08**   

 `itch.io` [itch.io](https://defensehub-a.itch.io/help-me-guadians)  



<br/>    
<br/> 

## 🧾 목차
  
[게임 개요](#-게임-개요)  
[게임 소개](#-게임-소개)  
[주요 사용 기술](#-주요-사용-기술)  
[개발 진행](#-개발-진행)  
[개발 결과](#-개발-결과)  
[프로젝트 개선](#-프로젝트-개선)  
[트러블 슈팅](#-트러블-슈팅)  
  
<br/>    
<br/>   
      
## 😃 게임 개요 

`Genre` 3D 디펜스 게임  

`Description` 다양한 유닛을 소환하고 조합하여 각기 다른 특성을 가진 보스들을 물리치는 게임입니다 ⚔️

    

<br/>    
<br/>  

## 💝 게임 소개
<img src="https://github.com/user-attachments/assets/9ba990dd-5c8f-4e36-92b2-da68896412df" alt="coding" width="550px" />

<br/>
<br/>

### 📽️ 게임 영상
 ⬇ `Youtube Link `⬇   
[![Video Label](http://img.youtube.com/vi/dXaxCyhyKd8/0.jpg)](https://www.youtube.com/watch?v=dXaxCyhyKd8)

<br/>    
<br/>  

## 🛠 주요 사용 기술
<img src="https://github.com/user-attachments/assets/86726e67-8475-4521-8911-c29f66484b4f" alt="coding" width="550px" />

<br/>    
<br/>  
<br/>  


## 🧾 개발 진행

### 🛠 FSM
#### 💡 사용 이유
- **유닛과 Enemy의 상태를 유기적으로 관리하기 위함**

#### 📌 사용 방법
-  **아래와 같은 구조로 사용**  
<img src="https://github.com/user-attachments/assets/7268aa19-c270-452c-8abe-67abcd2ef54b" alt="coding" width="550px" />

---

### 🛠 Custom Editor Tool
#### 💡 사용 이유
- **유닛 혹은 적을 테스트 하거나 개발 편의성을 위한 커스텀 툴 개발**  

#### 📌 사용처
1️⃣ **특정 유닛 소환**  
<img src="https://github.com/user-attachments/assets/b51a4014-0b4c-421f-acf1-19f50f4deef7" alt="coding" width="550px" />  
2️⃣ **특정 보스 소환**  
<img src="https://github.com/user-attachments/assets/0606b1db-4e7d-4975-be1a-a2d2046f55a5" alt="coding" width="550px" />  
3️⃣ **씬 전환**  
<img src="https://github.com/user-attachments/assets/02df5b8f-cf67-41d2-99a0-02200902215a" alt="coding" width="550px" />  
4️⃣ **유닛 스텟 조절**  
<img src="https://github.com/user-attachments/assets/b623efd9-00fc-4e59-88ca-39e0b3db0142" alt="coding" width="550px" />  

---

### 🛠 Addressable
#### 💡 사용 이유
- **메모리 효율성과 유연한 리소스 관리를 위해 사용**  

#### 📌 구현 방법
1️⃣ **IResourceLocation으로 어드레서블 그룹에 저장된 리소스들의 경로를 저장**  

    public Dictionary<int, IList<IResourceLocation>> LocationDict { get;  private set; }  
    
2️⃣ **각각의 리소스 이름으로 정의된 Enum 타입 순서대로 IResourceLocation 리스트 재 정렬**  
<img src="https://github.com/user-attachments/assets/9c196c19-9f96-4e05-ad1c-2e0c0290b631" alt="coding" width="550px" />  
3️⃣ **IResourceLocation 정보를 매개변수로 어드레서블에 저장된 리소스를 로드**  
<img src="https://github.com/user-attachments/assets/ed9a50b9-017e-4e1a-b220-6a4d28e37b69" alt="coding" width="550px" />  

---

### 🛠 Custom Editor Tool
#### 💡 사용 이유
- **유닛 혹은 적을 테스트 하거나 개발 편의성을 위한 커스텀 툴 개발**  

#### 📌 사용처
1️⃣ **특정 유닛 소환**  
<img src="https://github.com/user-attachments/assets/b51a4014-0b4c-421f-acf1-19f50f4deef7" alt="coding" width="550px" />  
2️⃣ **특정 보스 소환**  
<img src="https://github.com/user-attachments/assets/0606b1db-4e7d-4975-be1a-a2d2046f55a5" alt="coding" width="550px" />  
3️⃣ **씬 전환**   
<img src="https://github.com/user-attachments/assets/02df5b8f-cf67-41d2-99a0-02200902215a" alt="coding" width="550px" />  
4️⃣ **유닛 스텟 조절**  
<img src="https://github.com/user-attachments/assets/b623efd9-00fc-4e59-88ca-39e0b3db0142" alt="coding" width="550px" />  

---

### 🛠 Batching
#### 💡 설명
- **Batch 최적화를 위해 Static Batching, Sprite Atlas, Terrain To Mesh 사용**  
<img src="https://github.com/user-attachments/assets/b9a3fa67-d9cc-4520-b002-65fdf79d98cc" alt="coding" width="550px" />  

---

### 🛠 UI 동적 생성
#### 💡 사용 이유
- **다양한 게임 상황에 따른 UI 관리**  
- **리소스 관리 최적화**  
- **유연한 확장성**

#### 📌 구현 내용
1️⃣ UIManager  
- UI의 생성, 관리, 제거를 담당하는 중심 역할  
- Singleton 패턴을 사용하여 게임 전체에서 유일하게 존재하는 UI 매니저 인스턴스를 제공함으로써 UI의 중앙 집중식 관리를 구현  
  
2️⃣ ShowPopupUI<T> 메서드  
- 특정 UI 팝업을 비동기로 생성  
- ResourceManager를 통해 해당 UI 타입의 게임 오브젝트를 불러오고, 이를 UIManager의 Root 오브젝트 하위에 배치  
- 제네릭 타입을 사용하여 다양한 팝업 UI를 유연하게 생성
  
3️⃣ SetCanvas 메서드  
- 동적으로 생성된 UI 오브젝트가 Canvas를 포함하도록 설정하는 역할  
- Canvas의 renderMode를 `ScreenSpaceOverlay`로 설정하여 UI가 화면에 항상 오버레이 되도록 하며, 필요한 경우 정렬 순서를 지정하여 UI가 올바르게 렌더링되도록 함

4️⃣ UI_Base 클래스   
- UI 컴포넌트를 관리하는 기본 클래스  
- 다양한 UI 요소를 효율적으로 관리하기 위해, UI 요소를 Dictionary에 저장하고, 필요할 때 타입과 인덱스를 통해 해당 UI 요소를 가져옴  
- UI 요소의 이벤트 핸들러를 추가하는 기능 포함
  
5️⃣ UI_EventHandler 클래스  
- UI의 클릭 및 드래그 이벤트를 처리하는 클래스  
- 이 클래스는 Unity의 이벤트 시스템과 통합되어, 다양한 UI 상호작용을 관리  

---

### 🛠 Unity Gaming Service Analytics
#### 💡 사용 이유  
- **User Test 에서 실제 플레이 환경의 이벤트를 전송 받아 유저 데이터를 확보, 
개선 방향을 수립하기 위해 사용**  

#### 📌 사용처
<img src="https://github.com/user-attachments/assets/6a8fd9ca-b607-469d-a353-ab68bfae453d" alt="coding" width="550px" />  
⬆︎ 유저들의 이탈 시점 Data  
<br/>

> SQL 탐색기로 게임 종료 시 어느 스테이지까지 클리어 했는지에 대한 RawData 분석

- 게임 중간에 이탈하는 유저들이 많다는 **유저 데이터 확보**  
- 중간에 빠져나가거나 끝까지 클리어 하거나 하는 **유저 스타일 파악**  

<br/>

**위 정보들을 바탕으로 중간에 이탈하는 유저들을 막기 위해 계획 중인 사안**
1. 5Stage 전후 러닝 커브에 맞춘 밸런스 조절  
2. 유저들을 후킹할만한 요소 추가  



<br/>    
<br/>  


## 📋 개발 결과
> 유저테스트: 8월 8일 ~ 8월 11일 (4일간)
<img src="https://github.com/user-attachments/assets/7f4ea95b-5e65-4327-8573-4070b33caafa" alt="coding" width="600px" />

<br/>    
<br/>  
<br/>  


## ⚡ 프로젝트 개선

### 🚨 버그 픽스
<img src="https://github.com/user-attachments/assets/5544e9a3-8137-415a-9c45-bf96935b31f9" alt="coding" width="650px" />
<br/> 

### ✅ 개선 사항
<details>
<summary> Map이 가득 찼을 때 소환버튼 막기 </summary>
<div markdown="1">
<br/>   
    
- 소환할 수 있는 타일이 없음에도 불구하고 소환 버튼이 활성화되어 골드가 차감되는 문제 발생
     
<img src="https://github.com/user-attachments/assets/9b079eb0-2331-4deb-a2d5-2e4c3b87c07d" alt="coding" width="350px" />

<br/>   

- 소환 가능한 타일이 없다면 비활성화 되도록 수정
    
<img src="https://github.com/user-attachments/assets/438a7cfc-6003-473a-b357-d72a834615b7" alt="coding" width="350px" />
    
</div>
</details>

<details>
<summary> SkillUI 가시성 개선 </summary>
<div markdown="1">
<br/>   

💡 **기존 Skill UI**
- 스킬 전환이 되는지 되지 않는지 모호했다.
     
<img src="https://github.com/user-attachments/assets/678ee86d-dc74-442e-9af6-a76bb494bffd" alt="coding" width="250px" />

<br/>   

💡 **신규 Skill UI**
- 직관적으로 좌우 이동을 표시
    
<img src="https://github.com/user-attachments/assets/15389f98-059a-4494-948a-318801a104e3" alt="coding" width="250px" />
    
</div>
</details>

<details>
<summary> 배속 UI 개선 </summary>
<div markdown="1">
<br/>   
    
💡 **기존 배속  UI**
- 설정창 안에 존재하여 유저가 배속 UI의 존재를 알기 힘들었음
     
<br/>   

💡 **현재 배속  UI**
- 게임 화면 내에서 바로 접근할 수 있도록 가시성을 높였다.
    
<img src="https://github.com/user-attachments/assets/dcb341a4-bad0-4481-a341-aaf9efeec05a" alt="coding" width="250px" />
    
</div>
</details>

<details>
<summary> 보스 등장 Indicator </summary>
<div markdown="1">
<br/>   
    
💡 **기존 보스 스테이지**
- 보스 등장이나, 보스 스킬에 대한 Indicator가 존재하지 않아 유저가 보스스테이지를 인식하기 어려웠음.
     
<br/>   

💡 **보스 Indicator 추가**
- Indicator를 추가하여 유저 편의성을 높였다.
    
<img src="https://github.com/user-attachments/assets/5f58e245-fb0c-40e5-b7fe-81e5a71252ba" alt="coding" width="250px" />  
<img src="https://github.com/user-attachments/assets/fdfee0bc-1119-42d1-8bfd-061a9500c490" alt="coding" width="250px" />

</div>
</details>

<br/>    
<br/>  


## ❗ 트러블 슈팅

<details>
<summary> 👿 어드레서블 정렬 문제 </summary>
<div markdown="1">  

#### 🚨 문제 상황 
- 어드레서블에 등록된 리소스를 로딩할 때, 로드 순서가 맞지 않아 원하는 리소스가 로드되지 않는 문제 발생

#### 📌 문제 분석
- 어드레서블 그룹에 리소스를 추가할 때, 등록된 리소스들의 순서를 변경해주는 기능을 지원하지 않음.

#### ✅ 해결 방법
- IResourceLocation 리스트를 저장한 후, Enum에 정의된 이름 순서에 맞게 정렬시켜주는 방법으로 해결

<img src="https://github.com/user-attachments/assets/d2f80ff8-b765-46fb-820c-5b7c1879f71b" alt="coding" width="450px" />

</div>
</details>

<details>
<summary> 👿 유닛 썸네일 스프라이트 배칭 </summary>
<div markdown="1">

#### 🚨 문제 상황 
- 유닛 썸네일 스프라이트가 제대로 배치되지 않아, 미션 창을 열 때 배치가 약 40 정도 증가하는 문제가 발생  
     
<img src="https://github.com/user-attachments/assets/52dda7a2-fc6f-4bea-bca8-bd06cdc9273e" alt="coding" width="450px" />

#### 📌 문제 분석
- 유닛 썸네일 스프라이트가 각각 독립된 Texture2D로 사용되었기 때문에 배칭이 이루어지지 않음

✅ 해결 방법
- 썸네일들을 Sprite Atlas에 저장한 후, 어드레서블에서 로드하는 방식으로 배칭 문제를 해결  
- 추가적으로, Canvas 배치가 높다고 판단하여 UI에 사용되는 스프라이트들도 Sprite Atlas에 저장한 후 사용
    
<img src="https://github.com/user-attachments/assets/ee3259c6-833f-49a4-966b-246cb64e6508" alt="coding" width="450px" />
    
</div>
</details>

<details>
<summary> 👿 런타임 중 Static 배칭 적용안되는 문제 </summary>
<div markdown="1">

#### 🚨 문제 상황 
- 인스펙터 창에서 Static 배칭을 적용하려 했지만, 해당 프로젝트에서는 런타임에 어드레서블을 사용해 맵을 로드하기 때문에 Static 배칭이 적용되지 않음
     
<img src="https://github.com/user-attachments/assets/6e62728f-4f65-4f35-84e8-ebaa59ea76c2" alt="coding" width="250px" />

#### 📌 문제 분석
- 게임을 시작할 떄, 유니티가 자동으로 Static 배칭을 적용시켜주지만 런타임 중에는 적용 시켜주지 않음

✅ 해결 방법
- CombineMeshes를 이용하여 같은 Material을 가진 Mesh들을 묶어 렌더링하는 방식으로 문제 해결
- 추가적으로 Terrian은 런타임 중 변화할 필요가 없다고 판단하여 Mesh로 변환하여 Batch 최적화 진행
    
<img src="https://github.com/user-attachments/assets/0dcb1229-b92c-4d60-a2dc-598b59c57f9a" alt="coding" width="450px" />
    
</div>
</details>

<details>
<summary> 👿 개발 내역 Test 문제 </summary>
<div markdown="1">

#### 🚨 문제 상황 
- 개발이 진행됨에 따라 원하는 Unit, Enemy를 제작하고 Test하기 어려웠다.

#### 📌 문제 분석
> Unit
- 랜덤으로 소환되고 상위 유닛을 뽑아 테스트 하기 위해서는 원하는 유닛을 뽑아 조합할 때까지 게임을 진행해야하는 불편함이 있다.

> Boss
- 스테이지가 진행됨에 따라 특정 스테이지에서만 나오기 때문에, 마지막 스테이지의 보스를 점검하기 위해선 마지막 스테이지까지 게임을 플레이 하면서 테스트 해야만 했다.

✅ 해결 방법
> Unit 소환 Tool 제작
- 원하는 유닛을 버튼으로 소환할 수 있는 Tool을 제작
<img src="https://github.com/user-attachments/assets/c0f02863-1574-4c49-9f9c-103180cc61ed" alt="coding" width="450px" />

> Boss 소환 Inspector Tool 제작
- 원하는 세팅으로 보스를 소환할 수 있는 Tool을 제작
- boss 소환이 독립적이지 않고, Stage에 따라 변경되어야 하는 다양한 요소들이 묶여 있어 editorWindow가 아닌 editorInspector를 활용하여 GameStart 시에 어떤 boss를 소환할지 선택할 수 있도록 했다    
<img src="https://github.com/user-attachments/assets/71f96364-f030-424d-8be2-d90306f04f5b" alt="coding" width="450px" />
    
</div>
</details>

<details>
<summary> 👿 게임 배속이 주사율에 따라 달라지는 현상 </summary>
<div markdown="1">

#### 🚨 문제 상황 
- PC로 플레이할 때, 모니터의 주사율에 따라 게임 배속이 달라지는 현상이 발생했다.
<img src="https://github.com/user-attachments/assets/d8a2d1e9-3a40-4696-a421-ac2ee8c2d6f8" alt="coding" width="450px" />

#### 📌 문제 분석
- vSync `CPU 작업과 GPU 작업간의 동기화를 시켜주는 옵션`가 활성화되어 있어 발생한 문제였다.
  
✅ 해결 방법

    QualitySettings.vSyncCount = 0;
- 위의 코드와  vSync를 비활성화하는 코드를 작성하여 문제를 해결했다.
    
</div>
</details>

<details>
<summary> 👿 몬스터 Hp바 최적화 </summary>
<div markdown="1">

#### 🚨 문제 상황 
- 몬스터가 생성될 때마다 드로우 콜이 증가하는 현상이 발생했다.

#### 📌 문제 분석
- 생성되는 몬스터가 각각 자신의 World Space Canvas를가지고 있어 몬스터가 가진 캔버스의 개수에 따라 드로우 콜이 발생하는 것으로 확인되었다.
<img src="https://github.com/user-attachments/assets/dd1982ee-3ff0-4d5a-a214-b2fc0663865b" alt="coding" width="450px" />

<img src="https://github.com/user-attachments/assets/d837b9ef-e8db-46dd-8545-8eaadb913784" alt="coding" width="450px" />

✅ 해결 방법
- HP바 이미지를 오브젝트 풀링을 사용하여 미리 생성해두고 Enemy가 스폰될 때 각각의 Enemy에 새로운 Hp바를 할당해주었다.
- 그 결과, 모든 Hp바가 하나의 캔버스에 그려지고, 드로우 콜이 1개로 줄어들었다.
<img src="https://github.com/user-attachments/assets/8e9a7c83-0ff1-4f24-afb8-c7da0bc8ef5c" alt="coding" width="450px" />

</div>
</details>

<details>
<summary> 👿 사운드 슬라이드 바 값 설정 오류  </summary>
<div markdown="1">

#### 🚨 문제 상황 
- 슬라이드 바의 value가 0일 때, 의도한 대로 소리가 설정되지 않는 문제가 발생했다

#### 📌 문제 분석
- 오디오 믹서의 데시벨 범위는 -80dB에서 0dB로 설정되어 있다.
- 따라서 슬라이드 바의 value(0~1사이 값)를 데시벨 값으로 변환하기 위해 `Log10(volume) * 20` 공식을 사용한다.
- 하지만 슬라이드 바의 value가 0일 때, Log10(0)은 정의되지 않아 의도한 대로 값이 설정되지 않는 문제가 발생한 것이다.


✅ 해결 방법

    // volume은 0.0001~1 값이 들어오도록 방어코드 작성
    if (volume < 0.0001f)
        volume = 0.0001f;
- 슬라이드 바의 value를 0으로 설정하여 데시벨을 -80dB로 설정하려면 volume 값을 최소 0.0001f로 설정해야 하므, 위와 같은 방어코드를 작성하여 해결했다.

</div>
</details>

<br/>    
<br/>  
