﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using IntexII.Models.ViewModels;

namespace IntexII.Infrastructure
{
    //Create a tag helper that will dynamically add pagination links for however many pages there are and style them
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            urlHelperFactory = temp;
        }
        //Set up different fields for tag helpers
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public string? PageAction { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        public PaginationInfo PageModel { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; } = String.Empty;
        public string PageClassNormal { get; set; } = String.Empty;
        public string PageClassSelected { get; set; } = String.Empty;
        public override void Process(TagHelperContext context, TagHelperOutput output)
{
    if (ViewContext != null && PageModel != null)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
        TagBuilder result = new TagBuilder("div");
        result.AddCssClass("pagination-wrapper"); // Add a wrapper class

        // Create a container for pagination links
        TagBuilder paginationContainer = new TagBuilder("div");
        paginationContainer.AddCssClass("pagination-links");

        for (int i = 1; i <= PageModel.TotalNumPages; i++)
        {
            TagBuilder tag = new TagBuilder("a");
            PageUrlValues["pageNum"] = i;
            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            if (PageClassesEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
            }
            tag.InnerHtml.Append(i.ToString());
            paginationContainer.InnerHtml.AppendHtml(tag);
        }

        result.InnerHtml.AppendHtml(paginationContainer);

        output.Content.AppendHtml(result.InnerHtml);
    }
}

    }
}
