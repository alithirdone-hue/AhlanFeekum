using System.Threading.Tasks;

namespace AhlanFeekum.PropertyEvaluations
{
    public partial interface IPropertyEvaluationsAppService
    {
        //Write your custom code here...
        Task<PropertyEvaluationMobileDto> CreateMobileAsync(PropertyEvaluationCreateMobileDto input);
    }
}