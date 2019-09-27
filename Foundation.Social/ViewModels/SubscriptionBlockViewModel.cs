using EPiServer.Core;
using Foundation.Social.Models.Blocks;
using System.Collections.Generic;

namespace Foundation.Social.ViewModels
{
    public class SubscriptionBlockViewModel
    {
        public SubscriptionBlockViewModel(SubscriptionBlock block, ContentReference currentLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            ShowSubscriptionForm = false;
            UserSubscribedToPage = false;
            CurrentLink = currentLink;
        }

        public bool ShowSubscriptionForm { get; set; }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public bool UserSubscribedToPage { get; set; }

        public ContentReference CurrentLink { get; set; }

        public List<MessageViewModel> Messages { get; set; }
    }
}