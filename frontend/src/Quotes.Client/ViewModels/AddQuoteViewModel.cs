using System.ComponentModel.DataAnnotations;

namespace Quotes.Client.ViewModels;

public record AddQuoteViewModel
{

    [Required]
    public string Message { get; set; }

    public AddQuoteViewModel(string message)
    {
        Message = message;
       
    }

    public AddQuoteViewModel()
    {

    }
}
