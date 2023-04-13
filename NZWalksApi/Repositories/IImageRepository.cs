using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
