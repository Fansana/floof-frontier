using Content.Shared.InteractionVerbs;
using Content.Shared.Mood;
using Robust.Shared.Prototypes;

namespace Content.Server.InteractionVerbs.Actions;

// Floofstation notice:
// This file comes from EE. Flooftier does not currently have the mood system, and likely never will.
// These are only here for compatibility.
/// <summary>
///     An action that adds a moodlet to the target, or removes one.
/// </summary>
[Serializable]
public sealed partial class MoodAction : InteractionAction
{
    [DataField(required: true)]
    // public ProtoId<MoodEffectPrototype> Effect;
    public string Effect; // Floof - since we don't have the mood system, this serves as a placeholder

    /// <summary>
    ///     Parameters for the <see cref="MoodEffectEvent"/>. Only used if <see cref="Remove"/> is false.
    /// </summary>
    [DataField]
    public float Modifier = 1f, Offset = 0f;

    /// <summary>
    ///     If true, the moodlet will be removed. Otherwise, it will be added.
    /// </summary>
    [DataField]
    public bool Remove = false;

    public override bool CanPerform(InteractionArgs args, InteractionVerbPrototype proto, bool isBefore, VerbDependencies deps)
    {
        return true;
    }

    public override bool Perform(InteractionArgs args, InteractionVerbPrototype proto, VerbDependencies deps)
    {
        // Floofstation - replaced with a placeholder
        // if (Remove)
        //     deps.EntMan.EventBus.RaiseLocalEvent(args.Target, new MoodRemoveEffectEvent(Effect));
        // else
        //     deps.EntMan.EventBus.RaiseLocalEvent(args.Target, new MoodEffectEvent(Effect, Modifier, Offset));

        return true; // Mood system is shitcode so we can't even know if the effect was added or anything
    }
}
