﻿using EPiServer.Commerce.Order;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Pages;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyAccount.OrderConfirmation
{
    public abstract class OrderConfirmationControllerBase<T> : PageController<T> where T : FoundationPageData
    {
        protected readonly ConfirmationService _confirmationService;
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly UrlResolver _urlResolver;
        protected readonly ICustomerService _customerService;

        protected OrderConfirmationControllerBase(ConfirmationService confirmationService,
            IAddressBookService addressBookService,
            IOrderGroupCalculator orderGroupTotalsCalculator,
            UrlResolver urlResolver,
            ICustomerService customerService)
        {
            _confirmationService = confirmationService;
            _addressBookService = addressBookService;
            _orderGroupCalculator = orderGroupTotalsCalculator;
            _urlResolver = urlResolver;
            _customerService = customerService;
        }

        protected OrderConfirmationViewModel<T> CreateViewModel(T currentPage, IPurchaseOrder order)
        {
            var hasOrder = order != null;

            if (!hasOrder)
            {
                return new OrderConfirmationViewModel<T>(currentPage);
            }

            var lineItems = order.GetFirstForm().Shipments.SelectMany(x => x.LineItems);
            var totals = _orderGroupCalculator.GetOrderGroupTotals(order);

            var viewModel = new OrderConfirmationViewModel<T>(currentPage)
            {
                Currency = order.Currency,
                CurrentContent = currentPage,
                HasOrder = hasOrder,
                OrderId = order.OrderNumber,
                Created = order.Created,
                Items = lineItems,
                BillingAddress = new AddressModel(),
                ShippingAddresses = new List<AddressModel>(),
                ContactId = PrincipalInfo.CurrentPrincipal.GetContactId(),
                Payments = order.GetFirstForm().Payments.Where(c => c.TransactionType == TransactionType.Authorization.ToString() || c.TransactionType == TransactionType.Sale.ToString()),
                OrderGroupId = order.OrderLink.OrderGroupId,
                OrderLevelDiscountTotal = order.GetOrderDiscountTotal(),
                ShippingSubTotal = order.GetShippingSubTotal(),
                ShippingDiscountTotal = order.GetShippingDiscountTotal(),
                ShippingTotal = totals.ShippingTotal,
                HandlingTotal = totals.HandlingTotal,
                TaxTotal = totals.TaxTotal,
                CartTotal = totals.Total,
                SubTotal = order.GetSubTotal(),
                FileUrls = new List<Dictionary<string, string>>(),
                Keys = new List<Dictionary<string, string>>()
            };

            foreach (var lineItem in lineItems)
            {
                var entry = lineItem.GetEntryContent<GenericVariant>();
                if (entry == null || entry.VirtualProductMode == null || entry.VirtualProductMode.Equals("None"))
                {
                    continue;
                }

                if (entry.VirtualProductMode.Equals("File"))
                {
                    var url = "";// _urlResolver.GetUrl(((FileVariant)lineItem.GetEntryContentBase()).File);
                    viewModel.FileUrls.Add(new Dictionary<string, string>() { { lineItem.DisplayName, url } });
                }
                else if (entry.VirtualProductMode.Equals("Key"))
                {
                    var key = Guid.NewGuid().ToString();
                    viewModel.Keys.Add(new Dictionary<string, string>() { { lineItem.DisplayName, key } });
                }
                else if (entry.VirtualProductMode.Equals("ElevatedRole"))
                {
                    viewModel.ElevatedRole = entry.VirtualProductRole;
                    var currentContact = _customerService.GetCurrentContact();
                    if (currentContact != null)
                    {
                        currentContact.ElevatedRole = ElevatedRoles.Reader.ToString();
                        currentContact.SaveChanges();
                    }
                }
            }

            var billingAddress = order.GetFirstForm().Payments.First().BillingAddress;

            // Map the billing address using the billing id of the order form.
            _addressBookService.MapToModel(billingAddress, viewModel.BillingAddress);

            // Map the remaining addresses as shipping addresses.
            foreach (var orderAddress in order.Forms.SelectMany(x => x.Shipments).Select(s => s.ShippingAddress))
            {
                var shippingAddress = new AddressModel();
                _addressBookService.MapToModel(orderAddress, shippingAddress);
                viewModel.ShippingAddresses.Add(shippingAddress);
            }

            return viewModel;
        }
    }
}
