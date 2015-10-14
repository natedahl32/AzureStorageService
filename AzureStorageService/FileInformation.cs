using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService
{
    public class FileInformation
    {
        #region Constructors

        public FileInformation(Stream stream)
        {
            this.Stream = stream;
        }

        public FileInformation(Stream stream, string contentType) : this(stream)
        {
            this.ContentType = contentType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stream containing the file contents
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Content type of the file
        /// </summary>
        public string ContentType { get; set; }

        #endregion
    }
}
