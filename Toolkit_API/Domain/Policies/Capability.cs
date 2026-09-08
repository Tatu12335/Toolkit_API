using System.Text.Json.Serialization;

namespace Toolkit_API.Domain.Policies
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Capability
    {
        None,
        Keylogging,
        ScreenCapture,
        WebcamAccess,
        MicrophoneAccess,
        ClipboardAccess,
        CredentialAccess,
        ProcessInjection,
        ProcessEnumeration,
        MemoryReading,
        MemoryWriting,
        Persistance,
        PrivilegeEscalation,
        ServiceInstalation,
        RegisteryModification,
        ScheduleTask,
        NetworkCommunication,
        ReverseShell,
        Downloader,
        FileEncryption,
        FileDeletion,
        FileModification,
        AntiDebug,
        AntiVM,
        PackedExecutable,
        SelfReplicating,
        CommandExecution,
        PowerShellExecution,
        WMIEExecution,
        CommandLineExecution,
        DLLInjection,
        CodeInjection,
    }
}
