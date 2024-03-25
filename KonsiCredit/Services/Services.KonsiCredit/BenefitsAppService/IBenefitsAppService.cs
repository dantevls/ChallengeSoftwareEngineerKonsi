using Application.KonsiCredit.UserBenefitsViewModels;

namespace Services.KonsiCredit.BenefitsAppService;

public interface IBenefitsAppService
{
    /// <summary>
    /// Busca os beneficios do usuário
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns>Beneficios do usuário</returns>
    Task<UserBenefitsViewModel> GetUserBenefits(string cpf);
}