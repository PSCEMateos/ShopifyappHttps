using System;
using System.Collections.Generic;

namespace ShopifyAppSherable.Models
{
    public class WebhookModel
    {
        // Common fields across Shopify webhooks
        public string id { get; set; }// Generic ID field
        public string AdminGraphqlApiId { get; set; } // GraphQL API ID
        public string Title { get; set; } // Product title
        public string BodyHtml { get; set; } // Product description in HTML
        public string ProductType { get; set; } // Product type/category
        public DateTime? PublishedAt { get; set; } // Publish date
        public string Handle { get; set; } // Product handle (URL slug)
        public string Vendor { get; set; } // Vendor name
        public string Topic { get; set; }// Webhook topic (e.g., "CARTS_UPDATE", "APP_UNINSTALLED")
        public DateTime? created_at { get; set; }// Timestamp for the event
        public DateTime? updated_at { get; set; } // Timestamp for updates
        public string token { get; set; } // Unique token (if applicable)
        public string TemplateSuffix { get; set; } // Template suffix
        public string Status { get; set; } // Product status (e.g., "active")
        public string PublishedScope { get; set; } // Published scope (e.g., "web")
        public List<string> Tags { get; set; } // Product tags
        public List<ShopifyVariantDetails> Variants { get; set; } // Product variants
        public List<ShopifyMediaDetails> Media { get; set; } // Media items
        public List<ShopifyVariantGid> VariantGids { get; set; } // Variant GraphQL IDs
        public bool HasVariantsThatRequiresComponents { get; set; } // Indicates if variants require components
        public string Category { get; set; } // Product category

        // Payload-specific fields (Dynamic or JSON objects)
        public Dictionary<string, object> Data { get; set; } // General-purpose dictionary for JSON payload data
        public List<ShopifyLineItem> line_items { get; set; } // For payloads with line items (e.g., carts)

        // Additional fields for specific webhooks
        public ShopifyAppDetails AppDetails { get; set; } // For app-related webhooks
        public ShopifyCheckoutDetails CheckoutDetails { get; set; } // For checkout-related webhooks
        public ShopifyCollectionDetails CollectionDetails { get; set; } // For collection-related webhooks
        public ShopifyAuditDetails AuditDetails { get; set; } // For audit events
        public ShopifyCompanyDetails CompanyDetails { get; set; } // For company-related webhooks
        public ShopifyCustomerDetails CustomerDetails { get; set; } // For customer-related webhooks
        public ShopifyDiscountDetails DiscountDetails { get; set; } // For discount-related webhooks
        public ShopifyDisputeDetails DisputeDetails { get; set; } // For dispute-related webhooks

        // Fallback for unstructured data
        public string RawPayload { get; set; } // Raw JSON payload as a string

    }
    public class WebhookValidationRequest
    {
        public string Challenge { get; set; }
    }// Nested models for common data structures
    public class ShopifyLineItem
    {
        public long id { get; set; }
        public int quantity { get; set; }
        public string variant_id { get; set; }
        public string key { get; set; }
        public string discounted_price { get; set; }
        public ShopifyDiscountDetails discounts { get; set; }
        public string gift_card { get; set; }
        public string grams { get; set; }
        public string line_price { get; set; }
        public string original_line_price { get; set; }
        public string original_price { get; set; }
        public string price { get; set; }
        public string product_id { get; set; }
        public string sku { get; set; }
        public string taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public string vendor { get; set; }
        public string Price { get; set; }
        public string Vendor { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; } // For extra fields
    }
    public class ShopifyLineItemProperties
    {
        public long id { get; set; }
    }
    public class ShopifyLineItemPriceSet
    {
        public long id { get; set; }
    }
    public class ShopifyLineItemMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class ShopifyAppDetails
    {
        public string Scopes { get; set; } // Scopes for app/scopes_update
        public string Status { get; set; } // Status for app/uninstalled
        public string AppId { get; set; } // App ID or similar identifier
    }

    public class ShopifyCheckoutDetails
    {
        public string CheckoutId { get; set; }
        public string CartToken { get; set; }
        public List<ShopifyLineItem> LineItems { get; set; }
    }

    public class ShopifyCollectionDetails
    {
        public string CollectionId { get; set; }
        public string Action { get; set; } // e.g., "add", "remove", "update"
    }

    public class ShopifyAuditDetails
    {
        public string AdminId { get; set; }
        public string Activity { get; set; } // e.g., "bulk_operations/finish"
        public string Description { get; set; }
    }

    public class ShopifyCompanyDetails
    {
        public string CompanyId { get; set; } // Company ID for company-related webhooks
        public string ContactId { get; set; } // Contact ID for contact-related events
        public string LocationId { get; set; } // Location ID for location-related events
        public string Role { get; set; } // Role assigned or revoked
        public string Action { get; set; } // e.g., "create", "delete", "update"
    }

    public class ShopifyCustomerDetails
    {
        public string CustomerId { get; set; } // Customer ID for customer-related webhooks
        public List<string> Tags { get; set; } // Tags added or removed
        public string AccountSettings { get; set; } // Account settings for updates
        public string PaymentMethodId { get; set; } // Payment method ID for customer payment methods
        public string Action { get; set; } // e.g., "create", "delete", "update"
        public string MergeTargetCustomerId { get; set; } // Target customer ID for merge events
        public string ConsentType { get; set; } // Marketing or email consent type
        public string DataRequestId { get; set; } // Data request ID for GDPR-related events
    }

    public class ShopifyDiscountDetails
    {
        public string DiscountId { get; set; } // Discount ID for discount-related webhooks
        public string Code { get; set; } // Discount code details
        public string Action { get; set; } // e.g., "create", "delete", "update"
    }

    public class ShopifyDisputeDetails
    {
        public string DisputeId { get; set; } // Dispute ID for dispute-related webhooks
        public string Status { get; set; } // Status of the dispute (e.g., "open", "won", "lost")
        public string Reason { get; set; } // Reason for the dispute
    }
    public class ShopifyVariantDetails
    {
        public long Id { get; set; } // Variant ID
        public string AdminGraphqlApiId { get; set; } // GraphQL API ID
        public string Title { get; set; } // Variant title
        public string Sku { get; set; } // SKU
        public string Price { get; set; } // Price
        public string CompareAtPrice { get; set; } // Compare-at price
        public int InventoryQuantity { get; set; } // Inventory quantity
        public int OldInventoryQuantity { get; set; } // Previous inventory quantity
        public string InventoryPolicy { get; set; } // Inventory policy
        public bool Taxable { get; set; } // Taxable flag
        public string Option1 { get; set; } // Option 1
        public string Option2 { get; set; } // Option 2
        public string Option3 { get; set; } // Option 3
        public long? ImageId { get; set; } // Associated image ID
        public long? InventoryItemId { get; set; } // Inventory item ID
    }
    public class ShopifyVariantGid
    {
        public string AdminGraphqlApiId { get; set; } // GraphQL API ID
        public DateTime? UpdatedAt { get; set; } // Last update timestamp
    }

    public class ShopifyMediaDetails
    {
        public string AdminGraphqlApiId { get; set; } // Media GraphQL API ID
        public string MediaType { get; set; } // Media type (e.g., "image", "video")
        public string Src { get; set; } // Source URL
    }


}

/*Este modelo soporta los tópicos:
app/scopes_update
app/uninstalled
app_purchases_one_time/update
app_subscriptions/approaching_capped_amount
app_subscriptions/update
audit_events/admin_api_activity
bulk_operations/finish
carts/create
carts/update
channels/delete
checkouts/create
checkouts/delete
checkouts/update
collection_listings/add
collection_listings/remove
collection_listings/update
collection_publications/create
collection_publications/delete
collection_publications/update
collections/create
collections/delete
collections/update
companies/create
companies/delete
companies/update
company_contact_roles/assign
company_contact_roles/revoke
company_contacts/create
company_contacts/delete
company_contacts/update
company_locations/create
company_locations/delete
company_locations/update
customer.tags_added
customer.tags_removed
customer_account_settings/update
customer_groups/create
customer_groups/delete
customer_groups/update
customer_payment_methods/create
customer_payment_methods/revoke
customer_payment_methods/update
customers/create
customers/data_request
customers/delete
customers/disable
customers/enable
customers/merge
customers/redact
customers/update
customers_email_marketing_consent/update
customers_marketing_consent/update
discounts/create
discounts/delete
discounts/redeemcode_added
discounts/redeemcode_removed
discounts/update
disputes/create
PRODUCTS_UPDATE
*/