using System;
using System.Collections.Generic;
using System.Text;
using Cube.FileProcessor.Options;
using Microsoft.Extensions.Logging;
using Azure.Storage.Files.Shares;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Azure;

namespace Cube.FileProcessor.Services.FileShareService
{
    public class FileShareService: IFileShareService
    {
        private readonly FileShareOptions _fileShareOptions;
        private readonly ShareClient _shareClient;
        private readonly ILogger<FileShareService> _logger;

        public FileShareService(FileShareOptions fileShareOptions, ILogger<FileShareService> logger)
        {
            _fileShareOptions = fileShareOptions;
            _shareClient = new ShareClient(fileShareOptions.StorageConnectionString, fileShareOptions.FileShareName);
            _logger = logger;
        }

        public async Task<string> GetFileContentsAsync(string fileName)
        {
            if (await _shareClient.ExistsAsync())
            {
                // Get a reference to the client uploads directory
                ShareDirectoryClient directory = _shareClient.GetDirectoryClient(_fileShareOptions.ClientUploadsDirectory);
                // Ensure that the directory exists
                if (await directory.ExistsAsync())
                {
                    ShareFileClient file = directory.GetFileClient(fileName);
                    // Ensure that the file exists
                    if (await file.ExistsAsync())
                    {
                        using var stream = await file.OpenReadAsync();
                        using var reader = new StreamReader(stream);
                        
                        return await reader.ReadToEndAsync();
                        
                    }
                }
            }
            return null;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            if (await _shareClient.ExistsAsync())
            {
                // Get a reference to the client uploads directory
                ShareDirectoryClient directory = _shareClient.GetDirectoryClient(_fileShareOptions.ClientUploadsDirectory); 
                // Ensure that the directory exists
                if (await directory.ExistsAsync())
                {
                    ShareFileClient file = directory.GetFileClient(fileName);
                    try
                    {
                        await file.DeleteIfExistsAsync();
                    }
                    catch (RequestFailedException ex)
                    {
                        _logger.LogError("Error occured while deleting the file {0} . ErrorMessage {1}, InnerException",
                            fileName, ex.Message, ex.InnerException);

                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error occured while deleting the file {0} . ErrorMessage {1}, InnerException",
                           fileName, ex.Message, ex.InnerException);

                        throw ex;
                    }

                }
            }
        }

        public async Task SaveFileAsync(string fileName, string fileContents)
        {
            if (await _shareClient.ExistsAsync())
            {
                try
                {
                    // Get a reference to the client uploads directory
                    ShareDirectoryClient directory = _shareClient.GetDirectoryClient("ProcessedFiles");
                    
                    await directory.CreateIfNotExistsAsync();
                    ShareFileClient file = directory.GetFileClient(fileName);

                    using MemoryStream stream = new MemoryStream();
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.Write(fileContents);
                    writer.Flush();

                    await file.CreateAsync(stream.Length);
                    await file.UploadRangeAsync(
                         new HttpRange(0, stream.Length),
                         stream);
                }
                catch(RequestFailedException ex)
                {
                    _logger.LogError("Error occured while saving the file {0} . ErrorMessage {1}, InnerException",
                        fileName, ex.Message, ex.InnerException);

                    throw ex;
                }
                catch(Exception ex)
                {
                    _logger.LogError("Error occured while saving the file {0} . ErrorMessage {1}, InnerException",
                       fileName, ex.Message, ex.InnerException);

                    throw ex;
                }
            }
        }
    }
}
