using UnityEngine;

public class PlayerInfo
{
    private const string NicknameKey = "PlayerNickname";

    public static void SetNickName(string nickName)
    {
        PlayerPrefs.SetString(NicknameKey, nickName);
        PlayerPrefs.Save();
    }

    public static string GetNickName()
    {
        if (!PlayerPrefs.HasKey(NicknameKey))
        {
            var randomName = "Player_" + Random.Range(100, 999);
            SetNickName(randomName);
        }
        
        return PlayerPrefs.GetString(NicknameKey);
    }
}
