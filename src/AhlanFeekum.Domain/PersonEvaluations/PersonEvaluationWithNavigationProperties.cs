using AhlanFeekum.UserProfiles;
using AhlanFeekum.UserProfiles;

using System;
using System.Collections.Generic;

namespace AhlanFeekum.PersonEvaluations
{
    public abstract class PersonEvaluationWithNavigationPropertiesBase
    {
        public PersonEvaluation PersonEvaluation { get; set; } = null!;

        public UserProfile Evaluator { get; set; } = null!;
        public UserProfile EvaluatedPerson { get; set; } = null!;
        

        
    }
}