using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Drawing;

using RestSharp;
using QRCoder;


namespace QRCodeTester
{
    class PurchaseReceiptProcessor
    {
        public const string BASE_URL = "https://20deka.com";

        public const int SELLER_DOES_NOT_EXIST = 53;
        public const int SELLER_BRANCH_DOES_NOT_EXIST = 77;

        public PurchaseResult Process(PurchaseReceipt purchaseReceipt)
        {
            var restClient = new RestClient(BASE_URL);

            var request = new RestRequest("/purchase-receipt/upload", Method.POST);
            request.AddParameter("customer_card_id", purchaseReceipt.customerCardId);
            request.AddParameter("payload", purchaseReceipt.ToString());

            IRestResponse response = restClient.Execute(request);
            MemoryStream responseStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PurchaseReceiptUploadResponse));
            var purchaseReceiptUploadResponse = jsonSerializer.ReadObject(responseStream) as PurchaseReceiptUploadResponse;
            responseStream.Close();

            if (purchaseReceiptUploadResponse.status == 0)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(String.Format("{0}/purchase-receipt/process/{1}", BASE_URL, purchaseReceiptUploadResponse.path), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                return new PurchaseResult(purchaseReceiptUploadResponse.isLinkedWithHousehold ?
                    qrCode.GetGraphic(20, Color.Black, Color.White, new Bitmap(Image.FromFile("C:\\tmp\\ic_launcher.png"))) : null);
            }
            else if (purchaseReceiptUploadResponse.status == SELLER_DOES_NOT_EXIST)
            {
                throw new InvalidSellerIdException(String.Format("Seller id {0} does not exist.", purchaseReceipt.sellerId));
            }
            else if (purchaseReceiptUploadResponse.status == SELLER_BRANCH_DOES_NOT_EXIST)
            {
                throw new InvalidSellerBranchIdException(String.Format("Seller branch id {0} does not exist.", purchaseReceipt.sellerId));
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

        [DataMember(Name = "path")]
        public string path { get; set; }

        [DataMember(Name = "is_linked_with_household")]
        public bool isLinkedWithHousehold { get; set; }
    }

}
