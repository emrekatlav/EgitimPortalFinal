using System.ComponentModel.DataAnnotations;

namespace EgitimPortalFinal.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name = "Ders Başlığı")]

        [Required(ErrorMessage = "Lütfen Ders Başlığı Giriniz!")]
        public string Title { get; set; }



        [Display(Name = "Ders Açıklaması")]

        [Required(ErrorMessage = "Lütfen Ders Açıklaması Giriniz!")]
        public string Description { get; set; }



        [Display(Name = "Ders İçeriği")]

        [Required(ErrorMessage = "Lütfen Ders İçeriği Giriniz!")]
        public string Content { get; set; }



        [Display(Name = "Ders Etiketi")]

        [Required(ErrorMessage = "Lütfen Ders Etiketi Giriniz!")]
        public string Tag { get; set; }

        [Display(Name = "Ders Fotoğrafı")]

        [Required(ErrorMessage = "Lütfen Ders Fotoğrafı Giriniz!")]
        public string PhotoUrl { get; set; }
    }
}
