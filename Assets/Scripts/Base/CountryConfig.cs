using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Country Config", menuName = "Country Config")]
public class CountryConfig : ScriptableObject
{
    [Serializable]
    public class Country
    {
        public string countryCode;
        public Sprite flag;
    }

    [SerializeField] private List<Country> countries;

    public Sprite GetCountryFlag(string code) => countries.FirstOrDefault(country => country.countryCode == code).flag; 
}
