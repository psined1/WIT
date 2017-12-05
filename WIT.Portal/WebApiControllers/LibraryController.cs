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
using System.Linq.Dynamic;

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

        #region Item Type

        /// <summary>
        /// GetItemTypes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("GetItemTypes")]
        [HttpPost]
        public HttpResponseMessage GetLItemTypes(HttpRequestMessage request, [FromBody] GridInfo info)
        {
            return BaseAction(request, (transaction) => {

                var q = _db.LItemTypes
                    .OrderBy(t => t.ItemTypeID)
                    .Include(t => t.LItemProps)
                    .AsQueryable()
                    ;

                // filter
                if (!string.IsNullOrWhiteSpace(info.Filter))
                {
                    q = q.Where(t => t.Name.Contains(info.Filter) || t.Description.Contains(info.Filter));
                }

                // total items count
                info.TotalRows = q.Count();

                // order by
                info.SortDirection = info.SortDirection.StartsWith("D") ? "DESC" : "ASC";

                if (string.IsNullOrWhiteSpace(info.SortExpression))
                {
                    info.SortExpression = "ItemTypeID";
                }

                q = q.OrderBy(info.SortExpression + " " + info.SortDirection);

                // return our list
                List<ItemEntity> list = new List<ItemEntity>(info.TotalRows);
                foreach (var t in q.Page(info).ToList())
                {
                    list.Add(new ItemEntity(_db, t));
                }

                transaction.Data = new
                {
                    GridInfo = info,
                    Items = list
                };

            });
        }

        /// <summary>
        /// DeleteItem
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("GetItemType")]
        [HttpGet]
        public HttpResponseMessage GetLItemType(HttpRequestMessage request, long itemTypeId)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                var existingItem = _db.LItemTypes.Include(t => t.LItemProps).FirstOrDefault(t => t.ItemTypeID == itemTypeId);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Item type with id {0} not found", itemTypeId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ItemEntity item = new ItemEntity(_db, existingItem);

                transaction.Data = item;
            });
        }

        /// <summary>
        /// DeleteItem
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("DeleteItemType")]
        [HttpGet]
        public HttpResponseMessage DeleteLItemType(HttpRequestMessage request, long itemTypeId)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                var existingItem = _db.LItemTypes.Include(t => t.LItemProps).FirstOrDefault(t => t.ItemTypeID == itemTypeId);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Item type with id {0} not found", itemTypeId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ItemEntity item = new ItemEntity(_db, existingItem);

                /* TODO: if (!ItemValidator.CheckDelete(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }*/

                _db.LItemTypes.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = item;
            });
        }

        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateItemType")]
        [HttpPost]
        public HttpResponseMessage UpdateItemType(HttpRequestMessage request, [FromBody] ItemEntity item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ItemTypeValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                LItemType existingItem = _db.LItemTypes
                    .Include(i => i.LItemProps)
                    .FirstOrDefault(i => i.ItemTypeID == item.ItemTypeId)
                    ;

                if (existingItem == null)
                {
                    existingItem = _db.LItemTypes.Add(new LItemType()
                    {
                        CreatedBy = transaction.CurrentUserEmail
                    });
                }

                existingItem.UpdatedBy = transaction.CurrentUserEmail;

                item.UpdateItemType(_db, existingItem, transaction);

                _db.SaveChanges();

                transaction.Data = new ItemEntity(_db, existingItem);
            });
        }

        #endregion  // Item Type

        #region Item

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

                var itemType = _db.LItemTypes.Include(t => t.LItemProps).FirstOrDefault(i => i.ItemTypeID == info.ItemTypeId);

                if (itemType == null)
                {
                    transaction.ReturnMessage = string.Format("Item type {0} not found", info.ItemTypeId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                info.Name = itemType.Name;
                info.Help = itemType.Description;

                // make grid payload
                ItemGrid grid = new ItemGrid(info);

                ItemField sortField = grid.Fields.FirstOrDefault(f => f.Id == 0);

                foreach (var prop in itemType.LItemProps.OrderBy(p => p.Radix))
                {
                    ItemField field = new ItemField(prop);

                    grid.Fields.Add(field);

                    if (field.Key == info.SortExpression)
                    {
                        sortField = field;
                    }
                }

                // items filter
                var q = _db.LItems.Where(i => i.ItemTypeID == itemType.ItemTypeID).AsQueryable();

                if (!string.IsNullOrWhiteSpace(info.Filter))
                {
                    q = q.Where(i => i.LItemValues.Any(v => v.LItemValueString.Value.Contains(info.Filter)));
                }

                // total items count
                info.TotalRows = q.Count();

                // order by
                if (string.IsNullOrWhiteSpace(info.SortDirection))
                    info.SortDirection = "ASC";

                if (sortField.Id <= 0)
                {
                    if (info.SortDirection.StartsWith("D"))
                        q = q.OrderByDescending(i => i.ItemID);
                    else
                        q = q.OrderBy(i => i.ItemID);
                }

                else
                {
                    var qv = from i in q
                        join v in _db.LItemValues.Where(vp => vp.ItemPropID == sortField.Id) on i.ItemID equals v.ItemID into g
                        from gv in g.DefaultIfEmpty()
                        select new {
                            LItem = i, ItemValueID = gv.ItemValueID
                        };

                    switch (sortField.PropType)
                    {
                        case LPropTypeEnum.String:
                            if (info.SortDirection.StartsWith("D"))
                                q = from qq in qv
                                    join vx in _db.LItemValueStrings on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value descending
                                    select qq.LItem
                                    ;
                            else
                                q = from qq in qv
                                    join vx in _db.LItemValueStrings on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value ascending
                                    select qq.LItem
                                    ;
                            break;

                        case LPropTypeEnum.Integer:
                            if (info.SortDirection.StartsWith("D"))
                                q = from qq in qv
                                    join vx in _db.LItemValueIntegers on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value descending
                                    select qq.LItem
                                    ;
                            else
                                q = from qq in qv
                                    join vx in _db.LItemValueIntegers on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value ascending
                                    select qq.LItem
                                    ;
                            break;

                        case LPropTypeEnum.Decimal:
                            if (info.SortDirection.StartsWith("D"))
                                q = from qq in qv
                                    join vx in _db.LItemValueDecimals on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value descending
                                    select qq.LItem
                                    ;
                            else
                                q = from qq in qv
                                    join vx in _db.LItemValueDecimals on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value ascending
                                    select qq.LItem
                                    ;
                            break;

                        case LPropTypeEnum.Date:
                        case LPropTypeEnum.Time:
                        case LPropTypeEnum.DateTime:
                            if (info.SortDirection.StartsWith("D"))
                                q = from qq in qv
                                    join vx in _db.LItemValueDateTimes on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value descending
                                    select qq.LItem
                                    ;
                            else
                                q = from qq in qv
                                    join vx in _db.LItemValueDateTimes on qq.ItemValueID equals vx.ItemValueID into g
                                    from v in g.DefaultIfEmpty()
                                    orderby v.Value ascending
                                    select qq.LItem
                                    ;
                            break;
                    }
                }

                // data page before values
                var items = q.Page(info).ToList();
                var itemIds = items.Select(i => i.ItemID).ToArray();

                var values = _db.LItemValues.Where(v => itemIds.Contains(v.ItemID))
                    .Include(v => v.LItemValueString)
                    .Include(v => v.LItemValueText)
                    .Include(v => v.LItemValueInteger)
                    .Include(v => v.LItemValueDecimal)
                    .Include(v => v.LItemValueDateTime)
                    .Include(v => v.LItemValueItem)
                    .OrderBy(v => v.ItemID)
                    .ThenBy(v => v.ItemPropID)
                    .ThenBy(v => v.ItemValueID)
                    .ToList()
                    ;

                Dictionary<string, object> itemData = null;
                foreach (var item in items)
                {
                    itemData = new Dictionary<string, object>(2 + itemType.LItemProps.Count);
                    itemData.Add("id", item.ItemID);
                    grid.Data.Add(itemData);

                    foreach (var prop in itemType.LItemProps.OrderBy(p => p.ItemPropID))
                    {
                        var itemValues = values
                            .Where(v => v.ItemID == item.ItemID && v.ItemPropID == prop.ItemPropID)
                            .OrderBy(v => v.ItemValueID)
                            ;

                        LItemValue sValue = null;

                        switch (prop.PropType)
                        {
                            case LPropTypeEnum.String:
                                sValue = itemValues.FirstOrDefault();
                                if (sValue != null)
                                    itemData.Add(string.Format("p{0}", sValue.ItemPropID), sValue.LItemValueString.Value);
                                break;

                            case LPropTypeEnum.Text:
                                sValue = itemValues.FirstOrDefault();
                                if (sValue != null)
                                    itemData.Add(string.Format("p{0}", sValue.ItemPropID), sValue.LItemValueText.Value);
                                break;

                            case LPropTypeEnum.Date:
                            case LPropTypeEnum.DateTime:
                            case LPropTypeEnum.Time:
                                sValue = itemValues.FirstOrDefault();
                                if (sValue != null)
                                    itemData.Add(string.Format("p{0}", sValue.ItemPropID), sValue.LItemValueDateTime.Value);
                                break;

                            case LPropTypeEnum.Integer:
                                sValue = itemValues.FirstOrDefault();
                                if (sValue != null)
                                    itemData.Add(string.Format("p{0}", sValue.ItemPropID), sValue.LItemValueInteger.Value);
                                break;

                            case LPropTypeEnum.Decimal:
                                sValue = itemValues.FirstOrDefault();
                                if (sValue != null)
                                    itemData.Add(string.Format("p{0}", sValue.ItemPropID), sValue.LItemValueDecimal.Value);
                                break;

                            case LPropTypeEnum.Item:
                                var linkedIds = itemValues
                                    .Select(v => v.LItemValueItem.ItemID)
                                    .Take(prop.Multiple ? itemValues.Count() : 1)
                                    .ToArray()
                                    ;
                                itemData.Add(string.Format("p{0}", sValue.ItemPropID), linkedIds);
                                break;

                            // TODO: implement other types
                            case LPropTypeEnum.Hyperlink:
                            case LPropTypeEnum.Image:
                            case LPropTypeEnum.Video:
                                break;
                        }
                    }
                }

                transaction.Data = grid;

            });
        }

        /// <summary>
        /// GetProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("GetItem")]
        [HttpGet]
        public HttpResponseMessage GetItem(HttpRequestMessage request, long itemId)
        {
            return BaseAction(request, (transaction) => {

                var existingItem = _db.LItems.Include(i => i.LItemType.LItemProps).FirstOrDefault(i => i.ItemID == itemId);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Item {0} not found", itemId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ItemEntity item = new ItemEntity(_db, existingItem);

                transaction.Data = item;
            });
        }

        /// <summary>
        /// GetProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("GetBlankItem")]
        [HttpGet]
        public HttpResponseMessage GetItemForType(HttpRequestMessage request, long itemTypeId)
        {
            return BaseAction(request, (transaction) => {

                var existingItemType = _db.LItemTypes.Include(t => t.LItemProps).FirstOrDefault(t => t.ItemTypeID == itemTypeId);

                if (existingItemType == null)
                {
                    transaction.ReturnMessage = string.Format("Item type {0} not found", itemTypeId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ItemEntity blankItem = new ItemEntity(_db, existingItemType);

                transaction.Data = blankItem;
            });
        }

        /// <summary>
        /// DeleteItem
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("DeleteItem")]
        [HttpGet]
        public HttpResponseMessage DeleteItem(HttpRequestMessage request, long itemId)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                var existingItem = _db.LItems.Include(i => i.LItemType.LItemProps).FirstOrDefault(i => i.ItemID == itemId);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Item {0} not found", itemId);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                ItemEntity item = new ItemEntity(_db, existingItem);

                if (!ItemValidator.CheckDelete(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                _db.LItems.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = item;
            });
        }


        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateItem")]
        [HttpPost]
        public HttpResponseMessage UpdateItem(HttpRequestMessage request, [FromBody] ItemEntity item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ItemValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                LItem existingItem = _db.LItems
                    .Include(i => i.LItemType.LItemProps)
                    .Include(i => i.LItemValues)
                    .FirstOrDefault(i => i.ItemID == item.Id)
                    ;

                if (existingItem == null)
                {
                    existingItem = _db.LItems.Add(new LItem() {
                        LItemType = _db.LItemTypes.FirstOrDefault(t => t.ItemTypeID == item.ItemTypeId),
                        CreatedBy = transaction.CurrentUserEmail
                    });
                }

                existingItem.UpdatedBy = transaction.CurrentUserEmail;

                item.UpdateItemValues(_db, existingItem, transaction);

                _db.SaveChanges();

                transaction.Data = new ItemEntity(_db, existingItem);
            });
        }

        #endregion  // Item

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