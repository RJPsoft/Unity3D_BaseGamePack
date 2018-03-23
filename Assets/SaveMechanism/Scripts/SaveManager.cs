using Newtonsoft.Json;
using RJPSoft.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Manages the load and save to the PlayerPrefs
/// </summary>
public static class SaveManager
{
    #region Public Methods

    const string KEY = "";
    const string IV = "";

    /// <summary>Clears all saved values.</summary>
    public static void ClearAllSavedValues()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>Gets a saved value by key.</summary>
    /// <typeparam name="T">The Type of the saved value</typeparam>
    /// <param name="key">The saved value key.</param>
    /// <param name="defaultValue">The default value to return.</param>
    /// <returns>The Saved Value</returns>
    public static T GetValue<T>(PlayerPrefKey key, T defaultValue = default(T))
    {
        var stringData = PlayerPrefs.GetString(key.PrefKey);
        if (!string.IsNullOrEmpty(stringData))
        {
            if (key.UseCryptography)
            {
                stringData = CryptoHandler.DecryptAES(stringData, KEY, IV);
            }

            T savedValue = JsonConvert.DeserializeObject<T>(stringData);
            if (savedValue != null)
            {
                defaultValue = savedValue;
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// Determines whether the specified key exists.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if the specified key exists; otherwise, <c>false</c>.</returns>
    public static bool HasSavedKey(PlayerPrefKey key)
    {
        return PlayerPrefs.HasKey(key.PrefKey);
    }

    /// <summary>Saves the value.</summary>
    /// <typeparam name="T">Type of <paramref name="value"/></typeparam>
    /// <param name="key">The key to save the value with.</param>
    /// <param name="value">The value to be saved.</param>
    public static void SaveValue<T>(PlayerPrefKey key, T value)
    {
        var valueToBeSaved = JsonConvert.SerializeObject(value);
        if (key.UseCryptography)
        {
            valueToBeSaved = CryptoHandler.EncryptAES(valueToBeSaved, KEY, IV);
        }

        PlayerPrefs.SetString(key.PrefKey, valueToBeSaved);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Removes the saved value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void RemoveSavedValue(PlayerPrefKey key)
    {
        PlayerPrefs.DeleteKey(key.PrefKey);
    }

    static void GenerateKeyIvPair()
    {
        var keyIvPair = CryptoHandler.CreateAESKeyIVPair();
        Debug.Log(string.Format("key: {0} - IV: {1}", keyIvPair.Item1, keyIvPair.Item2));
    }

    #endregion Public Methods
}