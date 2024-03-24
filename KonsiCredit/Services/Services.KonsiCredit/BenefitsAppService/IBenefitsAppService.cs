using Application.KonsiCredit.UserBenefitsViewModels;

namespace Services.KonsiCredit.BenefitsAppService;

public interface IBenefitsAppService
{
    Task<UserBenefitsViewModel> GetUserBenefits(string cpf, string token);
}