using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeTester
{
    class PurchaseResult
    {
        public Bitmap purchaseQRCode { get; set; }

        public PurchaseResult(Bitmap purchaseQRCode)
        {
            this.purchaseQRCode = purchaseQRCode;
        }
    }
}
