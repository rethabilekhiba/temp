using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CreditCards.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Card Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must be numbers only")]
        [StringLength(16, ErrorMessage = "Can not exceed 16 characters")]
        public string CardNumber { get; set; }
        [Required]
        [DisplayName("Card Holder Name")]
        public string CardName { get; set; }
        //[Required]
        //[DisplayName("CVV")]
        //[StringLength(3, ErrorMessage = "Can not exceed 3 characters")]
        [RegularExpression(@"^[0-9]{3}", ErrorMessage = "Please enter 3 digits.")]
        public int CardCvv { get; set; }

        [DisplayName("Card Provider")]
        public string? CardType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Expires Date")]
        public DateTime CardExpDate { get; set; }
        public CreditCard() { }

        public CreditCard(int id, string cardNumber, string cardName, int cardCvv, string cardType, DateTime cardExpDate)
        {
            Id = id;
            CardNumber = cardNumber;
            CardName = cardName;
            CardCvv = cardCvv;
            CardType = cardType;
            CardExpDate = cardExpDate;
        }
    }
}
