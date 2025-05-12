namespace Tapsell.Mediation
{
    public static class TapsellConstants
    {
        // Used to check format of ZONE_ID and TAPSELL_APP_ID
        public const string REGEX_STR_TAPSELL_ID = "^[a-fA-F0-9]{24}$";
        // Used to check format of ZONE_ID and TAPSELL_APP_ID in old apps
        public const string REGEX_STR_UUID = "^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$";
    }
}