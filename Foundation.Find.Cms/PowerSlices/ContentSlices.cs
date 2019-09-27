﻿using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using PowerSlice;

namespace Foundation.Find.Cms.PowerSlices
{
    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class LandingPagesSlice : ContentSliceBase<LandingPage>
    {
        public override string Name => "Landing pages";

        public override int SortOrder => 10;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class StandardPagesSlice : ContentSliceBase<StandardPage>
    {
        public override string Name => "Standard Pages";

        public override int SortOrder => 11;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class BlogsSlice : ContentSliceBase<BlogItemPage>
    {
        public override string Name => "Blogs";

        public override int SortOrder => 12;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class BlocksSlice : ContentSliceBase<FoundationBlockData>
    {
        public override string Name => "Blocks";

        public override int SortOrder => 50;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class MediaSlice : ContentSliceBase<MediaData>
    {
        public override string Name => "Media";

        public override int SortOrder => 70;
    }

    [ServiceConfiguration(typeof(IContentQuery)), ServiceConfiguration(typeof(IContentSlice))]
    public class ImagesSlice : ContentSliceBase<ImageData>
    {
        public override string Name => "Images";

        public override int SortOrder => 71;
    }
}
