using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Model.Entity;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column]
    public string Username { get; set; }
    
    [Required]
    [Column]
    [MaxLength(100)]
    public Guid ExternalId { get; set; }
    
    [Required]
    [Column]
    public Role Role { get; set; }
}