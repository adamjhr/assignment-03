using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    
    [StringLength(100),Required]
    public string Name { get; set; }
    
    // Den mangler unique
    [StringLength(100),Required]
    public string Email { get; set; }

    public virtual List<Task> Tasks { get; set; }

}
