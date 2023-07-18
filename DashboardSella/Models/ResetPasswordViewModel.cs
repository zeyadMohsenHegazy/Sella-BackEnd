using System.ComponentModel.DataAnnotations;

namespace DashboardSella.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string ResetToken { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
