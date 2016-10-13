using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeTester
{
    class PurchaseItem
    {
        public const int ITEM_ID_MAX_LENGTH = 16;
        public const int MAX_ITEM_AMOUNT = 999;

        public string itemId { get; set;  }
        public ushort amount { get; set; }

        public PurchaseItem(string itemId, ushort amount)
        {
            // ensure itemId passes sanity check            
            if (itemId == null)
            {
                throw new InvalidPurchaseItemIdException("item id can not be null");
            }
            else if (itemId.Length == 0)
            {
                throw new InvalidPurchaseItemIdException("item id can not be empty");
            }
            else if (itemId.Length > ITEM_ID_MAX_LENGTH)
            {
                throw new InvalidPurchaseItemIdException(String.Format("expected up to {0} characters, got {1}", ITEM_ID_MAX_LENGTH, itemId.Length));
            }

            // ensure amount passes sanity check too
            if (amount == 0 || amount > MAX_ITEM_AMOUNT)
            {
                throw new InvalidAmountException(String.Format("expected amount from interval 1 to {0}, got {1}", MAX_ITEM_AMOUNT, amount));
            }

            this.itemId = itemId.PadLeft(ITEM_ID_MAX_LENGTH, '0');
            this.amount = amount;
        }
    }
}
