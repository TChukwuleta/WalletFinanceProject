using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.ViewModels
{
    public class MasterCardVM
    {
        public string hostRefNo { get; set; }
        public string actionCode { get; set; } = "I";
        public string branchCode { get; set; } = "003";
        public string productCode { get; set; } = "P";
        public string statusCode { get; set; } = "0";
        public string deliveryFlag { get; set; } = "1";
        public string deliveryDate { get; set; }
        public string languageInd { get; set; } = "en";
        public string createFlag { get; set; } = "N";
        public string photoIndicator { get; set; } = "0";
        public string applicationDate { get; set; }
        public string mobilePhone { get; set; }
        public string cityCode { get; set; }
        public string zipCode { get; set; }
        public int MyProperty { get; set; }
    }


    public class MailingAddressVM
    {
        public string addressLine1 { get; set; }
        public string cityCode { get; set; }
        public string zipCode { get; set; }
        public string countryCode { get; set; }  
    }

    public class MobileVM
    {
        public string addressLine1 { get; set; }
    }
}


/*{
"cardDetail": {
        "card": {
            "basicCardFlag": "0", "cardIndicator":"V"
        },
"customer": {
            "gender": "M", "customerDemographics": {
                "address": {
                    "addressLine1": "Shreecolony",
"cityCode": "pune",
"zipCode": "411007",
"countryCode": "356"
                },
"mailingAddress": {
                    "addressLine1": " Mastercard ",
"countryCode": "356"
},
"phonesMap": {
                    "MOBILE": {
                        "number": "912456678",
"type": " MOBILE"
                    }
                }
            },
"primaryAccount": {
                "number": "3478798",
"currency": "INR",
 "type": "10"
}*/