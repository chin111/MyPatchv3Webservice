using System.ComponentModel.DataAnnotations;

namespace MyPatchAPI.Models
{
    public class EmployeeListParameter
    {
        [Required]
        public string SupervisorID { get; set; }
    }
}