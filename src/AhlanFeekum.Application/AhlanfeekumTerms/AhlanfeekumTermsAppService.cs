using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AhlanFeekum.Permissions;
using AhlanFeekum.AhlanfeekumTerms;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using AhlanFeekum.Shared;
using Volo.Abp.BlobStoring;

namespace AhlanFeekum.AhlanfeekumTerms
{

    [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Default)]
    public abstract class AhlanfeekumTermsAppServiceBase : AhlanFeekumAppService
    {
        protected IDistributedCache<AhlanfeekumTermDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IAhlanfeekumTermRepository _ahlanfeekumTermRepository;
        protected AhlanfeekumTermManager _ahlanfeekumTermManager;
        protected IRepository<AppFileDescriptors.AppFileDescriptor, Guid> _appFileDescriptorRepository;
        protected IBlobContainer<AhlanfeekumTermFileContainer> _blobContainer;

        public AhlanfeekumTermsAppServiceBase(IAhlanfeekumTermRepository ahlanfeekumTermRepository, AhlanfeekumTermManager ahlanfeekumTermManager, IDistributedCache<AhlanfeekumTermDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository, IBlobContainer<AhlanfeekumTermFileContainer> blobContainer)
        {
            _downloadTokenCache = downloadTokenCache;
            _ahlanfeekumTermRepository = ahlanfeekumTermRepository;
            _ahlanfeekumTermManager = ahlanfeekumTermManager;
            _appFileDescriptorRepository = appFileDescriptorRepository;
            _blobContainer = blobContainer;
        }

        public virtual async Task<PagedResultDto<AhlanfeekumTermDto>> GetListAsync(GetAhlanfeekumTermsInput input)
        {
            var totalCount = await _ahlanfeekumTermRepository.GetCountAsync(input.FilterText, input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeAnnotation, input.WhoAreWeDescription, input.WhoAreWeIconExtension, input.IsActive);
            var items = await _ahlanfeekumTermRepository.GetListAsync(input.FilterText, input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeAnnotation, input.WhoAreWeDescription, input.WhoAreWeIconExtension, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<AhlanfeekumTermDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AhlanfeekumTerm>, List<AhlanfeekumTermDto>>(items)
            };
        }

        public virtual async Task<AhlanfeekumTermDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<AhlanfeekumTerm, AhlanfeekumTermDto>(await _ahlanfeekumTermRepository.GetAsync(id));
        }

        [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _ahlanfeekumTermRepository.DeleteAsync(id);
        }

        [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Create)]
        public virtual async Task<AhlanfeekumTermDto> CreateAsync(AhlanfeekumTermCreateDto input)
        {

            var ahlanfeekumTerm = await _ahlanfeekumTermManager.CreateAsync(
            input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconId, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeDescription, input.WhoAreWeIconId, input.WhoAreWeIconExtension, input.IsActive, input.WhoAreWeAnnotation
            );

            return ObjectMapper.Map<AhlanfeekumTerm, AhlanfeekumTermDto>(ahlanfeekumTerm);
        }

        [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Edit)]
        public virtual async Task<AhlanfeekumTermDto> UpdateAsync(Guid id, AhlanfeekumTermUpdateDto input)
        {

            var ahlanfeekumTerm = await _ahlanfeekumTermManager.UpdateAsync(
            id,
            input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconId, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeDescription, input.WhoAreWeIconId, input.WhoAreWeIconExtension, input.IsActive, input.WhoAreWeAnnotation, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<AhlanfeekumTerm, AhlanfeekumTermDto>(ahlanfeekumTerm);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(AhlanfeekumTermExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _ahlanfeekumTermRepository.GetListAsync(input.FilterText, input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeAnnotation, input.WhoAreWeDescription, input.WhoAreWeIconExtension, input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<AhlanfeekumTerm>, List<AhlanfeekumTermExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "AhlanfeekumTerms.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> ahlanfeekumtermIds)
        {
            await _ahlanfeekumTermRepository.DeleteManyAsync(ahlanfeekumtermIds);
        }

        [Authorize(AhlanFeekumPermissions.AhlanfeekumTerms.Delete)]
        public virtual async Task DeleteAllAsync(GetAhlanfeekumTermsInput input)
        {
            await _ahlanfeekumTermRepository.DeleteAllAsync(input.FilterText, input.TermsTitle, input.TermsAnnotation, input.TermsDescription, input.TermsIconExtension, input.WhoAreWeTitle, input.WhoAreWeAnnotation, input.WhoAreWeDescription, input.WhoAreWeIconExtension, input.IsActive);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetFileAsync(GetFileInput input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var fileDescriptor = await _appFileDescriptorRepository.GetAsync(input.FileId);
            var extension = Path.GetExtension(fileDescriptor.Name);
            string fileName = fileDescriptor.Id.ToString("N");
            if (!string.IsNullOrWhiteSpace(extension))
            {
                fileName += extension;
            }
            var stream = await _blobContainer.GetAsync(fileName);

            return new RemoteStreamContent(stream, fileDescriptor.Name, fileDescriptor.MimeType);
        }

        public virtual async Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input)
        {
            var id = GuidGenerator.Create();
            var fileDescriptor = await _appFileDescriptorRepository.InsertAsync(new AppFileDescriptors.AppFileDescriptor(id, input.FileName, input.ContentType));

            var extension = Path.GetExtension(fileDescriptor.Name);
            string fileName = fileDescriptor.Id.ToString("N");
            if (!string.IsNullOrWhiteSpace(extension))
            {
                fileName += extension;
            }
            await _blobContainer.SaveAsync(fileName, input.GetStream());

            return ObjectMapper.Map<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>(fileDescriptor);
        }

        public virtual async Task<AhlanFeekum.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new AhlanfeekumTermDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new AhlanFeekum.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}