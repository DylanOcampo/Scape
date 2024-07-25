using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardMenu : MonoBehaviour
{
    public int Identifier;
    private Tween CardUp;

    public bool Click = false;

    public Transform Ani_Position;
    public bool Top, ForceOnExit;

    public Image WordReference;

    private void Setup_MouseEnter()
    {
        CardUp = transform.DOMoveY(Ani_Position.position.y, .5f, true);
        CardUp.Pause();
        CardUp.SetAutoKill(false);

    }

    public bool IsAnimationPlaying()
    {
        if (CardUp != null)
        {
            return CardUp.IsPlaying();
        }
        return false;
    }


    public void OnCallBack_Animation()
    {
        if (ForceOnExit)
        {
            ForceOnExit = false;
            ForceRestart();
        }


    }

    public void OnMouseExitCheck()
    {
        if (Top)
        {
            ForceOnExit = true;
        }
    }

    public void OnMouseEnter()
    {

        if (Top)
        {
            return;
        }

        if (Click)
        {
            return;
        }

        Top = true;
        if (CardUp == null)
        {
            Setup_MouseEnter();

            CardUp.Play();
        }
        else
        {
            CardUp.Restart();
        }


    }

    public void ForceRestart()
    {
        if (Top)
        {
            if (Ani_Position.position.y != transform.position.y)
            {
                Click = false;
                Top = false;
                CardUp.PlayBackwards();
            }
        }
    }

    public void OnMouseExit()
    {
        Click = false;

        if (Ani_Position.position.y != transform.position.y)
        {
            Top = false;
            CardUp.PlayBackwards();
        }


    }

    public void OnClickCard()
    {
        Click = true;
        MainMenuManager.instance.CardMenuSelect(Identifier);
    }


}
