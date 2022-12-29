using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Approve;

public class EditApproveRequestModel
{
    [Required] public bool Approved { get; set; }
}