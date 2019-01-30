using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.DataAccess.SqlServer.Constants
{
    public static class Procedures
    {
        public static string SitesCreateUpdate => "Sites_CreateUpdate";
        public static string SitesGetBy => "Sites_GetBy";
        public static string PagesCreateUpdateDelete => "Pages_CreateUpdateDelete";
        public static string PagesGetBy => "Pages_GetBy";
        public static string ContentCreateUpdateDelete => "Content_CreateUpdateDelete";
        public static string ContentGetBy => "Content_GetBy";
        public static string MetadataCreateUpdateDelete => "Metadata_CreateUpdateDelete";
        public static string MetadataGetBy => "Metadata_GetBy";

        public static string MessagingTemplateCreateUpdateDelete => "MessagingTemplate_CreateUpdateDelete";
        public static string MessagingTemplateGetBy => "MessagingTemplate_GetBy";

        public static string ContentToPageMap => "ContentToPage_Map";

        public static string MenuCreateUpdateDelete => "Menu_CreateUpdateDelete";
        public static string MenuGetBy => "Menu_GetBy";

        public static string MenuItemsCreateUpdateDelete => "MenuItems_CreateUpdateDelete";
        public static string MenuItemsGetBy => "MenuItems_GetBy";

    }
}
