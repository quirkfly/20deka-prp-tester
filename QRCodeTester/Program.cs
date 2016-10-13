using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using QRCoder;


namespace QRCodeTester
{
    class Program
    {
        static void Main(string[] args)
        {
            PurchaseReceipt purchaseReceipt = new PurchaseReceipt(1, 1);
            for (ushort i = 1; i < 101; i++)
            {
                purchaseReceipt.Add(new PurchaseItem(i.ToString(), i));
            }
            Bitmap qrCodeImage = purchaseReceipt.CreateQRCode();
            qrCodeImage.Save("c:\\tmp\\qrcode.png");            
        }
    }
}
