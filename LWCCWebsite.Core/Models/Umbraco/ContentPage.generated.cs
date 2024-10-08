//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.3.0+a325ba3
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace LWCCWebsite.Core.UmbracoModels
{
	/// <summary>Content Page</summary>
	[PublishedModel("contentPage")]
	public partial class ContentPage : PublishedContentModel, ICallToAction, IContactForm, IHero, INavigation, IUsefulLinks
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		public new const string ModelTypeAlias = "contentPage";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<ContentPage, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public ContentPage(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Content Grid
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("contentGrid")]
		public virtual global::Umbraco.Cms.Core.Models.Blocks.BlockGridModel ContentGrid => this.Value<global::Umbraco.Cms.Core.Models.Blocks.BlockGridModel>(_publishedValueFallback, "contentGrid");

		///<summary>
		/// Page Content: This is the Page Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("pageContent")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString PageContent => this.Value<global::Umbraco.Cms.Core.Strings.IHtmlEncodedString>(_publishedValueFallback, "pageContent");

		///<summary>
		/// Call To Action Link: Link to Something Somewhere
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("callToActionLink")]
		public virtual global::Umbraco.Cms.Core.Models.Link CallToActionLink => global::LWCCWebsite.Core.UmbracoModels.CallToAction.GetCallToActionLink(this, _publishedValueFallback);

		///<summary>
		/// Call To Action Text: Some Explanatory Text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("callToActionText")]
		public virtual string CallToActionText => global::LWCCWebsite.Core.UmbracoModels.CallToAction.GetCallToActionText(this, _publishedValueFallback);

		///<summary>
		/// Call To Action Title: This will be the title text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("callToActionTitle")]
		public virtual string CallToActionTitle => global::LWCCWebsite.Core.UmbracoModels.CallToAction.GetCallToActionTitle(this, _publishedValueFallback);

		///<summary>
		/// Conact Form Switch
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[ImplementPropertyType("conactFormSwitch")]
		public virtual bool ConactFormSwitch => global::LWCCWebsite.Core.UmbracoModels.ContactForm.GetConactFormSwitch(this, _publishedValueFallback);

		///<summary>
		/// Hero Image: This is the image to use as a background
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroImage")]
		public virtual global::Umbraco.Cms.Core.Models.MediaWithCrops HeroImage => global::LWCCWebsite.Core.UmbracoModels.Hero.GetHeroImage(this, _publishedValueFallback);

		///<summary>
		/// Hero Overlay Colour: This colour to give the overlay or tint
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroOverlayColour")]
		public virtual string HeroOverlayColour => global::LWCCWebsite.Core.UmbracoModels.Hero.GetHeroOverlayColour(this, _publishedValueFallback);

		///<summary>
		/// Hero Subtitle: This is the text that goes under the title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroSubtitle")]
		public virtual string HeroSubtitle => global::LWCCWebsite.Core.UmbracoModels.Hero.GetHeroSubtitle(this, _publishedValueFallback);

		///<summary>
		/// Hero Title: This is the Hero title text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroTitle")]
		public virtual string HeroTitle => global::LWCCWebsite.Core.UmbracoModels.Hero.GetHeroTitle(this, _publishedValueFallback);

		///<summary>
		/// Umbraco Navi Hide: If this is checked then the page won't appear in the menu
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[ImplementPropertyType("umbracoNaviHide")]
		public virtual bool UmbracoNaviHide => global::LWCCWebsite.Core.UmbracoModels.Navigation.GetUmbracoNaviHide(this, _publishedValueFallback);

		///<summary>
		/// Link Nested Content: This is nested content of links
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.3.0+a325ba3")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("linkNestedContent")]
		public virtual global::Umbraco.Cms.Core.Models.Blocks.BlockListModel LinkNestedContent => global::LWCCWebsite.Core.UmbracoModels.UsefulLinks.GetLinkNestedContent(this, _publishedValueFallback);
	}
}
