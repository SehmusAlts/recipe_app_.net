using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Web.Models.ViewModels;

public class CreateRecipeViewModel
{
    [Required(ErrorMessage = "Tarif adı gereklidir")]
    [StringLength(200, ErrorMessage = "Tarif adı en fazla 200 karakter olabilir")]
    [Display(Name = "Tarif Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Malzemeler gereklidir")]
    [Display(Name = "Malzemeler (her satıra bir malzeme)")]
    public string IngredientsText { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tarif gereklidir")]
    [Display(Name = "Tarif")]
    public string Instructions { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori seçiniz")]
    [Display(Name = "Kategori")]
    public RecipeCategory Category { get; set; }

    [Required(ErrorMessage = "Hazırlık süresi gereklidir")]
    [Range(1, 1440, ErrorMessage = "Hazırlık süresi 1-1440 dakika arasında olmalıdır")]
    [Display(Name = "Hazırlık Süresi (dk)")]
    public int PreparationTimeMinutes { get; set; }

    [Required(ErrorMessage = "Pişirme süresi gereklidir")]
    [Range(0, 1440, ErrorMessage = "Pişirme süresi 0-1440 dakika arasında olmalıdır")]
    [Display(Name = "Pişirme Süresi (dk)")]
    public int CookingTimeMinutes { get; set; }

    [Required(ErrorMessage = "Porsiyon sayısı gereklidir")]
    [Range(1, 100, ErrorMessage = "Porsiyon sayısı 1-100 arasında olmalıdır")]
    [Display(Name = "Porsiyon Sayısı")]
    public int Servings { get; set; }

    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    [Display(Name = "Görsel URL")]
    public string? ImageUrl { get; set; }
}
