﻿using Optimizely.Graph.Source.Sdk.Model;

namespace Optimizely.Graph.Source.Sdk.Repository
{
    public interface IGraphSourceTypeRepository : IGraphSourceRepository
    {
        SourceConfigurationModel<T> Configure<T>() where T : class, new();

        Task<string> SaveTypesAsync();

        void AddLanguage(string language);
    }
}
