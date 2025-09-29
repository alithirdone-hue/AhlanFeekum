using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string TermsTitle { get; set; }

        [NotNull]
        public virtual string TermsAnnotation { get; set; }

        [NotNull]
        public virtual string TermsDescription { get; set; }

        public virtual Guid TermsIconId { get; set; }

        [NotNull]
        public virtual string TermsIconExtension { get; set; }

        [NotNull]
        public virtual string WhoAreWeTitle { get; set; }

        [CanBeNull]
        public virtual string? WhoAreWeAnnotation { get; set; }

        [NotNull]
        public virtual string WhoAreWeDescription { get; set; }

        public virtual Guid WhoAreWeIconId { get; set; }

        [NotNull]
        public virtual string WhoAreWeIconExtension { get; set; }

        public virtual bool IsActive { get; set; }

        protected AhlanfeekumTermBase()
        {

        }

        public AhlanfeekumTermBase(Guid id, string termsTitle, string termsAnnotation, string termsDescription, Guid termsIconId, string termsIconExtension, string whoAreWeTitle, string whoAreWeDescription, Guid whoAreWeIconId, string whoAreWeIconExtension, bool isActive, string? whoAreWeAnnotation = null)
        {

            Id = id;
            Check.NotNull(termsTitle, nameof(termsTitle));
            Check.NotNull(termsAnnotation, nameof(termsAnnotation));
            Check.NotNull(termsDescription, nameof(termsDescription));
            Check.NotNull(termsIconExtension, nameof(termsIconExtension));
            Check.NotNull(whoAreWeTitle, nameof(whoAreWeTitle));
            Check.NotNull(whoAreWeDescription, nameof(whoAreWeDescription));
            Check.NotNull(whoAreWeIconExtension, nameof(whoAreWeIconExtension));
            TermsTitle = termsTitle;
            TermsAnnotation = termsAnnotation;
            TermsDescription = termsDescription;
            TermsIconId = termsIconId;
            TermsIconExtension = termsIconExtension;
            WhoAreWeTitle = whoAreWeTitle;
            WhoAreWeDescription = whoAreWeDescription;
            WhoAreWeIconId = whoAreWeIconId;
            WhoAreWeIconExtension = whoAreWeIconExtension;
            IsActive = isActive;
            WhoAreWeAnnotation = whoAreWeAnnotation;
        }

    }
}