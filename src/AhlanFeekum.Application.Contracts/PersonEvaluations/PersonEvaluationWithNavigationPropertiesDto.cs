using AhlanFeekum.UserProfiles;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace AhlanFeekum.PersonEvaluations
{
    public abstract class PersonEvaluationWithNavigationPropertiesDtoBase
    {
        public PersonEvaluationDto PersonEvaluation { get; set; } = null!;

        public UserProfileDto Evaluator { get; set; } = null!;
        public UserProfileDto EvaluatedPerson { get; set; } = null!;

    }
}