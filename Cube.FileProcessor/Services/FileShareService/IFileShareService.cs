using System.Threading.Tasks;

namespace Cube.FileProcessor.Services.FileShareService
{
   public interface IFileShareService
    {
        Task<string> GetFileContentsAsync(string fileName);
        Task DeleteFileAsync(string fileName);
        Task SaveFileAsync(string fileName, string fileContents);
    }
}
