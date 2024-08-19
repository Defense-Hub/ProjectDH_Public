using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DogamSlot : UI_Base
{
    enum Buttons
    {
        Btn_DogamSlot
    }

    enum Texts
    {
        TXT_UnitName
    }

    enum Images
    {
        Image_Unit,
        UI_DogamSlot
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.Btn_DogamSlot).AddOnClickEvent(ShowInput);
    }

    public void SetDogamSlot(Transform transform, Sprite sprite, Color color, string name)
    {
        this.transform.SetParent(transform,false);
        GetImage((int)Images.Image_Unit).sprite = sprite;
        // 스프라이트의 비율을 유지하도록 설정
        GetImage((int)Images.Image_Unit).preserveAspect = true;
        GetImage((int)Images.UI_DogamSlot).color = color;
        GetText((int)Texts.TXT_UnitName).text = name;
    }

    private void ShowInput()
    {
        Debug.Log("정보 표시");
    }
}
