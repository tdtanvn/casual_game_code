using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : MonoBehaviour
{
    public virtual void Home()
    {
        MenuManager.LoadScene(BuiltInScenes.MainMenu);
    }

    public virtual void Back()
    {

    }
}
