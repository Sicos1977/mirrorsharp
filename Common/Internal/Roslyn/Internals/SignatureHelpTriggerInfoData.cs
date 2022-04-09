namespace MirrorSharp.Internal.Roslyn.Internals {
    internal struct SignatureHelpTriggerInfoData {
        public SignatureHelpTriggerReason TriggerReason { get; }
        public char? TriggerCharacter { get; }

        public SignatureHelpTriggerInfoData(SignatureHelpTriggerReason triggerReason, char? triggerCharacter = null) {
            TriggerReason = triggerReason;
            TriggerCharacter = triggerCharacter;
        }
    }
}