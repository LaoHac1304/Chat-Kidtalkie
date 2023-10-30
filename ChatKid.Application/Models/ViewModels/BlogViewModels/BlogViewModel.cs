using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Application.Models.ViewModels.TypeBlogViewModels;

namespace ChatKid.Application.Models.ViewModels.BlogViewModels
{
    public class BlogViewModel
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VoiceUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public short? Status { get; set; }
        public TypeBlogViewModel? TypeBlog { get; set; }
        public AdminViewModel? CreateAdmin { get; set; }
    }
}
