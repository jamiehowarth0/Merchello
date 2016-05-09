//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.2.93
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace Umbraco.Web.PublishedContentModels
{
	/// <summary>Store</summary>
	[PublishedContentModel("store")]
	public partial class Store : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "store";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Store(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Store, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Brief: A brief description of the store.
		///</summary>
		[ImplementPropertyType("brief")]
		public IHtmlString Brief
		{
			get { return this.GetPropertyValue<IHtmlString>("brief"); }
		}

		///<summary>
		/// Featured Products: The featured product collection
		///</summary>
		[ImplementPropertyType("featuredProducts")]
		public IEnumerable<Merchello.Web.Models.VirtualContent.IProductContent> FeaturedProducts
		{
			get { return this.GetPropertyValue<IEnumerable<Merchello.Web.Models.VirtualContent.IProductContent>>("featuredProducts"); }
		}

		///<summary>
		/// Headline: Store Headline
		///</summary>
		[ImplementPropertyType("headline")]
		public string Headline
		{
			get { return this.GetPropertyValue<string>("headline"); }
		}

		///<summary>
		/// Store Name: The name of the store
		///</summary>
		[ImplementPropertyType("storeName")]
		public string StoreName
		{
			get { return this.GetPropertyValue<string>("storeName"); }
		}
	}
}
