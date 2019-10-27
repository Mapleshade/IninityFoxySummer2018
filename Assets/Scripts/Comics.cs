using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Comics : MonoBehaviour
{
    private static bool AlreadyShowComics;

    [SerializeField] private SpriteAtlas atlas;

    public Image ComicsPage;

    public Text text;

    public AudioClip clip;

    public GameObject tutCanvas;
    
    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("TutorFinished") == 1)
        {
            AlreadyShowComics = true;
            gameObject.SetActive(false);
        }
        else
        {
            AlreadyShowComics = false;
            gameObject.SetActive(true);
        }

        if (!AlreadyShowComics)
            StartCoroutine(ShowComics());
    }

    private IEnumerator ShowComics()
    {
        ComicsPage.sprite = atlas.GetSprite("P1");
        text.text = "Это Котофея, школа магии и волшебства для котиков.";
        yield return new WaitForSeconds(5f);
        ComicsPage.sprite = atlas.GetSprite("P2");
        text.text = "А это наш герой: лисенок, приехавший учиться по обмену.";
        yield return new WaitForSeconds(5f);
        ComicsPage.sprite = atlas.GetSprite("P3");
        text.text = "Сегодня, готовясь к занятиям о магических артефактах, он отправился в хранилище, и...";
        yield return new WaitForSeconds(5f);
        ComicsPage.sprite = atlas.GetSprite("P4");
        text.text = "кажется, что-то пошло не так...";
        GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitForSeconds(5f);
        AlreadyShowComics = true;
        tutCanvas.SetActive(true);
        tutCanvas.GetComponent<TutorCanvas>().StartTutorial();
        gameObject.SetActive(false);
    }
}