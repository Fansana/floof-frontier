using Robust.Shared.Configuration;

namespace Content.Shared._FS.CCVar;

[CVarDefs]
public sealed class FSCCVars
{
    /// <summary>
    ///     Enable Discord linking, show linking button and modal window
    /// </summary>
    public static readonly CVarDef<bool> DiscordAuthEnabled =
        CVarDef.Create("discord.auth_enabled", false, CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord auth server API
    /// </summary>
    public static readonly CVarDef<string> DiscordAuthApiUrl =
        CVarDef.Create("discord.auth_api_url", "", CVar.SERVERONLY);

    /// <summary>
    ///     Secret key of the Discord auth server API
    /// </summary>
    public static readonly CVarDef<string> DiscordAuthApiKey =
        CVarDef.Create("discord.auth_api_key", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);
}
