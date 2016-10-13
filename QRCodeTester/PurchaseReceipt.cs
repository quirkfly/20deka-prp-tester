using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using QRCoder;
using RestSharp;
using System.IO;

namespace QRCodeTester
{
    class PurchaseReceipt
    {
        public const string BASE_URL = "https://20deka.com";

        public const int SELLER_DOES_NOT_EXIST = 53;
        public const int SELLER_BRANCH_DOES_NOT_EXIST = 77;        

        public uint sellerId { get; set; }
        public uint sellerBranchId { get; set; }
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

        public Bitmap CreateQRCode()
        {
            string purchaseReceipt = String.Format("{0}{1}", this.sellerId.ToString().PadLeft(10, '0'), this.sellerBranchId.ToString().PadLeft(10, '0'));
            purchaseReceipt += String.Format("{0}{1}", this.sellerId.ToString().PadLeft(10, '0'), this.sellerBranchId.ToString().PadLeft(10, '0'));
            foreach (KeyValuePair<string, ushort> purchaseItem in this.purchaseItems)
            {
                purchaseReceipt += String.Format("{0}{1}", purchaseItem.Key, purchaseItem.Value.ToString().PadLeft(3, '0'));
            }

            var restClient = new RestClient(BASE_URL);
            var request = new RestRequest("/purchase-receipt/upload", Method.POST);
            request.AddParameter("payload", purchaseReceipt);
            IRestResponse response = restClient.Execute(request);

            MemoryStream responseStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PurchaseReceiptUploadResponse));
            var purchaseReceiptUploadResponse = jsonSerializer.ReadObject(responseStream) as PurchaseReceiptUploadResponse;
            responseStream.Close();

            if (purchaseReceiptUploadResponse.status == 0)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(String.Format("{0}/purchase-receipt/process/{1}", BASE_URL, purchaseReceiptUploadResponse.receiptId), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                //return qrCode.GetGraphic(20);
                return qrCode.GetGraphic(20, Color.Black, Color.White, new Bitmap(Image.FromFile("C:\\tmp\\ic_launcher.png")));
            }
            else if (purchaseReceiptUploadResponse.status == SELLER_DOES_NOT_EXIST)
            {
                throw new InvalidSellerIdException(String.Format("Seller id {0} does not exist.", this.sellerId));
            }
            else if (purchaseReceiptUploadResponse.status == SELLER_BRANCH_DOES_NOT_EXIST)
            {
                throw new InvalidSellerBranchIdException(String.Format("Seller branch id {0} does not exist.", this.sellerId));
            }
            else
            {
                throw new PurchaseReceiptUploadException(String.Format("Failed to upload purchase receipt, status: {0}", purchaseReceiptUploadResponse.status));
            }
        }
    }

    [DataContract]
    public class PurchaseReceiptUploadResponse
    {
        [DataMember(Name = "status")]
        public int status { get; set; }

        [DataMember(Name = "msg")]
        public string msg { get; set; }

        [DataMember(Name = "receipt_id")]
        public string receiptId { get; set; }
    }
}