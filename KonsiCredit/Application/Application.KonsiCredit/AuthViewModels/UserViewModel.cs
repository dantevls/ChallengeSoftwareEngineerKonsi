using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.KonsiCredit.AuthViewModels;

public class UserViewModel
{
    [Required(ErrorMessage = "O Nome de Usuário é Obrigatório")]
    [JsonPropertyName("username")]
    public string username { get; set; }
    
    [Required(ErrorMessage = "A Senha é Obrigatória")]
    [JsonPropertyName("password")]
    public string password { get; set; }
}