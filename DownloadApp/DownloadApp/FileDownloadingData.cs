using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DownloadApp
{
    public class FileDownloadingData
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}
