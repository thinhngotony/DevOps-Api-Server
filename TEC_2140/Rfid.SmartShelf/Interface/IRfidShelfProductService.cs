using System.Collections.Generic;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Interface
{
    public interface IRfidShelfProductService
    {
        List<RfidShelfProduct> GetRfidShelfProducts();
        List<RfidShelfProduct> GetRfidShelfProductsByShelfNo(string shelfNo);
        List<RfidShelfProduct> GetRfidShelfProductsByDeviceName(string scannerName, string shelfName);
        int InsertShelftProductToDb(List<RfidShelfProduct> rfidShelfProducts);

        int InsertShelftProductToDb(List<RfidShelfProduct> rfidShelfProducts , bool deleteIfExisted);
        RfidShelfProduct[] ReadShelftProductCsv(string pathToFile);
        List<RfidShelfProduct> ReadShelftProductCsvUseCsvHelper(string pathToFile);

        List<RfidShelfProduct> GetUnImportRfidShelfProducts();

    }
}
