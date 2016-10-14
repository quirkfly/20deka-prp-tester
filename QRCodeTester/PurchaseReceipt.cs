using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCodeTester
{
    class PurchaseReceipt
    {
        public uint sellerId { get; set; }
        public uint sellerBranchId { get; set; }
        public string customerCardId { get; set; }
        private Dictionary<string, ushort> purchaseItems;

        public PurchaseReceipt(uint sellerId, uint sellerBranchId)
        {
            this.sellerId = sellerId;
            this.sellerBranchId = sellerBranchId;
            this.purchaseItems = new Dictionary<string, ushort>();
        }

        public void Add(PurchaseItem purchaseItem)
        {
            if (this.purchaseItems.ContainsKey(purchaseItem.itemId))
            {
                this.purchaseItems[purchaseItem.itemId] += purchaseItem.amount;
            }
            else
            {
                this.purchaseItems.Add(purchaseItem.itemId, purchaseItem.amount);
            }
        }

        public string ToString()
        {
            string purchaseReceipt = String.Format("{0}{1}", this.sellerId.ToString().PadLeft(10, '0'), this.sellerBranchId.ToString().PadLeft(10, '0'));
            purchaseReceipt += String.Format("{0}{1}", this.sellerId.ToString().PadLeft(10, '0'), this.sellerBranchId.ToString().PadLeft(10, '0'));
            foreach (KeyValuePair<string, ushort> purchaseItem in this.purchaseItems)
            {
                purchaseReceipt += String.Format("{0}{1}", purchaseItem.Key, purchaseItem.Value.ToString().PadLeft(3, '0'));
            }

            return purchaseReceipt;
        }
    }
}