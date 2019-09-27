﻿using EPiServer.ServiceLocation;
using FileHelpers;
using System.IO;

namespace Foundation.Commerce.Customer.Services
{
    [ServiceConfiguration(typeof(IFileHelperService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class FileHelperService : IFileHelperService
    {
        public T[] GetImportData<T>(Stream file) where T : class
        {
            var reader = new StreamReader(file);

            var fileEngine = new FileHelperEngine(typeof(T));
            fileEngine.ErrorManager.ErrorMode = ErrorMode.IgnoreAndContinue;

            return fileEngine.ReadStream(reader, int.MaxValue) as T[];
        }
    }
}