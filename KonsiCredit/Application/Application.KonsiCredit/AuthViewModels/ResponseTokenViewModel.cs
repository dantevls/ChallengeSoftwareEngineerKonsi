namespace Application.KonsiCredit.AuthViewModels;

public class ResponseTokenViewModel
{
    public bool success { get; set; }
    
    public Data data { get; set; }
}

public class Data
{
    public string token { get; set; } 
}