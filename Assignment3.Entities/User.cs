using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    
    [StringLength(100),Required]
    public string Name { get; set; } = null!;
    
    // Den mangler unique
    [StringLength(100),Required]
    public string Email { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = null!;

}
