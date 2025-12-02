using AhlanFeekum.AhlanfeekumTerms;
using AhlanFeekum.FavoriteProperties;
using AhlanFeekum.Governorates;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.PersonEvaluations;
using AhlanFeekum.PropertyCalendars;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.Reservations;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using AhlanFeekum.Statuses;
using AhlanFeekum.Tickets;
using AhlanFeekum.UserNotifications;
using AhlanFeekum.UserPayments;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.CashPayments;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Uow;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AhlanFeekum.EntityFrameworkCore;

[DependsOn(
    typeof(AhlanFeekumDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
public class AhlanFeekumEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AhlanFeekumEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AhlanFeekumDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<UserProfile, UserProfiles.EfCoreUserProfileRepository>();

            options.AddRepository<PropertyFeature, PropertyFeatures.EfCorePropertyFeatureRepository>();

            options.AddRepository<PropertyType, PropertyTypes.EfCorePropertyTypeRepository>();

            options.AddRepository<SiteProperty, SiteProperties.EfCoreSitePropertyRepository>();

            options.AddRepository<FavoriteProperty, FavoriteProperties.EfCoreFavoritePropertyRepository>();

            options.AddRepository<PersonEvaluation, PersonEvaluations.EfCorePersonEvaluationRepository>();

            options.AddRepository<PropertyEvaluation, PropertyEvaluations.EfCorePropertyEvaluationRepository>();

            options.AddRepository<PropertyMedia, PropertyMedias.EfCorePropertyMediaRepository>();

            options.AddRepository<Governorate, Governorates.EfCoreGovernorateRepository>();

            options.AddRepository<SpecialAdvertisment, SpecialAdvertisments.EfCoreSpecialAdvertismentRepository>();

            options.AddRepository<OnlyForYouSection, OnlyForYouSections.EfCoreOnlyForYouSectionRepository>();
           
            options.AddRepository<PropertyCalendar, PropertyCalendars.EfCorePropertyCalendarRepository>();
            options.AddRepository<Status, Statuses.EfCoreStatusRepository>();
            options.AddRepository<Reservation, Reservations.EfCoreReservationRepository>();

            options.AddRepository<Ticket, Tickets.EfCoreTicketRepository>();

            options.AddRepository<AhlanfeekumTerm, AhlanfeekumTerms.EfCoreAhlanfeekumTermRepository>();
            options.AddRepository<UserNotification, UserNotifications.EfCoreUserNotificationRepository>();
           
            options.AddRepository<UserPayment, UserPayments.EfCoreUserPaymentRepository>();
            options.AddRepository<CashPayment, CashPayments.EfCoreCashPaymentRepository>();
        });

        Configure<AbpDbContextOptions>(options =>
        {
                /* The main point to change your DBMS.
                 * See also AhlanFeekumMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer();
        });

    }
}
