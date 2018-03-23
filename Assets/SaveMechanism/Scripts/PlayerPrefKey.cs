/// <summary>
/// Key used to save and restore data to the PlayerPref
/// </summary>
public class PlayerPrefKey
{
    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="PlayerPrefKey"/> class.</summary>
    /// <param name="playerPrefKey">The player preference key.</param>
    /// <param name="useCryptography">Should use cryptografy?</param>
    public PlayerPrefKey(string playerPrefKey, bool useCryptography = false)
    {
        PrefKey = playerPrefKey;
        UseCryptography = useCryptography;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets or sets the preference key.</summary>
    /// <value>The preference key.</value>
    public string PrefKey { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [use cryptography].
    /// </summary>
    /// <value><c>true</c> if [use cryptography]; otherwise, <c>false</c>.</value>
    public bool UseCryptography { get; set; }

    #endregion Public Properties
}