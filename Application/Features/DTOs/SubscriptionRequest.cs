using System.Text.Json.Serialization;

namespace Application.Features.DTOs
{
    public class SubscriptionRequest
    {
        public string PlanType { get; set; } = default!;
    }

    public class PaystackCallbackModel
    {
        public bool Status { get; set; }
        public PaystackDatas Data { get; set; }
    }

    public class PaystackResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public PaystackData Data { get; set; }
    }

    public class PaystackData
    {
        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; set; }

        [JsonPropertyName("access_code")]
        public string AccessCode { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }
    }


    public class PaystackDatas
    {
        public int Amount { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public PaystackCustomer Customer { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class PaystackCustomer
    {
        public string Email { get; set; }
    }

    public class Metadata
    {
        public string PlanType { get; set; }
    }


    public class PaystackVerifyResponse
    {
        public bool Status { get; set; } 
        public PaystackVerifyData Data { get; set; }
    }

    public class PaystackVerifyData
    {
        
        public string Status { get; set; } // 'success', 'failed', etc.
        public string Reference { get; set; }
        public int Amount { get; set; } // Amount in kobo (or the smallest unit of currency)
        public string GatewayResponse { get; set; }
   
        public PaystackMetadata Metadata { get; set; } // Custom metadata like the plan type
        public PaystackCustomer Customer { get; set; }
    }

    public class PaystackMetadata
    {
        public string PlanType { get; set; } // Custom metadata field for the plan type ("classic", "standard", etc.)
    }

}
