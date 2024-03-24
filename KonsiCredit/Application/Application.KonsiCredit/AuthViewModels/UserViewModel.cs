using System.ComponentModel.DataAnnotations;

namespace Application.KonsiCredit.AuthViewModels;

public class UserViewModel
{
    [Required(ErrorMessage = "O Nome de Usuário é Obrigatório")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "A Senha é Obrigatória")]
    public string Password { get; set; }
}