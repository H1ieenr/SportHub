namespace demo_project.app.contracts
{
    public static class VoucherScopeObject
    {
        public const string Service = "voucher.scope.object.service";
        public const string ServiceGroup = "voucher.scope.object.service.group";
        public const string Course = "voucher.scope.object.course";
        public const string Package = "voucher.scope.object.package";
        public const string Product = "voucher.scope.object.product";
        public const string ProductCategory = "voucher.scope.object.product.category";
    }

    public static class VoucherTypeKey
    {
        public const string Percent = "voucher.type.percent";
        public const string Amount = "voucher.type.amount";
    }

    public static class VoucherTypeTime
    {
        public const string Fixed = "voucher.type.time.fixed";
        public const string Dynamic = "voucher.type.time.dynamic";
    }
}
