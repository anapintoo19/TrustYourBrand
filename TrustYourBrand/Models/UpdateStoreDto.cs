namespace TrustYourBrand.Models
{
    public class UpdateStoreDto
    {
        public int LojaId { get; set; }
        public int TenantId { get; set; }
        public int? MarcaId { get; set; }
        public string Nome { get; set; }
        public string Zona { get; set; }
        public bool IsActive { get; set; } = true;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Condition { get; set; }
        public string Inspector { get; set; }
        public string StoreManager { get; set; }
        public string StoreType { get; set; }
    }
}
