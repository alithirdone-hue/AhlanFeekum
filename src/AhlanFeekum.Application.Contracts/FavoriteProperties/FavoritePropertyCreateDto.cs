using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AhlanFeekum.FavoriteProperties
{
    public abstract class FavoritePropertyCreateDtoBase
    {

        public Guid UserProfileId { get; set; }
        public Guid SitePropertyId { get; set; }
    }
}