using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.KonsiCredit.UserBenefitsViewModels;

public class UserBenefitsViewModel
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("data")]
    public Data Data { get; set; }

}

public class Data
{
    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }
    
    [DisplayName("beneficios")]
    public Benefits[] Beneficios { get; set; } 
}

public class Benefits
{
    [DisplayName("numero_beneficio")]
    public string BenefitNumber { get; set; }
    
    [DisplayName("codigo_tipo_beneficio")]
    public int CodeBenefitType { get; set; }
    
}