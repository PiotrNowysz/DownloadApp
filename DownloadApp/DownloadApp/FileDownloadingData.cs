using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DownloadApp
{
    /// <summary>
    /// Class that contains user data
    /// </summary>
    public class FileDownloadingData
    {
        /// <summary>
        /// The url to download website from
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Url { get; set; }
        /// <summary>
        /// The name of the file to save to
        /// </summary>
        public string FileName { get; set; }
    }
}
