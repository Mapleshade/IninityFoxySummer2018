using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TutorCanvas : MonoBehaviour
{
    public GameObject PicturesDialogsWindows;
    public GameObject FoxIcons;
    public GameObject CatIcons;
    private Image catImage;
    public Image poster;
    public SpriteAtlas atlas;
    public Text FoxText;
    public Text CatText;
    private Replica _replica;
    private string _endOfPhrase;
    public GameObject tutorButton;

//    void Start()
//    {
//
//        catImage = CatIcons.GetComponentInChildren<Image>();
//        _endOfPhrase = "\n(Кликните в любом месте, чтобы продолжить)";
//        //если обучение ещё не пройдено, то начинаем обучение
//        if (PlayerPrefs.GetInt("TutorFinished") == 0)
//        {
//            DialogsOnTutor.GenerateDialogs();
//            NextReplica();
//            Time.timeScale = 0;
//        }
//        else
//        {
//            PicturesDialogsWindows.SetActive(false);
//        }
//    }

    public void StartTutorial()
    {
        catImage = CatIcons.GetComponentInChildren<Image>();
        _endOfPhrase = "\n(Кликните в любом месте, чтобы продолжить)";
        //если обучение ещё не пройдено, то начинаем обучение
        if (PlayerPrefs.GetInt("TutorFinished") == 0)
        {
            DialogsOnTutor.GenerateDialogs();
            NextReplica();
            Time.timeScale = 0;
        }
        else
        {
            PicturesDialogsWindows.SetActive(false);
        }
    }

    public void InvisibleButtonForDealogs()
    {
        NextReplica();
        Time.timeScale = 0;

        if (_replica.Id == 1)
        {
            catImage.sprite = atlas.GetSprite("AvaCat2");
            poster.sprite = atlas.GetSprite("Poster2");
        }
        
        if (_replica.Id == 2)
        {
            catImage.sprite = atlas.GetSprite("AvaCat3");
            poster.sprite = atlas.GetSprite("Poster3");
        }
        
        if (_replica.Id == 7 || _replica.Id == 14 || _replica.Id == 16)
        {
            Time.timeScale = 1;
            PicturesDialogsWindows.SetActive(false);
        }

        if (_replica.Id == 7)
        {
            tutorButton.GetComponentInChildren<Text>().text = "Используйте клавиши WASD для передвижения" + _endOfPhrase;
            tutorButton.SetActive(true);
        }

        if (_replica.Id == 14)
            TutorLevel.BookReaded = true;
        
        if(_replica.Id == 16)
            GameInformation.SaveGame(0, 0, 0, 100f, 100f, 2, "RainOfFire", "FireArrow", 0, 0, 0, PlayerPrefs.GetInt("IDOfLastSpell"), 1, 0);
    }

    private void NextReplica()
    {
        _replica = DialogsOnTutor.GetNextReplica();
        if (_replica.WhoSaid == "CAT")
        {
            CatText.text = _replica.WhatSay + _endOfPhrase;
            CatIcons.SetActive(true);
            FoxIcons.SetActive(false);
        }

        else
        {
            FoxText.text = _replica.WhatSay + _endOfPhrase;
            CatIcons.SetActive(false);
            FoxIcons.SetActive(true);
        }

        PicturesDialogsWindows.SetActive(true);
    }

    public void InvisibleButtonForTutButton()
    {
        tutorButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowTutForJump()
    {
        tutorButton.GetComponentInChildren<Text>().text = "Нажмите SPACE для прыжка" + _endOfPhrase;
        tutorButton.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ShowTutForAttack()
    {
        tutorButton.GetComponentInChildren<Text>().text = "Нажмите E или F для атаки" + _endOfPhrase;
        tutorButton.SetActive(true);
        Time.timeScale = 0;
    }
    
    
}