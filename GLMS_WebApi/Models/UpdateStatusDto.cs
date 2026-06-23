using System.ComponentModel.DataAnnotations;

namespace GLMS.API.Models
{
    public class UpdateStatusDto
    {
        [Required]
        [RegularExpression("Active|Draft|Expired|Approved|Declined",
            ErrorMessage = "Status must be one of: Active, Draft, Expired, Approved, Declined")]
        public string Status { get; set; } = string.Empty;
    }
}