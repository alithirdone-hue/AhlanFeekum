using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace AhlanFeekum.AhlanfeekumTerms
{
    public abstract class AhlanfeekumTermManagerBase : DomainService
    {
        protected IAhlanfeekumTermRepository _ahlanfeekumTermRepository;

        public AhlanfeekumTermManagerBase(IAhlanfeekumTermRepository ahlanfeekumTermRepository)
        {
            _ahlanfeekumTermRepository = ahlanfeekumTermRepository;
        }

        public virtual async Task<AhlanfeekumTerm> CreateAsync(
        string termsTitle, string termsAnnotation, string termsDescription, Guid termsIconId, string termsIconExtension, string whoAreWeTitle, string whoAreWeDescription, Guid whoAreWeIconId, string whoAreWeIconExtension, bool isActive, string? whoAreWeAnnotation = null)
        {
            Check.NotNullOrWhiteSpace(termsTitle, nameof(termsTitle));
            Check.NotNullOrWhiteSpace(termsAnnotation, nameof(termsAnnotation));
            Check.NotNullOrWhiteSpace(termsDescription, nameof(termsDescription));
            Check.NotNullOrWhiteSpace(termsIconExtension, nameof(termsIconExtension));
            Check.NotNullOrWhiteSpace(whoAreWeTitle, nameof(whoAreWeTitle));
            Check.NotNullOrWhiteSpace(whoAreWeDescription, nameof(whoAreWeDescription));
            Check.NotNullOrWhiteSpace(whoAreWeIconExtension, nameof(whoAreWeIconExtension));

            var ahlanfeekumTerm = new AhlanfeekumTerm(
             GuidGenerator.Create(),
             termsTitle, termsAnnotation, termsDescription, termsIconId, termsIconExtension, whoAreWeTitle, whoAreWeDescription, whoAreWeIconId, whoAreWeIconExtension, isActive, whoAreWeAnnotation
             );

            return await _ahlanfeekumTermRepository.InsertAsync(ahlanfeekumTerm);
        }

        public virtual async Task<AhlanfeekumTerm> UpdateAsync(
            Guid id,
            string termsTitle, string termsAnnotation, string termsDescription, Guid termsIconId, string termsIconExtension, string whoAreWeTitle, string whoAreWeDescription, Guid whoAreWeIconId, string whoAreWeIconExtension, bool isActive, string? whoAreWeAnnotation = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(termsTitle, nameof(termsTitle));
            Check.NotNullOrWhiteSpace(termsAnnotation, nameof(termsAnnotation));
            Check.NotNullOrWhiteSpace(termsDescription, nameof(termsDescription));
            Check.NotNullOrWhiteSpace(termsIconExtension, nameof(termsIconExtension));
            Check.NotNullOrWhiteSpace(whoAreWeTitle, nameof(whoAreWeTitle));
            Check.NotNullOrWhiteSpace(whoAreWeDescription, nameof(whoAreWeDescription));
            Check.NotNullOrWhiteSpace(whoAreWeIconExtension, nameof(whoAreWeIconExtension));

            var ahlanfeekumTerm = await _ahlanfeekumTermRepository.GetAsync(id);

            ahlanfeekumTerm.TermsTitle = termsTitle;
            ahlanfeekumTerm.TermsAnnotation = termsAnnotation;
            ahlanfeekumTerm.TermsDescription = termsDescription;
            ahlanfeekumTerm.TermsIconId = termsIconId;
            ahlanfeekumTerm.TermsIconExtension = termsIconExtension;
            ahlanfeekumTerm.WhoAreWeTitle = whoAreWeTitle;
            ahlanfeekumTerm.WhoAreWeDescription = whoAreWeDescription;
            ahlanfeekumTerm.WhoAreWeIconId = whoAreWeIconId;
            ahlanfeekumTerm.WhoAreWeIconExtension = whoAreWeIconExtension;
            ahlanfeekumTerm.IsActive = isActive;
            ahlanfeekumTerm.WhoAreWeAnnotation = whoAreWeAnnotation;

            ahlanfeekumTerm.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _ahlanfeekumTermRepository.UpdateAsync(ahlanfeekumTerm);
        }

    }
}