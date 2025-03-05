using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<float> SiliconNpcUpdateTime =
            CVarDef.Create("silicon.npcupdatetime", 1.5f, CVar.SERVERONLY);
}
