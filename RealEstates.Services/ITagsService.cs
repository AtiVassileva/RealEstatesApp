namespace RealEstates.Services
{
    public interface ITagsService
    {
        void Add(string name, int? importance = null);

        void BulkTagToProperties();
    }
}