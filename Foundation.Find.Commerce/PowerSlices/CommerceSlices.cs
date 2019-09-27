﻿using EPiServer.Commerce.Marketing;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using Foundation.Commerce.Models.Catalog;
using PowerSlice;

namespace Foundation.Find.Commerce.PowerSlices
{
    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class ProductsSlice : ContentSliceBase<GenericProduct>
    {
        public override string Name => "Products";

        public override int SortOrder => 100;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class PackagesSlice : ContentSliceBase<GenericPackage>
    {
        public override string Name => "Packages";

        public override int SortOrder => 101;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class BundlesSlice : ContentSliceBase<GenericBundle>
    {
        public override string Name => "Bundles";

        public override int SortOrder => 102;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class VariantsSlice : ContentSliceBase<GenericVariant>
    {
        public override string Name => "Variants";

        public override int SortOrder => 103;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class OrderPromotionsSlice : ContentSliceBase<OrderPromotion>
    {
        public override string Name => "Order discounts";

        public override int SortOrder => 111;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class ShippingPromotionsSlice : ContentSliceBase<ShippingPromotion>
    {
        public override string Name => "Shipping discounts";

        public override int SortOrder => 112;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class EntryPromotionsSlice : ContentSliceBase<EntryPromotion>
    {
        public override string Name => "Item discounts";

        public override int SortOrder => 113;
    }
}
