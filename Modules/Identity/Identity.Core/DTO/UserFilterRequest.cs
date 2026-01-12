using System;
using System.ComponentModel.DataAnnotations;
using Identity.Core.Enums;

namespace Identity.Core.DTO;

public class UserFilterRequest
{
    //filters
    public Guid? FacultyId { get; set; }
    public UserRole? Role { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
}
