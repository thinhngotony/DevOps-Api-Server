using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Interface
{
    public interface IRfidShelfHttpService
    {
        //bool ItemInOutShelfChangeNotify();
        Task ItemInOutShelfChangeNotify();

        Task<RfidToJanResponse> GetJanByRfid(string rfid);

        Task<JanToItemInfoResponse> GetItemInfoByJanCode(string janCode);

    }
}
