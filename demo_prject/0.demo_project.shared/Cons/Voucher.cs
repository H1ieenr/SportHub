
namespace mini_form.shared
{
    public enum VoucherStatusEnum
    {
        Imported = 0,
        Issued = 1,
        Collected = 2,
        Activated = 3,
        Redeemed = 4,
        Expired = 5,
        Disabled = 6
    }

    public static class VoucherStatusKeyConst
    {
        public const string PARENT_KEY = "voucher.status";

        public const string IMPORTED = $"{PARENT_KEY}.imported";
        public const string ISSUED = $"{PARENT_KEY}.issued";
        public const string COLLECTED = $"{PARENT_KEY}.collected";
        public const string ACTIVATED = $"{PARENT_KEY}.activated";
        public const string REDEEMED = $"{PARENT_KEY}.redeemed";
        public const string EXPIRED = $"{PARENT_KEY}.expired";
        public const string DISABLED = $"{PARENT_KEY}.disabled";

        // Thêm name hiển thị
        public const string IMPORTED_NAME = "Đã nhập";
        public const string ISSUED_NAME = "Đã phát hành";
        public const string COLLECTED_NAME = "Đã thu thập";
        public const string ACTIVATED_NAME = "Đã kích hoạt";
        public const string REDEEMED_NAME = "Đã sử dụng";
        public const string EXPIRED_NAME = "Hết hạn";
        public const string DISABLED_NAME = "Đã vô hiệu";

        // Hàm tiện ích: convert Enum -> status_key string
        public static string GetKey(VoucherStatusEnum status)
        {
            return $"{PARENT_KEY}.{status.ToString().ToLower()}";
        }
        // Key -> name
        public static string GetNameFromKey(string key)
        {
            return key switch
            {
                var k when k == IMPORTED => IMPORTED_NAME,
                var k when k == ISSUED => ISSUED_NAME,
                var k when k == COLLECTED => COLLECTED_NAME,
                var k when k == ACTIVATED => ACTIVATED_NAME,
                var k when k == REDEEMED => REDEEMED_NAME,
                var k when k == EXPIRED => EXPIRED_NAME,
                var k when k == DISABLED => DISABLED_NAME,
                _ => "Không xác định"
            };
        }

    }
}