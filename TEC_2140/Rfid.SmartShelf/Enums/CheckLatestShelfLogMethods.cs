namespace Vjp.Rfid.SmartShelf.Enums
{
    public enum CheckLatestShelfLogMethods
    {
        /// <summary>
        /// load all master data when init and check latest of shelf log in loaded data
        /// </summary>
        CheckInLocal =0 ,
        /// <summary>
        /// check direct in db 
        /// </summary>
        CheckInDatabase = 1
    }
}
