using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MapChangeEffect : MonoBehaviour
{
    [SerializeField] private Image fadeImg;
    [SerializeField] private float fadeSpeed;

    private void Start()
    {
        GameManager.Instance.MapChangeEffect = this;
    }

    public async Task FadeIn()
    {
        float alpha = 1.0f;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImg.color = new Color(0, 0, 0, alpha);
            await Task.Yield();
        }
    }

    public async Task FadeOut()
    {
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImg.color = new Color(0, 0, 0, alpha);
            await Task.Yield();
        }
    }
}
