using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ItemDatabase itemDatabase;
    public ItemUIConfig itemUIConfg;
    public CountryConfig countryConfg;

    [SerializeField] private GameObject popupPrefab;
    private CommonPopup _commonPopup;
    public CommonPopup commonPopup
    {
        get
        {
            if (_commonPopup == null)
            {
                _commonPopup = Instantiate(popupPrefab).GetComponent<CommonPopup>();
            }

            return _commonPopup;
        }
    }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);
    }
}
