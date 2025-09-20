using AhlanFeekum.FavoriteProperties;
using AhlanFeekum.Governorates;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.PersonEvaluations;
using AhlanFeekum.PropertyCalendars;
using AhlanFeekum.PropertyEvaluations;
using AhlanFeekum.PropertyFeatures;
using AhlanFeekum.PropertyMedias;
using AhlanFeekum.PropertyTypes;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SiteProperties;
using AhlanFeekum.SpecialAdvertisments;
using AhlanFeekum.UserProfiles;
using AhlanFeekum.OnlyForYouSections;
using AhlanFeekum.SpecialAdvertisments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace AhlanFeekum.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class AhlanFeekumDbContext :
    AbpDbContext<AhlanFeekumDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<AppFileDescriptors.AppFileDescriptor> AppFileDescriptors { get; set; } = null!;
    public DbSet<PropertyCalendar> PropertyCalendars { get; set; } = null!;
    public DbSet<OnlyForYouSection> OnlyForYouSections { get; set; } = null!;
    public DbSet<SpecialAdvertisment> SpecialAdvertisments { get; set; } = null!;
    public DbSet<Governorate> Governorates { get; set; } = null!;
    public DbSet<PropertyMedia> PropertyMedias { get; set; } = null!;
    public DbSet<PropertyEvaluation> PropertyEvaluations { get; set; } = null!;
    public DbSet<PersonEvaluation> PersonEvaluations { get; set; } = null!;
    public DbSet<FavoriteProperty> FavoriteProperties { get; set; } = null!;
    public DbSet<SiteProperty> SiteProperties { get; set; } = null!;
    public DbSet<PropertyType> PropertyTypes { get; set; } = null!;
    public DbSet<PropertyFeature> PropertyFeatures { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public AhlanFeekumDbContext(DbContextOptions<AhlanFeekumDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(AhlanFeekumConsts.DbTablePrefix + "YourEntities", AhlanFeekumConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PropertyFeature>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PropertyFeatures", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Title).HasColumnName(nameof(PropertyFeature.Title)).IsRequired();
                b.Property(x => x.Icon).HasColumnName(nameof(PropertyFeature.Icon)).IsRequired();
                b.Property(x => x.Order).HasColumnName(nameof(PropertyFeature.Order));
                b.Property(x => x.IsActive).HasColumnName(nameof(PropertyFeature.IsActive));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<UserProfile>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "UserProfiles", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(UserProfile.Name)).IsRequired();
                b.Property(x => x.Email).HasColumnName(nameof(UserProfile.Email));
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(UserProfile.PhoneNumber));
                b.Property(x => x.Latitude).HasColumnName(nameof(UserProfile.Latitude));
                b.Property(x => x.Longitude).HasColumnName(nameof(UserProfile.Longitude));
                b.Property(x => x.Address).HasColumnName(nameof(UserProfile.Address));
                b.Property(x => x.ProfilePhoto).HasColumnName(nameof(UserProfile.ProfilePhoto));
                b.Property(x => x.IsSuperHost).HasColumnName(nameof(UserProfile.IsSuperHost));
                b.HasOne<IdentityRole>().WithMany().HasForeignKey(x => x.IdentityRoleId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<IdentityUser>().WithMany().IsRequired().HasForeignKey(x => x.IdentityUserId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PropertyType>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PropertyTypes", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Title).HasColumnName(nameof(PropertyType.Title)).IsRequired();
                b.Property(x => x.Order).HasColumnName(nameof(PropertyType.Order));
                b.Property(x => x.IsActive).HasColumnName(nameof(PropertyType.IsActive));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<FavoriteProperty>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "FavoriteProperties", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasOne<UserProfile>().WithMany().IsRequired().HasForeignKey(x => x.UserProfileId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PersonEvaluation>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PersonEvaluations", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Rate).HasColumnName(nameof(PersonEvaluation.Rate)).IsRequired().HasMaxLength(PersonEvaluationConsts.RateMaxLength);
                b.Property(x => x.Comment).HasColumnName(nameof(PersonEvaluation.Comment));
                b.HasOne<UserProfile>().WithMany().IsRequired().HasForeignKey(x => x.EvaluatorId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<UserProfile>().WithMany().IsRequired().HasForeignKey(x => x.EvaluatedPersonId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PropertyEvaluation>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PropertyEvaluations", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Cleanliness).HasColumnName(nameof(PropertyEvaluation.Cleanliness)).IsRequired().HasMaxLength(PropertyEvaluationConsts.CleanlinessMaxLength);
                b.Property(x => x.PriceAndValue).HasColumnName(nameof(PropertyEvaluation.PriceAndValue)).IsRequired().HasMaxLength(PropertyEvaluationConsts.PriceAndValueMaxLength);
                b.Property(x => x.Location).HasColumnName(nameof(PropertyEvaluation.Location)).IsRequired().HasMaxLength(PropertyEvaluationConsts.LocationMaxLength);
                b.Property(x => x.Accuracy).HasColumnName(nameof(PropertyEvaluation.Accuracy)).IsRequired().HasMaxLength(PropertyEvaluationConsts.AccuracyMaxLength);
                b.Property(x => x.Attitude).HasColumnName(nameof(PropertyEvaluation.Attitude)).IsRequired().HasMaxLength(PropertyEvaluationConsts.AttitudeMaxLength);
                b.Property(x => x.RatingComment).HasColumnName(nameof(PropertyEvaluation.RatingComment));
                b.HasOne<UserProfile>().WithMany().IsRequired().HasForeignKey(x => x.UserProfileId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PropertyMedia>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PropertyMedias", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Image).HasColumnName(nameof(PropertyMedia.Image)).IsRequired();
                b.Property(x => x.Order).HasColumnName(nameof(PropertyMedia.Order));
                b.Property(x => x.isActive).HasColumnName(nameof(PropertyMedia.isActive));
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Governorate>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "Governorates", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Title).HasColumnName(nameof(Governorate.Title)).IsRequired();
                b.Property(x => x.Order).HasColumnName(nameof(Governorate.Order));
                b.Property(x => x.IsActive).HasColumnName(nameof(Governorate.IsActive));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SiteProperty>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "SiteProperties", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.PropertyTitle).HasColumnName(nameof(SiteProperty.PropertyTitle)).IsRequired();
                b.Property(x => x.HotelName).HasColumnName(nameof(SiteProperty.HotelName));
                b.Property(x => x.Bedrooms).HasColumnName(nameof(SiteProperty.Bedrooms));
                b.Property(x => x.Bathrooms).HasColumnName(nameof(SiteProperty.Bathrooms));
                b.Property(x => x.NumberOfBed).HasColumnName(nameof(SiteProperty.NumberOfBed));
                b.Property(x => x.Floor).HasColumnName(nameof(SiteProperty.Floor));
                b.Property(x => x.MaximumNumberOfGuest).HasColumnName(nameof(SiteProperty.MaximumNumberOfGuest));
                b.Property(x => x.Livingrooms).HasColumnName(nameof(SiteProperty.Livingrooms));
                b.Property(x => x.PropertyDescription).HasColumnName(nameof(SiteProperty.PropertyDescription)).IsRequired();
                b.Property(x => x.HourseRules).HasColumnName(nameof(SiteProperty.HourseRules));
                b.Property(x => x.ImportantInformation).HasColumnName(nameof(SiteProperty.ImportantInformation));
                b.Property(x => x.Address).HasColumnName(nameof(SiteProperty.Address));
                b.Property(x => x.StreetAndBuildingNumber).HasColumnName(nameof(SiteProperty.StreetAndBuildingNumber));
                b.Property(x => x.LandMark).HasColumnName(nameof(SiteProperty.LandMark));
                b.Property(x => x.PricePerNight).HasColumnName(nameof(SiteProperty.PricePerNight));
                b.Property(x => x.IsActive).HasColumnName(nameof(SiteProperty.IsActive));
                b.HasOne<PropertyType>().WithMany().IsRequired().HasForeignKey(x => x.PropertyTypeId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Governorate>().WithMany().IsRequired().HasForeignKey(x => x.GovernorateId).OnDelete(DeleteBehavior.NoAction);
                b.HasMany(x => x.PropertyFeatures).WithOne().HasForeignKey(x => x.SitePropertyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<SitePropertyPropertyFeature>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "SitePropertyPropertyFeature", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasKey(
                    x => new { x.SitePropertyId, x.PropertyFeatureId }
                );

                b.HasOne<SiteProperty>().WithMany(x => x.PropertyFeatures).HasForeignKey(x => x.SitePropertyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasOne<PropertyFeature>().WithMany().HasForeignKey(x => x.PropertyFeatureId).IsRequired().OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(
                        x => new { x.SitePropertyId, x.PropertyFeatureId }
                );
            });
        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PropertyCalendar>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "PropertyCalendars", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Date).HasColumnName(nameof(PropertyCalendar.Date));
                b.Property(x => x.IsAvailable).HasColumnName(nameof(PropertyCalendar.IsAvailable));
                b.Property(x => x.Price).HasColumnName(nameof(PropertyCalendar.Price));
                b.Property(x => x.Note).HasColumnName(nameof(PropertyCalendar.Note));
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<SpecialAdvertisment>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "SpecialAdvertisments", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ImageId).HasColumnName(nameof(SpecialAdvertisment.ImageId));
                b.Property(x => x.Order).HasColumnName(nameof(SpecialAdvertisment.Order));
                b.Property(x => x.IsActive).HasColumnName(nameof(SpecialAdvertisment.IsActive));
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<OnlyForYouSection>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "OnlyForYouSections", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstPhotoId).HasColumnName(nameof(OnlyForYouSection.FirstPhotoId));
                b.Property(x => x.SecondPhotoId).HasColumnName(nameof(OnlyForYouSection.SecondPhotoId));
                b.Property(x => x.ThirdPhotoId).HasColumnName(nameof(OnlyForYouSection.ThirdPhotoId));
                b.Property(x => x.FirstPhotoExtension).HasColumnName(nameof(OnlyForYouSection.FirstPhotoExtension)).IsRequired();
                b.Property(x => x.SecondPhotoExtension).HasColumnName(nameof(OnlyForYouSection.SecondPhotoExtension)).IsRequired();
                b.Property(x => x.ThirdPhotoExtension).HasColumnName(nameof(OnlyForYouSection.ThirdPhotoExtension)).IsRequired();
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SpecialAdvertisment>(b =>
            {
                b.ToTable(AhlanFeekumConsts.DbTablePrefix + "SpecialAdvertisments", AhlanFeekumConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ImageId).HasColumnName(nameof(SpecialAdvertisment.ImageId));
                b.Property(x => x.ImageExtension).HasColumnName(nameof(SpecialAdvertisment.ImageExtension)).IsRequired();
                b.Property(x => x.Order).HasColumnName(nameof(SpecialAdvertisment.Order));
                b.Property(x => x.IsActive).HasColumnName(nameof(SpecialAdvertisment.IsActive));
                b.HasOne<SiteProperty>().WithMany().IsRequired().HasForeignKey(x => x.SitePropertyId).OnDelete(DeleteBehavior.NoAction);
            });

        }
        builder.Entity<AppFileDescriptors.AppFileDescriptor>(b =>
        {
            b.ToTable(AhlanFeekumConsts.DbTablePrefix + "FileDescriptors", AhlanFeekumConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name);
            b.Property(x => x.MimeType);
        });
    }


}