using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popupBox;
    public Animator animator;
    public TMP_Text popupText;

    public void PopUp(string text)
    {
        popupBox.SetActive(true);
        popupText.text = text;
        animator.SetTrigger("pop");
    }

}
