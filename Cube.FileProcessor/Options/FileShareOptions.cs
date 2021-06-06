using System;
using System.ComponentModel.DataAnnotations;

namespace Cube.FileProcessor.Options
{
   public class FileShareOptions
    {
        [Required]
        public string StorageConnectionString { get; set; }
        [Required]
        public string FileShareName { get; set; }
        [Required]
        public string ClientUploadsDirectory { get; set; }
        [Required]
        public string ProcessedFiesDirectory { get; set; }
    }
}
