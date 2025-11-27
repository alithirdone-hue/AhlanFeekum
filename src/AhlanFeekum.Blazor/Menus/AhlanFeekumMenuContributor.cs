using System.Threading.Tasks;
using AhlanFeekum.Localization;
using AhlanFeekum.MultiTenancy;
using AhlanFeekum.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace AhlanFeekum.Blazor.Menus;

public class AhlanFeekumMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<AhlanFeekumResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                AhlanFeekumMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        context.Menu.AddItem(
    new ApplicationMenuItem(
        AhlanFeekumMenus.UserProfiles,
        l["Menu:UserProfiles"],
        url: "/user-profiles",
icon: "fa fa-file-alt",
        requiredPermissionName: AhlanFeekumPermissions.UserProfiles.Default)
);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PropertyFeatures,
                l["Menu:PropertyFeatures"],
                url: "/property-features",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PropertyFeatures.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PropertyTypes,
                l["Menu:PropertyTypes"],
                url: "/property-types",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PropertyTypes.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.SiteProperties,
                l["Menu:SiteProperties"],
                url: "/site-properties",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.SiteProperties.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.FavoriteProperties,
                l["Menu:FavoriteProperties"],
                url: "/favorite-properties",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.FavoriteProperties.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PersonEvaluations,
                l["Menu:PersonEvaluations"],
                url: "/person-evaluations",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PersonEvaluations.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PropertyEvaluations,
                l["Menu:PropertyEvaluations"],
                url: "/property-evaluations",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PropertyEvaluations.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PropertyMedias,
                l["Menu:PropertyMedias"],
                url: "/property-medias",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PropertyMedias.Default)
        );



        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.Governorates,
                l["Menu:Governorates"],
                url: "/governorates",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.Governorates.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.SpecialAdvertisments,
                l["Menu:SpecialAdvertisments"],
                url: "/special-advertisments",
icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.SpecialAdvertisments.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.OnlyForYouSections,
                l["Menu:OnlyForYouSections"],
                url: "/only-for-you-sections",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.OnlyForYouSections.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.PropertyCalendars,
                l["Menu:PropertyCalendars"],
                url: "/property-calendars",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.PropertyCalendars.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.Statuses,
                l["Menu:Statuses"],
                url: "/statuses",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.Statuses.Default)
        );


        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.Reservations,
                l["Menu:Reservations"],
                url: "/reservations",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.Reservations.Default)
        );


        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.Tickets,
                l["Menu:Tickets"],
                url: "/tickets",
icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.Tickets.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.AhlanfeekumTerms,
                l["Menu:AhlanfeekumTerms"],
                url: "/ahlanfeekum-terms",
icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.AhlanfeekumTerms.Default)
        );


        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.UserNotifications,
                l["Menu:UserNotifications"],
                url: "/user-notifications",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.UserNotifications.Default)
        );


        context.Menu.AddItem(
            new ApplicationMenuItem(
                AhlanFeekumMenus.UserPayments,
                l["Menu:UserPayments"],
                url: "/user-payments",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.UserPayments.Default)
        );
        return Task.CompletedTask;
    }
}
