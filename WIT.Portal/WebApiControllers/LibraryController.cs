using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIT.Business.Entities;
using WIT.Interfaces;
using WIT.Portal.TokenManagement;
using WIT.Business;
using WIT.Business.Common;
using System.Security.Claims;
using Ninject;
using WIT.Data.Models;
using System.Data.Entity;

namespace WIT.Portal.WebApiControllers
{
    [RoutePrefix("api/library")]
    public class LibraryController : BaseApiController
    {

        //[Inject]
        //public IProductFeatureDataService _customerDataService { get; set; }

        private WitEntities _db = new WitEntities();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// GetLItems
        /// </summary>
        /// <param name="request"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("GetItems")]
        [HttpPost]
        public HttpResponseMessage GetLItems(HttpRequestMessage request, [FromBody] GridInfo info)
        {
            return BaseAction(request, (transaction) => {

                var itemType = _db.LItemTypes.FirstOrDefault(i => i.ItemTypeID == info.Id);

                if (itemType == null)
                {
                    transaction.ReturnMessage = string.Format("Item type {0} not found", info.Id);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                // make grid payload
                ItemGrid grid = new ItemGrid(info);

                // fields
                ItemField sortField = new ItemField()
                {
                    Id = -1,
                    Key = "id",
                    Name = "Id",
                    PropType = LPropTypeEnum.Integer,
                    GridHide = true
                };

                grid.Fields.Add(sortField);

                grid.Fields.Add(new ItemField()
                {
                    Id = 0,
                    Key = "key",
                    Name = "Key",
                    PropType = LPropTypeEnum.String,
                    GridHide = false
                });

                foreach (var prop in itemType.LItemProps)
                {
                    ItemField field = new ItemField(prop);

                    grid.Fields.Add(field);

                    if (field.Id == info.SortId)
                    {
                        sortField = field;
                    }
                }

                /*
                    select i.ItemID
                    from LItem i
                    left join LItemValue sv on sv.ItemID=i.ItemID and sv.ItemPropID=1
                    left join LItemValueString svs on svs.ItemValueID=sv.ItemValueID
                    where i.[Key] like '%%' 
                    or exists(select 1 from dbo.LItemValueString fvs inner join LItemValue fv on fv.ItemValueID=fvs.ItemValueID where fv.ItemID=i.ItemID)
                    order by svs.Value, i.ItemID desc
                */

                // items filter
                var q = _db.LItems.AsQueryable();

                if (!string.IsNullOrWhiteSpace(info.Filter))
                {
                    q = q.Where(i => i.Key.Contains(info.Filter) || i.LItemValues.Any(v => v.LItemValueString.Value.Contains(info.Filter)));
                }

                // total items count
                info.TotalRows = q.Count();

                // order by
                if (string.IsNullOrWhiteSpace(info.SortDirection))
                    info.SortDirection = "ASC";

                if (sortField.Id < 0)
                {
                    if (info.SortDirection.StartsWith("D"))
                        q = q.OrderByDescending(i => i.ItemID);
                    else
                        q = q.OrderBy(i => i.ItemID);
                }

                else if (sortField.Id == 0)
                {
                    if (info.SortDirection.StartsWith("D"))
                        q = q.OrderByDescending(i => i.Key);
                    else
                        q = q.OrderBy(i => i.Key);
                }

                else
                {
                    switch (sortField.PropType)
                    {
                        case LPropTypeEnum.String:
                            var sv1 = q
                                .Join(_db.LItemValues.DefaultIfEmpty(),
                                    i => new { i.ItemID, ItemPropID = sortField.Id },
                                    v => new { v.ItemID, v.ItemPropID },
                                    (i, v) => new { LItem = i, Value = v.LItemValueString.Value }
                                );
                            if (info.SortDirection.StartsWith("D"))
                                q = sv1.OrderByDescending(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            else
                                q = sv1.OrderBy(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            break;

                        case LPropTypeEnum.Integer:
                            var sv2 = q
                                .Join(_db.LItemValues.DefaultIfEmpty(),
                                    i => new { i.ItemID, ItemPropID = sortField.Id },
                                    v => new { v.ItemID, v.ItemPropID },
                                    (i, v) => new { LItem = i, Value = v.LItemValueInteger.Value }
                                );
                            if (info.SortDirection.StartsWith("D"))
                                q = sv2.OrderByDescending(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            else
                                q = sv2.OrderBy(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            break;

                        case LPropTypeEnum.Decimal:
                            var sv3 = q
                                .Join(_db.LItemValues.DefaultIfEmpty(),
                                    i => new { i.ItemID, ItemPropID = sortField.Id },
                                    v => new { v.ItemID, v.ItemPropID },
                                    (i, v) => new { LItem = i, Value = v.LItemValueDecimal.Value }
                                );
                            if (info.SortDirection.StartsWith("D"))
                                q = sv3.OrderByDescending(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            else
                                q = sv3.OrderBy(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            break;

                        case LPropTypeEnum.Date:
                        case LPropTypeEnum.Time:
                        case LPropTypeEnum.DateTime:
                            var sv4 = q
                                .Join(_db.LItemValues.DefaultIfEmpty(),
                                    i => new { i.ItemID, ItemPropID = sortField.Id },
                                    v => new { v.ItemID, v.ItemPropID },
                                    (i, v) => new { LItem = i, Value = v.LItemValueDateTime.Value }
                                );
                            if (info.SortDirection.StartsWith("D"))
                                q = sv4.OrderByDescending(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            else
                                q = sv4.OrderBy(s => s.Value).ThenBy(s => s.LItem.ItemID).Select(s => s.LItem);
                            break;
                    }
                }

                // data page before values
                var itemIds = q.Page(info).Select(i => i.ItemID).ToArray();

                var valueQuery = _db.LItems.Where(i => itemIds.Contains(i.ItemID))
                    .Join(_db.LItemProps, 
                        i=> i.ItemTypeID, 
                        p => p.ItemTypeID, 
                        (i, p) => new { LItem = i, LItemProp = p }
                        )
                    .Join(_db.LItemValues.DefaultIfEmpty(), 
                        ip => new { ItemID = ip.LItem.ItemID, ItemPropID = ip.LItemProp.ItemPropID }, 
                        v => new { ItemID = v.ItemID, ItemPropID = v.ItemPropID }, 
                        (ip, v) => new { LItem = ip.LItem, LItemProp = ip.LItemProp, LItemValue = v }
                        )
                    .OrderBy(v => v.LItemValue.ItemID)
                    .ThenBy(v => v.LItemProp.ItemPropID)
                    .Select(v => new
                    {
                        ItemID = v.LItem.ItemID,
                        Key = v.LItem.Key,
                        ItemPropID = v.LItemProp.ItemPropID,
                        PropType = v.LItemProp.PropType,
                        ItemValueID = v.LItemValue.ItemValueID,
                        ValueString = v.LItemValue.LItemValueString != null ? v.LItemValue.LItemValueString.Value : null,
                        ValueText = v.LItemValue.LItemValueText != null ? v.LItemValue.LItemValueText.Value : null,
                        ValueInteger = v.LItemValue.LItemValueInteger != null ? (int?)v.LItemValue.LItemValueInteger.Value : null,
                        ValueDecimal = v.LItemValue.LItemValueDecimal != null ? (decimal?)v.LItemValue.LItemValueDecimal.Value : null,
                        ValueDateTime = v.LItemValue.LItemValueDateTime != null ? (DateTime?)v.LItemValue.LItemValueDateTime.Value : null
                    });

                System.Diagnostics.Debug.Print(((System.Data.Entity.Core.Objects.ObjectQuery)valueQuery).ToTraceString());

                long itemId = 0;
                Dictionary<string, object> itemValues = null;
                foreach (var value in valueQuery.ToList())
                {
                    if (value.ItemID != itemId)
                    {
                        // add previous
                        if (itemValues != null)
                            grid.Data.Add(itemValues);

                        itemId = value.ItemID;
                        itemValues = new Dictionary<string, object>();
                        itemValues.Add("id", value.ItemID);
                        itemValues.Add("key", value.Key);
                    }

                    itemValues.Add(string.Format("p{0}", value.ItemPropID), value.ValueString);
                }

                transaction.Data = grid;
            });
        }


        #region ProductFeature

        /// <summary>
        /// GetProductFeatures
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProductFeatures")]
        [HttpPost]
        public HttpResponseMessage GetProductFeatures(HttpRequestMessage request, [FromBody] ProductFeatureList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "Code";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.ProductFeatures.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.Code))
                {
                    q = q.Where(i => i.Code.Contains(list.Code));
                }

                if (!string.IsNullOrWhiteSpace(list.Name))
                {
                    q = q.Where(i => i.Name.Contains(list.Name));
                }

                list.GridInfo.TotalRows = q.Count();
                list.Items = q
                    .Page(list.GridInfo)
                    .ToList()
                    .Select(i => new ProductFeatureItem(i))
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProductFeature")]
        [HttpPost]
        public HttpResponseMessage GetProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureItem item)
        {
            return BaseAction(request, (transaction) => {

                ProductFeature existingItem = _db.ProductFeatures.FirstOrDefault(i => i.ProductFeatureID == item.ProductFeatureID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product feature {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = new ProductFeatureItem(existingItem);
            });
        }

        /// <summary>
        /// DeleteProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProductFeature")]
        [HttpPost]
        public HttpResponseMessage DeleteProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureItem item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                ProductFeature existingItem = _db.ProductFeatures.FirstOrDefault(i => i.ProductFeatureID == item.ProductFeatureID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product feature {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.ProductFeatures.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = new ProductFeatureItem(existingItem);
            });
        }

        /// <summary>
        /// UpdateProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProductFeature")]
        [HttpPost]
        public HttpResponseMessage UpdateProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ProductFeatureValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                ProductFeature existingItem = _db.ProductFeatures.FirstOrDefault(i => i.ProductFeatureID == item.ProductFeatureID);

                if (existingItem == null)
                {
                    existingItem = _db.ProductFeatures.Add(new ProductFeature());

                    existingItem.Code = item.Code;

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                existingItem.Name = item.Name;
                existingItem.Description = item.Description;

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = new ProductFeatureItem(existingItem);
            });
        }

        #endregion  // ProductFeature

        #region ProductClass

        /// <summary>
        /// GetProductClasss
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProductClasses")]
        [HttpPost]
        public HttpResponseMessage GetProductClasses(HttpRequestMessage request, [FromBody] ProductClassList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "Code";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.ProductClasses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.Code))
                {
                    q = q.Where(i => i.Code.Contains(list.Code));
                }

                if (!string.IsNullOrWhiteSpace(list.Name))
                {
                    q = q.Where(i => i.Name.Contains(list.Name));
                }

                list.GridInfo.TotalRows = q.Count();
                list.Items = q
                    .Page(list.GridInfo)
                    .ToList()
                    .Select(i => new ProductClassItem(i))
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProductClass")]
        [HttpPost]
        public HttpResponseMessage GetProductClass(HttpRequestMessage request, [FromBody] ProductClassItem item)
        {
            return BaseAction(request, (transaction) => {

                ProductClass existingItem = _db.ProductClasses.FirstOrDefault(i => i.ProductClassID == item.ProductClassID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product class {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = new ProductClassItem(existingItem);
            });
        }

        /// <summary>
        /// DeleteProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProductClass")]
        [HttpPost]
        public HttpResponseMessage DeleteProductClass(HttpRequestMessage request, [FromBody] ProductClassItem item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                ProductClass existingItem = _db.ProductClasses.FirstOrDefault(i => i.ProductClassID == item.ProductClassID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product class {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.ProductClasses.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = new ProductClassItem(existingItem);
            });
        }

        /// <summary>
        /// UpdateProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProductClass")]
        [HttpPost]
        public HttpResponseMessage UpdateProductClass(HttpRequestMessage request, [FromBody] ProductClassItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ProductClassValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                ProductClass existingItem = _db.ProductClasses.FirstOrDefault(i => i.ProductClassID == item.ProductClassID);

                if (existingItem == null)
                {
                    existingItem = _db.ProductClasses.Add(new ProductClass());

                    existingItem.Code = item.Code;
                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                existingItem.Name = item.Name;
                existingItem.Description = item.Description;

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = new ProductClassItem(existingItem);
            });
        }

        #endregion  // ProductClass

        #region Product

        /// <summary>
        /// GetProducts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProducts")]
        [HttpPost]
        public HttpResponseMessage GetProducts(HttpRequestMessage request, [FromBody] ProductList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "ProductCode";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.Code))
                {
                    q = q.Where(i => i.ProductCode.Contains(list.Code));
                }

                if (!string.IsNullOrWhiteSpace(list.Name))
                {
                    q = q.Where(i => i.ProductName.Contains(list.Name));
                }

                list.GridInfo.TotalRows = q.Count();
                list.Items = q
                    .Page(list.GridInfo)
                    .ToList()
                    .Select(i => new ProductItem(i))
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProduct")]
        [HttpPost]
        public HttpResponseMessage GetProduct(HttpRequestMessage request, [FromBody] ProductItem item)
        {
            return BaseAction(request, (transaction) => {

                Product existingItem = _db.Products.FirstOrDefault(i => i.ProductID == item.ProductID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product {0} not found", item.ProductCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = new ProductItem(existingItem);
            });
        }

        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProduct")]
        [HttpPost]
        public HttpResponseMessage DeleteProduct(HttpRequestMessage request, [FromBody] ProductItem item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                Product existingItem = _db.Products.FirstOrDefault(i => i.ProductID == item.ProductID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product {0} not found", item.ProductCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.Products.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = new ProductItem(existingItem);
            });
        }

        /// <summary>
        /// UpdateProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProduct")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(HttpRequestMessage request, [FromBody] ProductItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ProductValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                Product existingItem = _db.Products.FirstOrDefault(i => i.ProductID == item.ProductID);

                if (existingItem == null)
                {
                    existingItem = _db.Products.Add(new Product());

                    existingItem.ProductCode = item.ProductCode;

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                existingItem.ProductName = item.ProductName;
                existingItem.Description = item.Description;

                existingItem.ProductClassID = _db.ProductClasses.FirstOrDefault(i => i.Code == item.ProductClass)?.ProductClassID;
                existingItem.ProductFeatureID = _db.ProductFeatures.FirstOrDefault(i => i.Code == item.ProductFeature)?.ProductFeatureID;

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = new ProductItem(existingItem);
            });
        }

        #endregion  // Product

        #region Customer

        /// <summary>
        /// GetCustomers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetCustomers")]
        [HttpPost]
        public HttpResponseMessage GetCustomers(HttpRequestMessage request, [FromBody] CustomerList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "CustomerCode";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.CustomerCode))
                {
                    q = q.Where(i => i.CustomerCode.Contains(list.CustomerCode));
                }

                if (!string.IsNullOrWhiteSpace(list.CompanyName))
                {
                    q = q.Where(i => i.CompanyName.Contains(list.CompanyName));
                }

                list.GridInfo.TotalRows = q.Count();
                list.Items = q
                    .Page(list.GridInfo)
                    .ToList()
                    .Select(i => new CustomerItem(i))
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetCustomer")]
        [HttpPost]
        public HttpResponseMessage GetCustomer(HttpRequestMessage request, [FromBody] CustomerItem item)
        {
            return BaseAction(request, (transaction) => {

                Customer existingItem = _db.Customers.FirstOrDefault(i => i.CustomerID == item.CustomerID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Customer {0} not found", item.CustomerCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = new CustomerItem(existingItem);
            });
        }

        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteCustomer")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomer(HttpRequestMessage request, [FromBody] CustomerItem item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                Customer existingItem = _db.Customers.FirstOrDefault(i => i.CustomerID == item.CustomerID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Customer {0} not found", item.CustomerCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.Customers.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = new CustomerItem(existingItem);
            });
        }

        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateCustomer")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomer(HttpRequestMessage request, [FromBody] CustomerItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!CustomerValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                Customer existingItem = _db.Customers.FirstOrDefault(i => i.CustomerID == item.CustomerID);

                if (existingItem == null)
                {
                    existingItem = _db.Customers.Add(new Customer());

                    existingItem.CustomerCode = item.CustomerCode;

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                existingItem.CompanyName = item.CompanyName;
                existingItem.AddressLine1 = item.AddressLine1;
                existingItem.AddressLine2 = item.AddressLine2;
                existingItem.City = item.City;
                existingItem.State = item.State;
                existingItem.ZipCode = item.ZipCode;
                existingItem.PhoneNumber = item.PhoneNumber;

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = new CustomerItem(existingItem);
            });
        }

        #endregion  // Customer
    }
}