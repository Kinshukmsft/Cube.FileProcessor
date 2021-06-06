using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Cube.FileProcessor.Messages;
using Cube.FileProcessor.Services.SearchService;
using Cube.FileProcessor.Services.FileShareService;

namespace Cube.FileProcessor
{
    public class FileProcessingFunction
    {
        private readonly ISearchService _searchService;
        private readonly IFileShareService _fileShareService;

        public FileProcessingFunction(IFileShareService fileShareService, ISearchService searchService)
        {
            _fileShareService = fileShareService;
            _searchService = searchService;
        }

        [FunctionName("FileProcessingFunction")]
        public async void Run([ServiceBusTrigger("%fileUploadedQueueName%", Connection = "ProcessFileServiceBus")] FileUploadedMessage message, ILogger log)
        {
            log.LogInformation("Processing client file name", message.FriendlyName);
            if (string.IsNullOrEmpty(message.FileIdentifier) && string.IsNullOrEmpty(message.FriendlyName))
            {
                log.LogError("Invalid request");
                return;
            }
            
            // get the file contents
            var fileContent = await _fileShareService.GetFileContentsAsync(message.FileIdentifier);

            // set the regex pattern you want to search
            log.LogInformation("Check if the file {0} contains matching pattern", message.FriendlyName);
            _searchService.RegexPattern = "/test*/g";
            if (_searchService.ContainsAny(fileContent))
            {
                // Save the file to another directory
                log.LogInformation("Save the file {0} to another directory", message.FriendlyName);
               await _fileShareService.SaveFileAsync(message.FriendlyName, fileContent);
            }
            else
            {
                // delete the file from the directory
                log.LogInformation("Delete the file {0}", message.FriendlyName);
                await _fileShareService.DeleteFileAsync(message.FileIdentifier);
            }
        }
    }
}
