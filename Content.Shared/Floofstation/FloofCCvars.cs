using Robust.Shared.Configuration;

namespace Content.Shared.FloofStation;

/// <summary>
/// Floofstation specific cvars.
/// </summary>
[CVarDefs]
// ReSharper disable once InconsistentNaming - Shush you
public sealed class FSCVars
{   
    /// <summary>
    /// How many characters the consent text can be.
    /// </summary>
    public static readonly CVarDef<int> ConsentFreetextMaxLength = CVarDef.Create("consent.freetext_max_length", 1000, CVar.REPLICATED | CVar.SERVER);
}
