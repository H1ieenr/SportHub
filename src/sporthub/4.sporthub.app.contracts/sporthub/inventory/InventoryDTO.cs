using Shared.Common;
using sporthub.domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace sporthub.app.contracts.sporthub.inventory
{
    public class ChangeStockRequestDTO : BaseRequestDTO
    {
        public long product_variant_id { get; set; }
        public InventoryTransactionType type { get; set; }
        public int quantity { get; set; }
        public string? note { get; set; }
    }

    public class UpdateReorderLevelRequestDTO : BaseRequestDTO
    {
        public long product_variant_id { get; set; }
        public int reorder_level { get; set; }
    }
    public class InventoryGetByVariantIdRequestDTO : BaseRequestDTO { public long product_variant_id { get; set; } }
    public class GetInventoryPagedRequestDTO : BaseRequestDTO
    {
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = 20;
        public string? search_text { get; set; }      // SKU hoặc tên sản phẩm
        public long? product_id { get; set; }
        public bool? low_stock { get; set; }
        public string? sort_by { get; set; }          // sku | on_hand | available
        public string? sort_dir { get; set; }         // asc | desc
    }

    public class GetInventoryTransactionsPagedRequestDTO : BaseRequestDTO
    {
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = 20;
        public long product_variant_id { get; set; }
        public InventoryTransactionType? type { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
    }

    public class InventoryItemDTO
    {
        public long id { get; set; }
        public long product_variant_id { get; set; }
        public string sku { get; set; } = "";
        public string variant_name { get; set; } = "";
        public long product_id { get; set; }
        public string product_name { get; set; } = "";
        public int quantity_on_hand { get; set; }
        public int reserved_quantity { get; set; }
        public int available_quantity { get; set; }
        public int reorder_level { get; set; }
        public bool is_low_stock { get; set; }
    }

    public class InventoryTransactionDTO
    {
        public long id { get; set; }
        public InventoryTransactionType type { get; set; }
        public int quantity { get; set; }
        public string? reference_type { get; set; }
        public long? reference_id { get; set; }
        public string? note { get; set; }
        public DateTime created_date { get; set; }
        public long created_by { get; set; }
    }
}
