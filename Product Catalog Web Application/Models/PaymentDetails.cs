using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog_Web_Application.Models
{
    public enum PaymentTypes
    {
        Cash,
        Fawry,
        Visa
    }
    public enum status
    {
        Completed,
        Pending
    }
    public class PaymentDetails
    {
        public string Id { get; set; }
        public status? paymentStatus { get; set; }
        public PaymentTypes PaymentMehtod { get; set; }
        public DateTime creationDate { get; set; }
        public int amount { get; set; }
        [ForeignKey("order")]
        public string orderId { get; set; }
        public virtual Order? order { get; set; }

        public PaymentDetails() 
        {
          this.Id=Guid.NewGuid().ToString();
        }


        
    }
}
