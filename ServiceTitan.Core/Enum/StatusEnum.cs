namespace ServiceTitan.Core
{
    public enum StatusEnum
    {
        IssueWhileJWTToken,
        JWTTokenGenerated,
        IssueWhileCallingCallsAPi,
        CallApiSuccess,
        NotHaveEnoughCap,
        HaveEnoughCap,
        IssueWithDownloadFile,
        DownloadFileSuccess,
        TranscribeSuccess,
        IssueWithTranscribe,
        OtherIssue,
        Success
    }
}