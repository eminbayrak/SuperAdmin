using System.ComponentModel.DataAnnotations;

namespace SuperAdmin.Models
{
    public class CreateUser
    {
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Are you sure? This can't be a name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Are you sure? This can't be a name")]
        public string LastName { get; set; }
        [Required]
        public Buildings Building { get; set; }
        [Required]
        public int? SSN { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Are you sure? This can't be a postion name")]
        public string Position { get; set; }

        //The account name will appear as "lastname_f"
        public string Name => LastName + "_" + (FirstName).Remove(1).ToLower();
    }

    public enum Buildings
    {
        BES, BOE, CLS, MCM, MDV, MHS, MJHS, MLB, MPS, PTS, SSA
    }
}