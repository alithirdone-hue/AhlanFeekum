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
                AhlanFeekumMenus.VerificationCodes,
                l["Menu:VerificationCodes"],
                url: "/verification-codes",
                icon: "fa fa-file-alt",
                requiredPermissionName: AhlanFeekumPermissions.VerificationCodes.Default)
        );

        return Task.CompletedTask;
    }
}
