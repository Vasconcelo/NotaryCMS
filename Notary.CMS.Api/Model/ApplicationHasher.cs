//using Common.Security;

//namespace Notary.CMS.Api.Model
//{
//    public class ApplicationHasher: IApplicationHasher
//    {
//        private readonly IHasher _hasher;

//        public ApplicationHasher(IHasher hasher)
//        {
//            _hasher = hasher;
//        }

//        public string Hash(IProductVariant product)
//        {
//            var parts = new StringBuilder();
//            parts.Append(product.ProductTypeGuid);
//            parts.Append(product.ValidFromDate);
//            parts.Append(product.ValidToDate);
//            parts.Append(product.IsBundle);
//            parts.Append(product.CanBeSoldIndividually);
//            parts.Append(product.IsReplacementPart);

//            if (product.Properties == null || !product.Properties.Any())
//            {
//                parts.Append(product.ErpId);
//                return _hasher.Hash(parts.ToString());
//            }

//            foreach (var property in product.Properties)
//            {
//                parts.AppendFormat(CultureInfo.InvariantCulture, "{0}:{1}", property.AttributeName, !string.IsNullOrWhiteSpace(property.Value) ? property.Value : string.Join(",", property.Values));
//            }

//            return _hasher.Hash(parts.ToString());
//        }

//        public string Hash(Guid productTypeGuid, DateTime? validFromDate, DateTime? validToDate, bool? canBeSoldIndividually, bool? isReplacementPart, bool? isBundle, IList<ProductAttribute> properties, string bundleItem, string erpId = null)
//        {
//            var parts = new StringBuilder();
//            parts.Append(productTypeGuid);
//            parts.Append(validFromDate);
//            parts.Append(validToDate);
//            parts.Append(isBundle);
//            parts.Append(canBeSoldIndividually);
//            parts.Append(isReplacementPart);

//            if (isBundle == true)
//            {
//                parts.Append(bundleItem);
//            }

//            if (erpId != null && (properties == null || !properties.Any()))
//            {
//                parts.Append(erpId);
//                return _hasher.Hash(parts.ToString());
//            }

//            foreach (var property in properties)
//            {
//                parts.AppendFormat(CultureInfo.InvariantCulture, "{0}:{1}", property.AttributeName, property.Value);
//            }

//            return _hasher.Hash(parts.ToString());
//        }
//    }
//}
//}
