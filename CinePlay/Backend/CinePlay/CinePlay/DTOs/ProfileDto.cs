using MyMDb.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MyMDb.DTOs
{
    public class ProfileDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePicPath { get; set; }
    }
}
