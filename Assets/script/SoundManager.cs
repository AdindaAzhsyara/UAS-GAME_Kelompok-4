using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sumberSuara;

    private static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // agar tidak dihancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject); // jika sudah ada instance, hancurkan yang baru
        }
    }

    public void KetikaSliderDiubah(float nilaiSlider)
    {
        sumberSuara.volume = nilaiSlider;
    }
}
