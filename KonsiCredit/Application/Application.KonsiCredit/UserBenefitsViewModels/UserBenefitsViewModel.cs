using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.KonsiCredit.UserBenefitsViewModels;

public class UserBenefitsViewModel
{
    public UserBenefitsViewModel()
    {
        
    }
    public UserBenefitsViewModel(Data data)
    {
        this.data = data;
    }
    /// <summary>
    /// Status of return
    /// </summary>
    public bool success { get; set; }
    
    /// <summary>
    /// Object with benefits values
    /// </summary>
    public Data data { get; set; }

}

public class Data
{
    public Data(string cpf, Benefits[] beneficios)
    {
        this.cpf = cpf;
        this.beneficios = beneficios;
    }
    /// <summary>
    /// Identifier
    /// </summary>
    public string cpf { get; set; }
    
    /// <summary>
    /// Benefits of identifier user
    /// </summary>
    public Benefits[] beneficios { get; set; } 
}

public class Benefits
{

    public Benefits(string number, int code)
    {
        numero_beneficio = number;
        codigo_tipo_beneficio = code;
    }
    /// <summary>
    /// Benfetis number
    /// </summary>
    public string numero_beneficio { get; set; }
    
    /// <summary>
    /// Benefits code
    /// </summary>
    public int codigo_tipo_beneficio { get; set; }
    
}