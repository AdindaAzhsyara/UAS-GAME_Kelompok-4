using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public CountdownTimer timer; // referensi ke CountdownTimer
    public Image star1;
    public Image star2;
    public Image star3;
    public Image[] winnerStars; // Isi dari winStar1, winStar2, winStar3

    public Sprite starOn;
    public Sprite starOff;
    private bool isGameOver = false;


    void Update()
    {
        if (timer == null || isGameOver) return;

        float timeLeft = timer.RemainingTime;

        if (timeLeft > 120f)
        {
            SetStars(true, true, true);
        }
        else if (timeLeft > 60f)
        {
            SetStars(true, true, false);
        }
        else if (timeLeft > 0f)
        {
            SetStars(true, false, false);
        }
        else
        {
            isGameOver = true;
        }
    }

    void SetStars(bool s1, bool s2, bool s3)
    {
        Sprite[] sprites = new Sprite[] {
        s1 ? starOn : starOff,
        s2 ? starOn : starOff,
        s3 ? starOn : starOff
    };

        // Bintang di HUD
        star1.sprite = sprites[0];
        star2.sprite = sprites[1];
        star3.sprite = sprites[2];

        // Bintang di Winner Panel
        if (winnerStars != null && winnerStars.Length == 3)
        {
            winnerStars[0].sprite = sprites[0];
            winnerStars[1].sprite = sprites[1];
            winnerStars[2].sprite = sprites[2];
        }
    }
}
