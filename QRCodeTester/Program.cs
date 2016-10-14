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
            purchaseReceipt.customerCardId = "63400950019900345";

            for (ushort i = 1; i < 150; i++)
            {
                purchaseReceipt.Add(new PurchaseItem(i.ToString(), i));
            }

            PurchaseReceiptProcessor purchaseReceiptProcessor = new PurchaseReceiptProcessor();
            PurchaseResult purchaseResult = purchaseReceiptProcessor.Process(purchaseReceipt);

            if (purchaseResult.purchaseQRCode != null)
            {
                purchaseResult.purchaseQRCode.Save("c:\\tmp\\qrcode.png");
            }
        }
    }
}
