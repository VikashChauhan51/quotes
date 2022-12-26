
namespace Quotes.API.Profiles;

public class QuoteProfile: Profile
{
	public QuoteProfile()
	{
        // map from Quote (entity) to Quote (model), and back
        CreateMap<Quote, QuoteModel>().ReverseMap();
    }
}
