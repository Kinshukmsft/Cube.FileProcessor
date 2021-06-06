# Cube FileProcessor
Assuming there is a File watcher function that creates a message in the queue when file is uploaded in the FileShare.
The message id contains FileName and FileIdentifier which is a Guid.

The FileProcessingFunction is ServiceBus queue triggered function that process the files...