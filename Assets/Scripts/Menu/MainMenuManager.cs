using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager _instance;

    public static MainMenuManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainMenuManager>();
            }
            return _instance;
        }
    }



    public Transform Ani_Menu_Start, Ani_Menu_End;
    public GameObject Card, MenuCards, SettingsMenu, TutorialMenu, PopUp;

    public List<CardMenu> CardButtons = new List<CardMenu>();
    public List<Selectable> CardInteraction = new List<Selectable>();
    public List<GameObject> CardChecker = new List<GameObject>();

    public List<Sprite> Words = new List<Sprite>();
    public Tween Menu_Ani;

    private bool CallbackInterfaceBool;
    private int CallbackInterfaceInt;

    public int MenuSection = 0;

    private void Setup_Card_Menu()
    {
        Menu_Ani = MenuCards.transform.DOMove(Ani_Menu_End.position, .5f);
        Menu_Ani.Pause();
        Menu_Ani.SetAutoKill(false);
        Menu_Ani.OnComplete(OnCallBack_Menu_Ani);
    }

    private void OnCallBack_Menu_Ani()
    {
        if (CallbackInterfaceBool)
        {
            CallbackInterfaceBool = false;
            CardMenu(CallbackInterfaceInt);
        }

    }

    private void OnStart_CardMenu()
    {
        CardCheckerTrigger(true);
        foreach (var item in CardInteraction)
        {
            item.gameObject.SetActive(false);
        }

    }

    public void ForceCardsAnimationReset()
    {
        foreach (var item in CardButtons)
        {
            item.ForceRestart();
        }



    }

    private void Callback_CardMenu()
    {
        CardCheckerTrigger(false);
        foreach (var item in CardInteraction)
        {
            item.gameObject.SetActive(true);
        }

        foreach (var item in CardButtons)
        {
            item.OnCallBack_Animation();
        }


    }

    public void Play_Card_Menu()
    {
        OnStart_CardMenu();
        if (Menu_Ani == null)
        {
            Setup_Card_Menu();

            Menu_Ani.Play();
        }
        else
        {
            Menu_Ani.Restart();
        }

    }

    public bool MainMenuAnimationPlaying()
    {
        if (Menu_Ani != null)
        {

            return DOTween.IsTweening(gameObject);
        }
        return false;

    }

    private void OpenPlay()
    {
        CardButtons[3].gameObject.SetActive(true);
        CardInteraction[3].gameObject.SetActive(true);
        CardButtons[0].WordReference.sprite = Words[3];
        CardButtons[1].WordReference.sprite = Words[4];
        CardButtons[2].WordReference.sprite = Words[5];
        CardButtons[3].WordReference.sprite = Words[10];
        CardButtons[0].WordReference.gameObject.transform.localEulerAngles = Vector3.zero;
        CardButtons[2].WordReference.gameObject.transform.localEulerAngles = Vector3.zero;
    }

    private void OpenSinglePlayer()
    {
        CardButtons[0].WordReference.sprite = Words[6];
        CardButtons[1].WordReference.sprite = Words[7];

        CardButtons[2].WordReference.sprite = Words[8];
        CardButtons[3].gameObject.SetActive(true);
        CardInteraction[3].gameObject.SetActive(true);
        CardButtons[3].WordReference.sprite = Words[9];
    }

    private void OpenTutorial()
    {
        AudioManager.instance.PlayClip(9);
        TutorialMenu.SetActive(true);
    }

    private void OpenPopUp()
    {
        AudioManager.instance.PlayClip(9);
        PopUp.SetActive(true);
    }

    private void BackSinglePlayer()
    {
        CardButtons[3].gameObject.SetActive(false);
        CardInteraction[3].gameObject.SetActive(false);
        OpenPlay();
    }

    private void OpenMenuInitial()
    {
        CardButtons[3].gameObject.SetActive(false);
        CardInteraction[3].gameObject.SetActive(false);
        CardButtons[0].WordReference.sprite = Words[0];
        CardButtons[1].WordReference.sprite = Words[1];
        CardButtons[2].WordReference.sprite = Words[2];
        CardButtons[0].WordReference.gameObject.transform.localEulerAngles = new Vector3(0, 0, -20);
        CardButtons[2].WordReference.gameObject.transform.localEulerAngles = new Vector3(0, 0, 20);
    }

    private void OpenSettings()
    {
        SettingsMenu.SetActive(true);
    }

    IEnumerator FixAni()
    {
        foreach (var item in CardButtons)
        {
            if (item.IsAnimationPlaying())
            {
                yield return new WaitForSeconds(.5f);
                break;
            }

        }
        Play_Card_Menu();
    }

    public void CardMenuReset(bool value)
    {
        OpenMenuInitial();
        MenuSection = 0;
        UIManager.instance.FirstMenu.SetActive(value);
        ForceCardsAnimationReset();
    }

    private void CardMenu(int option)
    {
        AudioManager.instance.PlayClip(12);
        switch (MenuSection)
        {
            case 0:
                switch (option)
                {
                    case 0:
                        OpenSettings();
                        break;
                    case 1:
                        OpenPlay();
                        MenuSection = 1;
                        break;
                    case 2:
                        OpenPopUp();
                        //Collections
                        break;
                }
                break;
            case 1:
                switch (option)
                {
                    case 0:
                        //Tutorial
                        OpenTutorial();
                        break;
                    case 1:
                        OpenSinglePlayer();
                        MenuSection = 2;
                        break;
                    case 2:
                        OpenPopUp();
                        //Multiplayer
                        break;
                    case 3:
                        OpenMenuInitial();
                        MenuSection = 0;
                        break;

                }
                break;
            case 2:
                switch (option)
                {
                    case 0:
                        UIManager.instance.OnClick_PlayButton(1);
                        break;
                    case 1:
                        UIManager.instance.OnClick_PlayButton(2);
                        break;
                    case 2:
                        UIManager.instance.OnClick_PlayButton(3);
                        break;
                    case 3:
                        BackSinglePlayer();
                        MenuSection = 1;
                        break;

                }
                break;


        }

        MenuCards.transform.DOMove(Ani_Menu_Start.position, .25f).OnComplete(() => Callback_CardMenu());
    }


    private void CardCheckerTrigger(bool value)
    {
        foreach (GameObject item in CardChecker)
        {
            item.SetActive(value);
        }
    }

    public void CardMenuSelect(int option)
    {
        CallbackInterfaceInt = option;
        CallbackInterfaceBool = true;
        StartCoroutine(FixAni());

    }



}
