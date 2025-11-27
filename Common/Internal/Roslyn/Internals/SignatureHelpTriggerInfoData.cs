namespace MirrorSharp.Internal.Roslyn.Internals;

internal struct SignatureHelpTriggerInfoData(SignatureHelpTriggerReason triggerReason, char? triggerCharacter = null) {
    public SignatureHelpTriggerReason TriggerReason { get; } = triggerReason;
    public char? TriggerCharacter { get; } = triggerCharacter;
}