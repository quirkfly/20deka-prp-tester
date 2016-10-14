using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeTester
{
    class InvalidPurchaseItemIdException : Exception
    {
        public InvalidPurchaseItemIdException(string message)
            : base(message)
        {
        }
    }

    class InvalidSellerIdException : Exception
    {
        public InvalidSellerIdException(string message)
            : base(message)
        {
        }
    }

    class InvalidSellerBranchIdException : Exception
    {
        public InvalidSellerBranchIdException(string message)
            : base(message)
        {
        }
    }

    class PurchaseReceiptUploadException : Exception
    {
        public PurchaseReceiptUploadException(string message)
            : base(message)
        {
        }
    }
}
