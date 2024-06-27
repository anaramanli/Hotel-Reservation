using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Hotel.Services
{
    public class QRCodeService : IQRCodeService
    {
        public Bitmap GenerateQRCode(string qrText)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public byte[] GenerateQRCodeAsByteArray(string qrText)
        {
            using (var bitmap = GenerateQRCode(qrText))
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }

    public interface IQRCodeService
    {
        Bitmap GenerateQRCode(string qrText);
        byte[] GenerateQRCodeAsByteArray(string qrText);
    }
}
