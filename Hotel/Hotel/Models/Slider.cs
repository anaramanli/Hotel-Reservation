using Hotel.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models;

public class Slider : BaseEntity
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
}
