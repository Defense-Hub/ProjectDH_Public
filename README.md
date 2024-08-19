<div align="center">
<img src="https://github.com/user-attachments/assets/dbcfeed1-418b-4926-81db-ecbcd2a21faf" alt="coding" width="550px" />  
<br/>    
<br/>    
    
### Help Me Guardians Project
<br/>    
<br/> 

</div>

## 🙋‍♂️ 팀 소개

 `Stack`  **Unity, C#**   

 `Develop`  **2024.06 ~ 2024.08**   

 `Made by`  **ProjectDH** 박현호, 윤세나, 정은지, 지승도
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

`itch.io` [itch.io](https://defensehub-a.itch.io/help-me-guadians)  

`팀 SA 페이지` [팀 SA 페이지](https://www.notion.so/DefenseHub-04dec92b233a448097eb3f7fe2709e0e?pvs=24)  

`Jira` [Jira](https://profilehyunho.notion.site/Jira-1331eab7a4d5417ca29c757cb2e79aaf)  

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

`장르` 3D 디펜스 게임  
`레퍼런스`  운빨존많겜  
`차별점` 속성을 가진 3D 유닛들과 다양한 공격 패턴을 가진 보스  

다양한 유닛을 소환하고 조합하며
각기 다른 특성을 가진 보스들을 물리치는 게임입니다 :)


<br/>    
<br/>  

## 💝 게임 소개
<img src="https://github.com/user-attachments/assets/9ba990dd-5c8f-4e36-92b2-da68896412df" alt="coding" width="550px" />

#### 📽️ 게임 영상
 ⬇ `Youtube Link `⬇ 


<br/>    
<br/>  

## 주요 사용 기술
<img src="https://github.com/user-attachments/assets/86726e67-8475-4521-8911-c29f66484b4f" alt="coding" width="550px" />

<br/>    
<br/>  


## 개발 진행


<br/>    
<br/>  

## 📋 개발 결과
> 유저테스트: 8월 8일 ~ 8월 11일 (4일간)
<img src="https://github.com/user-attachments/assets/7f4ea95b-5e65-4327-8573-4070b33caafa" alt="coding" width="550px" />

<br/>    
<br/>  


## ⚡ 프로젝트 개선

### 💡 버그

**🚨 버그 픽스**   
<br/> 
<img src="https://github.com/user-attachments/assets/5544e9a3-8137-415a-9c45-bf96935b31f9" alt="coding" width="550px" />
<br/> 

**✅ 개선 사항**
<details>
<summary> 1. Map이 가득 찼을 때 소환버튼 막기 </summary>
<div markdown="1">
<br/>   
    
소환할 수 있는 타일이 없음에도 불구하고 소환 버튼이 활성화되어 골드가 차감되는 문제 발생
     
<img src="https://github.com/user-attachments/assets/9b079eb0-2331-4deb-a2d5-2e4c3b87c07d" alt="coding" width="450px" />

<br/>   

소환 가능한 타일이 없다면 비활성화 되도록 수정
    
<img src="https://github.com/user-attachments/assets/438a7cfc-6003-473a-b357-d72a834615b7" alt="coding" width="450px" />
    
</div>
</details>

<details>
<summary> 2. SkillUI 가시성 개선 </summary>
<div markdown="1">
<br/>   

> 기존 Skill UI
스킬 전환이 되는지 되지 않는지 모호했다.
     
<img src="https://github.com/user-attachments/assets/678ee86d-dc74-442e-9af6-a76bb494bffd" alt="coding" width="250px" />

<br/>   

> 신규 Skill UI
직관적으로 좌우 이동을 표시
    
<img src="https://github.com/user-attachments/assets/15389f98-059a-4494-948a-318801a104e3" alt="coding" width="250px" />
    
</div>
</details>

<details>
<summary> 3. 배속 UI 개선 </summary>
<div markdown="1">
<br/>   
    
> 기존 배속  UI
설정창 안에 존재하여 유저가 배속 UI의 존재를 알기 힘들었음
     
<br/>   

> 현재 배속  UI
게임 화면 내에서 바로 접근할 수 있도록 가시성을 높였다.
    
<img src="https://github.com/user-attachments/assets/dcb341a4-bad0-4481-a341-aaf9efeec05a" alt="coding" width="250px" />
    
</div>
</details>

<details>
<summary> 4. 보스 등장 Indicator </summary>
<div markdown="1">
<br/>   
    
> 기존 보스 스테이지
보스 등장이나, 보스 스킬에 대한 Indicator가 존재하지 않아 유저가 보스스테이지를 인식하기 어려웠음.
     
<br/>   

> 보스 Indicator 추가
Indicator를 추가하여 유저 편의성을 높였다.
    
<img src="https://github.com/user-attachments/assets/5f58e245-fb0c-40e5-b7fe-81e5a71252ba" alt="coding" width="250px" />
<img src="https://github.com/user-attachments/assets/fdfee0bc-1119-42d1-8bfd-061a9500c490" alt="coding" width="250px" />

</div>
</details>

<br/>    
<br/>  


## 트러블 슈팅

<details>
<summary> 1. 유닛 썸네일 스프라이트 배칭 </summary>
<div markdown="1">
<br/>   
    
유닛 썸네일 스프라이트가 제대로 배치되지 않아, 미션 창을 열 때 배치가 약 40 정도 증가하는 문제가 발생  
유닛 썸네일 스프라이트가 각각 독립된 Texture2D로 사용되었기 때문에 배칭이 이루어지지 않음
     
<img src="https://github.com/user-attachments/assets/52dda7a2-fc6f-4bea-bca8-bd06cdc9273e" alt="coding" width="450px" />

---

썸네일들을 Sprite Atlas에 저장한 후, 어드레서블에서 로드하는 방식으로 배칭 문제를 해결  
추가적으로, Canvas 배치가 높다고 판단하여 UI에 사용되는 스프라이트들도 Sprite Atlas에 저장한 후 사용
    
<img src="https://github.com/user-attachments/assets/ee3259c6-833f-49a4-966b-246cb64e6508" alt="coding" width="450px" />
    
</div>
</details>

<br/>    
<br/>  
