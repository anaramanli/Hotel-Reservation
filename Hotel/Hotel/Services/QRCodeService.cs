using QRCoder;
using System.Drawing;

namespace Hotel.Services
{
    //public class QRCodeService : IQRCodeService
    //{
    //    public Bitmap GenerateQRCode(string qrText)
    //    {
    //        var qrGenerator = new QRCodeGenerator();
    //        var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
    //        var qrCode = new QRCoder(qrCodeData);
    //        return qrCode.GetGraphic(20);
    //    }
    //}

    public interface IQRCodeService
    {
        Bitmap GenerateQRCode(string qrText);
    }
}
