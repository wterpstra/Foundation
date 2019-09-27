﻿using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Cms.ViewModels;
using Foundation.Cms.ViewModels.Blocks;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Blog.TagCloudBlock
{
    [TemplateDescriptor(Default = true)]
    public class TagCloudBlockController : BlockController<Cms.Blocks.TagCloudBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly CategoryRepository _categoryRepository;
        private readonly BlogTagFactory _blogTagFactory;

        public TagCloudBlockController(IContentLoader contentLoader,
            CategoryRepository categoryRepository,
            BlogTagRepository blogTagRepository,
            BlogTagFactory blogTagFactory)
        {
            _contentLoader = contentLoader;
            _categoryRepository = categoryRepository;
            _blogTagFactory = blogTagFactory;
        }

        public override ActionResult Index(Cms.Blocks.TagCloudBlock currentBlock)
        {
            var model = new TagCloudBlockModel(currentBlock)
            {
                Tags = GetTags(currentBlock.BlogTagLinkPage)
            };

            return PartialView(model);
        }

        public IEnumerable<BlogItemPageModel.TagItem> GetTags(ContentReference startTagLink)
        {
            var tags = new List<BlogItemPageModel.TagItem>();
            foreach (var item in BlogTagRepository.Instance.LoadTags())
            {
                var cat = _categoryRepository.Get(item.TagName);
                var url = string.Empty;

                if (startTagLink != null)
                {
                    url = _blogTagFactory.GetTagUrl(_contentLoader.Get<PageData>(startTagLink.ToPageReference()), cat);
                }

                tags.Add(new BlogItemPageModel.TagItem() { Count = item.Count, Title = item.DisplayName, Weight = item.Weight, Url = url });
            }
            return tags;
        }

    }
}
