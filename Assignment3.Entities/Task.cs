using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Assignment3.Core;
namespace Assignment3.Entities;

public class Task
{
    
    public int Id { get; set; }
    
    [StringLength(100),Required]
    public string Title { get; set; }
    
    [MaxLength]
    public string Description { get; set; }
    
    [Required]
    public State State { get; set; }

    public virtual ICollection<Tag> Tags { get; set; }



}
