using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class HashUtil
{
    public enum ID { Leaderboard, Quest, Reward, Inbox }
    public enum Type { Current, Previous }

    private const string currentPrefix = "hash_current_";
    private const string previousPrefix = "hash_pre_";

    public static string GetHash(Type type, ID id)
    {
        return PlayerPrefs.GetString(GetKey(type, id), "");
    }

    public static void SetHash(Type type, ID id, string hash)
    {
        PlayerPrefs.SetString(GetKey(type, id), hash);
    }

    private static string GetKey(Type type, ID id)
    {
        string key = "";
        switch (type)
        {
            case Type.Current:
                key += currentPrefix;
                break;
            case Type.Previous:
                key += previousPrefix;
                break;
        }

        key += id.ToString().ToLower();
        key += GetPlayerIDHash();

        return key;
    }

    private static string GetPlayerIDHash()
    {
        return GetSHA1Hash(OnlineManager.Instance.playerDB.PlayerProfile.PlayerId);
    }

    public static bool IsHashChanged(ID id)
    {
        return GetHash(Type.Current, id) != GetHash(Type.Previous, id);
    }

    public static void SaveCurrentHashToPreviousHash(ID id)
    {
        SetHash(Type.Previous, id, GetHash(Type.Current, id));
    }

    public static string GetSHA1Hash(IEnumerable<object> list)
    {
        string concatenatedData = string.Concat(list);
        return GetSHA1Hash(concatenatedData);
    }
    public static string GetSHA1Hash(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
