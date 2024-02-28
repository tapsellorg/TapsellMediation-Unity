using System;

namespace Tapsell.Mediation.Show
{
    [Serializable]
    public class ShowCompletionMessage
    {
        public string adId;
        public string completionState;

        public ShowCompletionMessage(string adId, string completionState)
        {
            this.adId = adId;
            this.completionState = completionState;
        }
    }

    public enum ShowCompletionState
    {
        Completed,
        Skipped,
        Unknown
    }

    public static class ShowCompletionStateHelper
    {
        public static ShowCompletionState FromString(string state)
        {
            return state switch
            {
                "COMPLETED" => ShowCompletionState.Completed,
                "SKIPPED" => ShowCompletionState.Skipped,
                _ => ShowCompletionState.Unknown
            };
        }
    }
}