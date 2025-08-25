using AhlanFeekum.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AhlanFeekum.Permissions;

public class AhlanFeekumPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AhlanFeekumPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AhlanFeekumPermissions.MyPermission1, L("Permission:MyPermission1"));
        var userProfilePermission = myGroup.AddPermission(AhlanFeekumPermissions.UserProfiles.Default, L("Permission:UserProfiles"));
        userProfilePermission.AddChild(AhlanFeekumPermissions.UserProfiles.Create, L("Permission:Create"));
        userProfilePermission.AddChild(AhlanFeekumPermissions.UserProfiles.Edit, L("Permission:Edit"));
        userProfilePermission.AddChild(AhlanFeekumPermissions.UserProfiles.Delete, L("Permission:Delete"));

        var propertyFeaturePermission = myGroup.AddPermission(AhlanFeekumPermissions.PropertyFeatures.Default, L("Permission:PropertyFeatures"));
        propertyFeaturePermission.AddChild(AhlanFeekumPermissions.PropertyFeatures.Create, L("Permission:Create"));
        propertyFeaturePermission.AddChild(AhlanFeekumPermissions.PropertyFeatures.Edit, L("Permission:Edit"));
        propertyFeaturePermission.AddChild(AhlanFeekumPermissions.PropertyFeatures.Delete, L("Permission:Delete"));

        var propertyTypePermission = myGroup.AddPermission(AhlanFeekumPermissions.PropertyTypes.Default, L("Permission:PropertyTypes"));
        propertyTypePermission.AddChild(AhlanFeekumPermissions.PropertyTypes.Create, L("Permission:Create"));
        propertyTypePermission.AddChild(AhlanFeekumPermissions.PropertyTypes.Edit, L("Permission:Edit"));
        propertyTypePermission.AddChild(AhlanFeekumPermissions.PropertyTypes.Delete, L("Permission:Delete"));

        var sitePropertyPermission = myGroup.AddPermission(AhlanFeekumPermissions.SiteProperties.Default, L("Permission:SiteProperties"));
        sitePropertyPermission.AddChild(AhlanFeekumPermissions.SiteProperties.Create, L("Permission:Create"));
        sitePropertyPermission.AddChild(AhlanFeekumPermissions.SiteProperties.Edit, L("Permission:Edit"));
        sitePropertyPermission.AddChild(AhlanFeekumPermissions.SiteProperties.Delete, L("Permission:Delete"));

        var favoritePropertyPermission = myGroup.AddPermission(AhlanFeekumPermissions.FavoriteProperties.Default, L("Permission:FavoriteProperties"));
        favoritePropertyPermission.AddChild(AhlanFeekumPermissions.FavoriteProperties.Create, L("Permission:Create"));
        favoritePropertyPermission.AddChild(AhlanFeekumPermissions.FavoriteProperties.Edit, L("Permission:Edit"));
        favoritePropertyPermission.AddChild(AhlanFeekumPermissions.FavoriteProperties.Delete, L("Permission:Delete"));

        var personEvaluationPermission = myGroup.AddPermission(AhlanFeekumPermissions.PersonEvaluations.Default, L("Permission:PersonEvaluations"));
        personEvaluationPermission.AddChild(AhlanFeekumPermissions.PersonEvaluations.Create, L("Permission:Create"));
        personEvaluationPermission.AddChild(AhlanFeekumPermissions.PersonEvaluations.Edit, L("Permission:Edit"));
        personEvaluationPermission.AddChild(AhlanFeekumPermissions.PersonEvaluations.Delete, L("Permission:Delete"));

        var propertyEvaluationPermission = myGroup.AddPermission(AhlanFeekumPermissions.PropertyEvaluations.Default, L("Permission:PropertyEvaluations"));
        propertyEvaluationPermission.AddChild(AhlanFeekumPermissions.PropertyEvaluations.Create, L("Permission:Create"));
        propertyEvaluationPermission.AddChild(AhlanFeekumPermissions.PropertyEvaluations.Edit, L("Permission:Edit"));
        propertyEvaluationPermission.AddChild(AhlanFeekumPermissions.PropertyEvaluations.Delete, L("Permission:Delete"));

        var propertyMediaPermission = myGroup.AddPermission(AhlanFeekumPermissions.PropertyMedias.Default, L("Permission:PropertyMedias"));
        propertyMediaPermission.AddChild(AhlanFeekumPermissions.PropertyMedias.Create, L("Permission:Create"));
        propertyMediaPermission.AddChild(AhlanFeekumPermissions.PropertyMedias.Edit, L("Permission:Edit"));
        propertyMediaPermission.AddChild(AhlanFeekumPermissions.PropertyMedias.Delete, L("Permission:Delete"));

        var verificationCodePermission = myGroup.AddPermission(AhlanFeekumPermissions.VerificationCodes.Default, L("Permission:VerificationCodes"));
        verificationCodePermission.AddChild(AhlanFeekumPermissions.VerificationCodes.Create, L("Permission:Create"));
        verificationCodePermission.AddChild(AhlanFeekumPermissions.VerificationCodes.Edit, L("Permission:Edit"));
        verificationCodePermission.AddChild(AhlanFeekumPermissions.VerificationCodes.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AhlanFeekumResource>(name);
    }
}
