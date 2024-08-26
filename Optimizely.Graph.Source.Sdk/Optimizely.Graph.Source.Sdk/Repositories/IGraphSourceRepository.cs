using Optimizely.Graph.Source.Sdk.SourceConfiguration;

namespace Optimizely.Graph.Source.Sdk.Repositories
{
    public interface IGraphSourceRepository
    {
        /// <summary>
        /// Adds language preference to SourceConfigurationModel.
        /// </summary>
        /// <param name="language"></param>
        void AddLanguage(string language);

        /// <summary>
        /// Configures Content Types within the SourceConfigurationModel.
        /// </summary>
        /// <typeparam name="T">Generic content type.</typeparam>
        /// <returns></returns>
        SourceConfigurationModel<T> ConfigureContentType<T>() where T : class, new();

        /// <summary>
        /// Configures Content Property Types within the SourceConfigurationModel.
        /// </summary>
        /// <typeparam name="T">Generic property type.</typeparam>
        /// <returns></returns>
        SourceConfigurationModel<T> ConfigurePropertyType<T>() where T : class, new();

        /// <summary>
        /// Saves Content Types set in the SourceConfigurationModel to the Content Graph api.
        /// </summary>
        /// <returns></returns>
        Task<string> SaveTypesAsync();

        /// <summary>
        /// Saves dynamic content sent in data array to the Content Graph api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="generateId">Id associated with content.</param>
        /// <param name="data">Dynamic data being saved to Content Graph.</param>
        /// <returns></returns>
        Task<string> SaveContentAsync<T>(Func<T, string> generateId, params T[] data) where T : class, new();

        /// <summary>
        /// Removes content previously saved from the Content Graph api.
        /// </summary>
        /// <param name="id">Id of the content being removed.</param>
        /// <returns></returns>
        Task<string> DeleteContentAsync(string id);
    }
}
