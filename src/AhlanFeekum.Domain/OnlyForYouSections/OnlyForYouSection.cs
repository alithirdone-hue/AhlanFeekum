using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace AhlanFeekum.OnlyForYouSections
{
    public abstract class OnlyForYouSectionBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid FirstPhotoId { get; set; }

        public virtual Guid SecondPhotoId { get; set; }

        public virtual Guid ThirdPhotoId { get; set; }

        [NotNull]
        public virtual string FirstPhotoExtension { get; set; }

        [NotNull]
        public virtual string SecondPhotoExtension { get; set; }

        [NotNull]
        public virtual string ThirdPhotoExtension { get; set; }

        protected OnlyForYouSectionBase()
        {

        }

        public OnlyForYouSectionBase(Guid id, Guid firstPhotoId, Guid secondPhotoId, Guid thirdPhotoId, string firstPhotoExtension, string secondPhotoExtension, string thirdPhotoExtension)
        {

            Id = id;
            Check.NotNull(firstPhotoExtension, nameof(firstPhotoExtension));
            Check.NotNull(secondPhotoExtension, nameof(secondPhotoExtension));
            Check.NotNull(thirdPhotoExtension, nameof(thirdPhotoExtension));
            FirstPhotoId = firstPhotoId;
            SecondPhotoId = secondPhotoId;
            ThirdPhotoId = thirdPhotoId;
            FirstPhotoExtension = firstPhotoExtension;
            SecondPhotoExtension = secondPhotoExtension;
            ThirdPhotoExtension = thirdPhotoExtension;
        }

    }
}