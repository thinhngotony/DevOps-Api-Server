namespace Vjp.Rfid.SmartShelf.Constants
{
    public static class AppConstants
    {
        public static string PosPassword = "00000000";
        public static string PosFilterId = "000000000000000000000000";
        public static string PosFilterMask = "000000000000000000000000";
        public static int PosStartReadTimeout = 1000;
        public static int PosReadTimerInterval = 1000;
        public static int ReadEmptyTagWaitTimeout = 2000;
        public static int ReadHasTagsWaitTimeout = 2000;
        public static int PosDeviceCnt = 0;
        //public static int LvMaxViewItems = 20;
        //public static string AppFontName = "Arial";
        //public static int AppMediumFontSize = 9;

        public static long MAX_ROW_DISPLAY =2048;

        public const string ActionRegisterShelfStart = "ACTION_REGISTER_SHELF_START";
        public const string ActionRegisterShelfEnd = "ACTION_REGISTER_SHELF_END";
        public const string ActionReloadShelfEnd = "ACTION_RELOAD_SHELF_END";
    }
}
