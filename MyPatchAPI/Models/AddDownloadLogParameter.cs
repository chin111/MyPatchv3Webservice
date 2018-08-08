using System.ComponentModel.DataAnnotations;

namespace MyPatchAPI.Models
{
    public class AddDownloadLogParameter
    {
        [Required]
        public string DownloadDate { get; set; }
        [Required]
        public string LoginID { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string TotalTime { get; set; }
        [Required]
        public string DBPath { get; set; }
        [Required]
        public string AppVersion { get; set; }
        [Required]
        public string OSType { get; set; }
        [Required]
        public string OSVersion { get; set; }
        [Required]
        public string MacAddr { get; set; }
    }
}